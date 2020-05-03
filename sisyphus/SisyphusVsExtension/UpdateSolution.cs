using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;
using EnvDTE;
using Microsoft.VisualStudioTools.Project.Automation;
using Microsoft.VisualStudio.Threading;

namespace SisyphusVsExtension
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class UpdateSolution
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("cd1de3f6-7de3-431c-b25a-858360ca81aa");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        private static DTE Dte;
        private static readonly Guid guidSisyphusOutputWindowPane = new Guid("{EDEC662F-BA1B-46A6-A76C-0125482579A3}");
        private static bool sisyphusOutputWindowPaneCreated = false;

        /// <summary>
		/// This Guid is the persistence guid for the output window.
		/// It can be found by running this sample, bringing up the output window,
		/// selecting it in the Persisted window and then looking in the Properties
		/// window.
		/// </summary>
		public static readonly Guid guidOutputWindowFrame = new Guid("{34e76e81-ee4a-11d0-ae2e-00a0c90fffc3}");

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSolution"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private UpdateSolution(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static UpdateSolution Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in UpdateSolution's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            Dte = await package.GetServiceAsync(typeof(DTE)) as DTE ?? throw new Exception("DTE not found");

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new UpdateSolution(package, commandService);
        }

        private static System.Collections.Generic.List<ProjectItem> GetAllItemsRecursive(ProjectItems items)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var result = new System.Collections.Generic.List<ProjectItem>();
            foreach (ProjectItem item in items)
            {
                result.Add(item);
                if(item.ProjectItems != null)
                {
                    result.AddRange(GetAllItemsRecursive(item.ProjectItems));
                }
            }
            return result;
        }

        private void InitSisyphusOutputWindowPane(IVsOutputWindow outWindow)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            outWindow.CreatePane(guidSisyphusOutputWindowPane, "Sisyphus", 1, 1);
            sisyphusOutputWindowPaneCreated = true;
        }

        private void LogToOutput(string text)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            // showing the output window in case it's hidden
            IVsUIShell uiShell = Package.GetGlobalService(typeof(SVsUIShell)) as IVsUIShell;
            Guid outputWindowGuid = guidOutputWindowFrame;
            IVsWindowFrame outputWindowFrame;
            uiShell.FindToolWindow((uint)__VSCREATETOOLWIN.CTW_fForceCreate, ref outputWindowGuid, out outputWindowFrame);
            if (outputWindowFrame != null)
                outputWindowFrame.Show();

            IVsOutputWindow outWindow = Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;
            if (!sisyphusOutputWindowPaneCreated)
            {
                InitSisyphusOutputWindowPane(outWindow);
            }

            IVsOutputWindowPane sisyphusPane;
            outWindow.GetPane(guidSisyphusOutputWindowPane, out sisyphusPane);
            sisyphusPane.OutputString(text);
            sisyphusPane.Activate();
        }

        private void RunPythonFileLikeVS(object pythonFileNode)
        {
            var pythonFileNodeType = pythonFileNode.GetType();
            var execCommandMethod = pythonFileNodeType.GetMethod("ExecCommandOnNode", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            Guid guidPythonToolsCmdSet = new Guid("bdfa79d2-2cd2-474a-a82a-ce8694116825");
            uint startWithoutDebuggingCmdId = 0x4004;
            execCommandMethod.Invoke(pythonFileNode, new object[] { guidPythonToolsCmdSet, startWithoutDebuggingCmdId, 0u, null, null });
        }

        private void RunPython()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            Projects projects = Dte.Solution.Projects as Projects;

            var projNames = new System.Collections.Generic.List<string>();
            Project scriptsProject = null;
            foreach (Project proj in projects)
            {
                projNames.Add(proj.Name);
                if (proj.Name == "Scripts")
                {
                    scriptsProject = proj;
                }
            }


            var itemNames = new System.Collections.Generic.List<string>();
            OAFileItem mainItem = null;
            foreach (ProjectItem item in GetAllItemsRecursive(scriptsProject.ProjectItems))
            {
                itemNames.Add(item.Name);
                if (item.Name == "generateSolution.py")
                {
                    mainItem = item as OAFileItem;
                }
            }

            OAProperties properties = mainItem.Properties as OAProperties;

            Microsoft.VisualStudioTools.Project.NodeProperties nodeProperties = properties.Target;

            object pythonFileNode = nodeProperties.Node;

            var pythonFileNodeType = pythonFileNode.GetType();

            var fileNodeType = pythonFileNodeType.BaseType.BaseType;

            var urlProperty = fileNodeType.GetProperty("Url");

            string url = urlProperty.GetValue(pythonFileNode) as string;
            string workingDir = System.IO.Path.GetDirectoryName(scriptsProject.FullName);

            var startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = "cmd.exe";
            startInfo.WorkingDirectory = workingDir;


            var cmd = new System.Diagnostics.Process();
            cmd.StartInfo = startInfo;

            // reading asynchronously to avoid deadlocks
            // it's retarded how complex it is to read some output here by the way
            string output = "";
            cmd.OutputDataReceived += new DataReceivedEventHandler((sender, e) => { output += e.Data + "\n"; });

            string error = "";
            cmd.ErrorDataReceived += new DataReceivedEventHandler((sender, e) => { error += e.Data + "\n"; });

            cmd.Start();
            cmd.StandardInput.WriteLine("python.exe " + url + " " + workingDir);
            cmd.StandardInput.Flush();
            cmd.BeginOutputReadLine();
            cmd.BeginErrorReadLine();
            cmd.StandardInput.WriteLine("exit");
            cmd.StandardInput.Flush();
            cmd.WaitForExit();

            LogToOutput(output);
            if (error != "" && error != "\n")
            {
                LogToOutput("ERROR: " + error);
            }
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            RunPython();

            sw.Stop();
            LogToOutput($"Time elapsed: {sw.Elapsed}\n");
        }
    }
}

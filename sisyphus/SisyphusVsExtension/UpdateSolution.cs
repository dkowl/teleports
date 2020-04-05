using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;
using EnvDTE;
using Microsoft.VisualStudioTools.Project.Automation;

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

            Dte = await package.GetServiceAsync(typeof(DTE)) as DTE;

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new UpdateSolution(package, commandService);
        }

        private static System.Collections.Generic.List<ProjectItem> GetAllItemsRecursive(ProjectItems items)
        {
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
            string message = string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.GetType().FullName);
            string title = "UpdateSolution";

            Projects projects = Dte.Solution.Projects as Projects;

            var projNames = new System.Collections.Generic.List<string>();
            Project scriptsProject = null;
            foreach (Project proj in projects)
            {
                projNames.Add(proj.Name);
                if(proj.Name == "Scripts")
                {
                    scriptsProject = proj;
                }
            }


            var itemNames = new System.Collections.Generic.List<string>();
            OAFileItem mainItem = null;
            foreach (ProjectItem item in GetAllItemsRecursive(scriptsProject.ProjectItems))
            {
                itemNames.Add(item.Name);
                if(item.Name == "generateSolution.py")
                {
                    mainItem = item as OAFileItem;
                }
            }

            OAProperties properties = mainItem.Properties as OAProperties;

            Microsoft.VisualStudioTools.Project.NodeProperties nodeProperties = properties.Target;

            object pythonFileNode = nodeProperties.Node;

            var pythonFileNodeType = pythonFileNode.GetType();

            var execCommandMethod = pythonFileNodeType.GetMethod("ExecCommandOnNode", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            Guid guidPythonToolsCmdSet = new Guid("bdfa79d2-2cd2-474a-a82a-ce8694116825");
            uint startWithoutDebuggingCmdId = 0x4004;
            execCommandMethod.Invoke(pythonFileNode, new object[] { guidPythonToolsCmdSet, startWithoutDebuggingCmdId, 0u, null, null });
        }
    }
}

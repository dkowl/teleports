using UnityEditor;
using UnityEngine;
using System.Linq;
using System.IO;
using SyntaxTree.VisualStudio.Unity.Bridge;
using System.Collections.Generic;

[InitializeOnLoad]
public class VisualStudioProjectAddTTFileReferences
{
    static VisualStudioProjectAddTTFileReferences()
    {
        ProjectFilesGenerator.ProjectFileGeneration += (string name, string content) =>
        {
            var lines = content.Split('\n').ToList();

            var isEditorProject = name.EndsWith(".Editor.csproj");

            var ttfileInsert = GetTTFileItemGroups(isEditorProject);

            var insertIndex = lines.Count - 1;

            for (; insertIndex >= 0; insertIndex--)
            {
                if (lines[insertIndex].Contains("</ItemGroup>"))
                {
                    insertIndex++;
                    break;
                }
            }

            lines.InsertRange(insertIndex, ttfileInsert);

            var s = string.Join("\n", lines.ToArray());

            return s;
        };
    }

    static List<string> GetTTFileItemGroups(bool isEditorProject)
    {
        var assetsPath = Application.dataPath;
        var parentPath = Directory.GetParent(assetsPath);

        var toTrimAmount = parentPath.FullName.Length + 1;

        var toReturn = new List<string>();
        foreach (var ttFile in Directory.GetFiles(assetsPath, "*.tt", SearchOption.AllDirectories))
        {
            var ttFilePath = ttFile.Replace("/", "\\");

            var isEditorPath = ttFilePath.ToLower().Contains(@"\editor\");

            if (isEditorProject && !isEditorPath)
            {
                continue;
            }

            if (!isEditorProject && isEditorPath)
            {
                continue;
            }

            var pathIncludingAssetsToTTFile = ttFilePath.Substring(toTrimAmount);
            var csFilenameOnly = pathIncludingAssetsToTTFile;

            var lastSlash = csFilenameOnly.LastIndexOf('\\');

            if (lastSlash != -1)
            {
                csFilenameOnly = csFilenameOnly.Substring(lastSlash + 1);
            }

            csFilenameOnly += ".cs";

            string formattedEntry = string.Format(@"<ItemGroup>
   <Content Include=""{0}"">
     <Generator>TextTemplatingFileGenerator</Generator>
     <LastGenOutput>{1}</LastGenOutput>
   </Content>
 </ItemGroup>", pathIncludingAssetsToTTFile, csFilenameOnly);

            toReturn.Add(formattedEntry);
        }
        return toReturn;
    }
}
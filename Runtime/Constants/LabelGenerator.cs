#if UNITY_EDITOR
using System.IO;
using System.Runtime.CompilerServices;
using Dythervin.Core.Extensions;
using UnityEditor;
using UnityEngine;

//Note: This class uses UnityEditorInternal which is an undocumented internal feature
namespace Dythervin.Game.Constants
{
    internal class LabelGenerator : EditorWindow
    {
        private static readonly string Namespace = typeof(LabelGenerator).Namespace;
        private static readonly string Tags = "Tags";
        private static readonly string Layers = "Layers";
        private static readonly string Scenes = "Scenes";
        private static readonly string Script = ".cs";


        [MenuItem("Labels/Generate labels")]
        // [UnityEditor.Callbacks.DidReloadScripts]
        private static void GenerateLabels()
        {
            // RebuildTagsAndLayersClasses("Assets/Game/Scripts/Constants");
            RebuildTagsAndLayersClassesCaller();
        }

        private static void RebuildTagsAndLayersClassesCaller([CallerFilePath] string path = "")
        {
            path = Path.GetDirectoryName(path);
            RebuildTagsAndLayersClasses(path);
        }

        private static void RebuildTagsAndLayersClasses(string path)
        {
            File.WriteAllText(GetFullPath(path, Tags), GetClassContent(Tags, UnityEditorInternal.InternalEditorUtility.tags));
            File.WriteAllText(GetFullPath(path, Layers), GetLayerClassContent(Layers, UnityEditorInternal.InternalEditorUtility.layers));
            File.WriteAllText(GetFullPath(path, Scenes),
                GetClassContent(Scenes, EditorBuildSettingsScenesToNameStrings(EditorBuildSettings.scenes)));
            Debug.Log("Rebuild Complete");
        }

        private static string GetFullPath(string path, string name)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return Path.Combine(path, $"{name}{Script}");
        }

        private static string GetPath(string path, string name)
        {
            int index = path.IndexOf("Assets");
            if (index != -1)
                path = path.Remove(0, index);
            return Path.Combine(path, $"{name}{Script}");
        }

        private static string[] EditorBuildSettingsScenesToNameStrings(EditorBuildSettingsScene[] scenes)
        {
            string[] sceneNames = new string[scenes.Length];
            for (int n = 0; n < sceneNames.Length; n++)
            {
                sceneNames[n] = Path.GetFileNameWithoutExtension(scenes[n].path);
            }

            return sceneNames;
        }

        private static string GetClassContent(string className, string[] labelsArray, string additionalMembers = null)
        {
            string output = "";
            output = $"{output}//This class is auto-generated do not modify (TagsLayersScenesBuilder.cs) - blog.almostlogical.com\n";
            output = $"{output}using System.Collections.Generic;\n\n";
            output = $"{output}namespace {Namespace}\n{{\n";

            output = $"{output}\tpublic static class {className} \n";
            output = $"{output}\t{{\n";


            if (!string.IsNullOrEmpty(additionalMembers))
                output = $"{output}{additionalMembers}";

            output = $"{output}\t\tpublic static readonly IReadOnlyList<string> All = new string[]";
            output = $"{output}\n\t\t{{\n\t\t\t\"{string.Join("\",\n\t\t\t\"", labelsArray)}\"\n\t\t}};\n\n";

            for (int i = 0; i < labelsArray.Length; i++)
            {
                output = $"{output}\t\tpublic static string {ToVarName(labelsArray[i])} => All[{i}];\n";
            }

            output = $"{output}\t}}\n}}";
            return output;
        }

        private static string GetLayerClassContent(string className, string[] labelsArray)
        {
            string output = "";
            output = $"{output}//This class is auto-generated do not modify (TagsLayersScenesBuilder.cs) - blog.almostlogical.com\n";
            output = $"{output}using UnityEngine;\n\n";
            output = $"{output}public static class {className}\n";
            output = $"{output}{{";


            //foreach (string label in labelsArray)
            //{
            //    output += "\t" + BuildConstVariable(label) + "\n";
            //}
            //output += "\n";

            //foreach (string label in labelsArray)
            //{
            //    output += "\t" + "public const int " + ToVarName(label) + "Int" + " = 1 << " + LayerMask.NameToLayer(label) + ";\n";
            //}
            foreach (string label in labelsArray)
            {
                output = output
                         + ($"\n\tpublic static class {ToVarName(label)}"
                            + "{"
                            + $"\n\t\tpublic static readonly string name = \"{label}\";"
                            + $"\n\t\tpublic const int Int = {LayerMask.NameToLayer(label)};"
                            + "\n\t\tpublic static readonly LayerMask mask = new LayerMask(){ value =  1 << Int};"
                            + "\n\t}");
                output = $"{output}\n";
            }

            output = $"{output}}}";
            return output;
        }

        private static string ToVarName(string input)
        {
            return $"{input}".Replace(" ", "").Replace("_", "").Replace("/", "_");
        }
    }
}
#endif
using UnityEngine;
using UnityEditor;
using System.IO;

public static class CreateTextFile
{
    // Todo: Enable this conditionally only
    //[MenuItem("Assets/Create/Text File", priority = 202)]
    private static void CreateNewTextFile()
    {
        string folderGUID = Selection.assetGUIDs[0];
        string projectFolderPath = AssetDatabase.GUIDToAssetPath(folderGUID);
        string folderDirectory = Path.GetFullPath(projectFolderPath);

        using (StreamWriter sw = File.CreateText(folderDirectory + "/NewTextFile.txt"))
        {
            sw.WriteLine("This is a new text file!");
        }
        AssetDatabase.Refresh();
    }
}
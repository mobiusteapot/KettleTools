using System;
using UnityEditor;
using UnityEngine;

namespace ETC.KettleTools.Documentation {
    [CustomEditor(typeof(DefaultAsset))]
    public class FolderReadmeEditor : UnityEditor.Editor {
        // Draw default inspector
        private Readme readme;
        private AssetImporter importer;
        private string assetPath;
        bool hasReadme = false;
        private void OnEnable() {
            assetPath = AssetDatabase.GetAssetPath(target);
            importer = AssetImporter.GetAtPath(assetPath);
        }
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            EditorGUI.EndDisabledGroup();
            if (importer.userData != "" ) {
                try {
                    readme = AssetDatabase.LoadAssetAtPath<Readme>(AssetDatabase.GUIDToAssetPath(importer.userData));
                } catch (Exception e) {
                    Debug.Log("Error reading folder .meta user data. Regenerating data." + e);
                    importer.userData = "";
                }
            }


            EditorGUILayout.LabelField("Folder Readme");
            EditorGUI.BeginChangeCheck();
            if (readme != null && readme.name != "") {
                readme = (Readme)EditorGUILayout.ObjectField("Readme", readme, typeof(Readme), false);
                hasReadme = true;
            } else {
                readme = (Readme)EditorGUILayout.ObjectField("Readme", null, typeof(Readme), false);
            }
            if (EditorGUI.EndChangeCheck()) {
                string guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(readme));
                importer.userData = guid;
            }

            if (hasReadme) {
                readme.DrawReadmeSections();
            }
        }
    }
}
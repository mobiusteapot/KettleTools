using System;
using UnityEditor;
using UnityEngine;
using ETC.KettleTools.Documentation;
namespace ETC.KettleTools.Scenes {
    [CustomEditor(typeof(SceneAsset))]
    public class SceneAssetAdditionalDataEditor : Editor {
        // Draw default inspector
        private SceneAdditionalData sceneData;
        private AssetImporter importer;
        private string assetPath;
        private void OnEnable() {
            // Blank scriptable object for json to write to
            sceneData = CreateInstance<SceneAdditionalData>();
            assetPath = AssetDatabase.GetAssetPath(target);
            importer = AssetImporter.GetAtPath(assetPath);
        }
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            EditorGUI.EndDisabledGroup();

            if (importer.userData != "") {
                string guid = importer.userData;
                string path = AssetDatabase.GUIDToAssetPath(guid);
                sceneData = AssetDatabase.LoadAssetAtPath<SceneAdditionalData>(path);
            }


            using (var check = new EditorGUI.ChangeCheckScope()) {
                if (sceneData != null && sceneData.name != "") {
                    EditorGUI.BeginDisabledGroup(true);
                    sceneData = (SceneAdditionalData)EditorGUILayout.ObjectField("Additional Scene Data", sceneData, typeof(SceneAdditionalData), false);
                    EditorGUI.EndDisabledGroup();

                    SerializedObject sceneDataObject = new SerializedObject(sceneData);
                    SerializedProperty sceneDataIterator = sceneDataObject.GetIterator();
                    if (sceneDataIterator.NextVisible(true)) {
                        do {
                            if (sceneDataIterator.name != "m_Script") {
                                EditorGUILayout.PropertyField(sceneDataIterator);
                            }
                        } while (sceneDataIterator.NextVisible(false));
                    }

                    // Todo: Move this to scene readme?
                    SerializedProperty readmeProp = sceneDataObject.FindProperty("readme");
                    SerializedProperty showSceneReadmeProp = sceneDataObject.FindProperty("showSceneReadmeSetting");
                    EditorGUILayout.PropertyField(readmeProp);
                    EditorGUILayout.PropertyField(showSceneReadmeProp,new GUIContent("Show Readme: "));
                    if (readmeProp.objectReferenceValue != null) {
                        Readme readme = sceneData.readme;
                        SceneReadmeVisibility readmeSetting = sceneData.showSceneReadmeSetting;
                        if(readmeSetting.HasFlag(SceneReadmeVisibility.onAssetSelect)) {
                            readme.DrawReadmeSections();
                        }
                    }
                    sceneDataObject.ApplyModifiedProperties();
                } else if (GUILayout.Button("Create Additional Scene Data")) {
                    sceneData = CreateInstance<SceneAdditionalData>();
                    AssetDatabase.CreateAsset(sceneData, assetPath.Replace(".unity", "SceneData.asset"));
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
                if (check.changed) {
                    importer.userData = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(sceneData));
                }
            }
        }

    }
}
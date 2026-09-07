using System.IO;
using UnityEditor;
using UnityEngine;
using Mobtp.KT.Core.Docs;

namespace Mobtp.KettleTools.Scenes {
    [CustomEditor(typeof(SceneAsset))]
    public class SceneAssetAdditionalDataEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            bool wasEnabled = GUI.enabled;
            GUI.enabled = true;

            try {
                string assetPath = AssetDatabase.GetAssetPath(target);
                AssetImporter importer = AssetImporter.GetAtPath(assetPath);
                if (importer == null) return;

                string dataPath = AssetDatabase.GUIDToAssetPath(importer.userData);
                SceneAdditionalData sceneData = string.IsNullOrEmpty(dataPath)
                    ? null
                    : AssetDatabase.LoadAssetAtPath<SceneAdditionalData>(dataPath);

                if (sceneData == null) {
                    if (GUILayout.Button("Create Additional Scene Data")) {
                        sceneData = CreateInstance<SceneAdditionalData>();
                        dataPath = AssetDatabase.GenerateUniqueAssetPath(Path.ChangeExtension(assetPath, null) + "SceneData.asset");
                        AssetDatabase.CreateAsset(sceneData, dataPath);
                        AssetDatabase.SaveAssets();
                        importer.userData = AssetDatabase.AssetPathToGUID(dataPath);
                        importer.SaveAndReimport();
                    }
                    return;
                }

                using (new EditorGUI.DisabledScope(true)) {
                    EditorGUILayout.ObjectField("Additional Scene Data", sceneData, typeof(SceneAdditionalData), false);
                }

                using (var dataObject = new SerializedObject(sceneData)) {
                    dataObject.Update();
                    SerializedProperty iterator = dataObject.GetIterator();
                    if (iterator.NextVisible(true)) {
                        do {
                            if (iterator.name != "m_Script" && iterator.name != "sceneDependencies") {
                                GUIContent label = iterator.name == "autoOpenAdditionalScenesInEditor"
                                    ? new GUIContent("Auto-Open Additional Scenes In Editor") : null;
                                EditorGUILayout.PropertyField(iterator, label, true);
                            }
                        } while (iterator.NextVisible(false));
                    }
                    dataObject.ApplyModifiedProperties();
                }

                if (sceneData.readme != null && sceneData.showSceneReadmeSetting.HasFlag(SceneReadmeVisibility.onAssetSelect)) {
                    sceneData.readme.DrawReadmeSections();
                }
            } finally {
                GUI.enabled = wasEnabled;
            }
        }
    }
}

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using Mobtp.KT.Core.Docs;

namespace Mobtp.KettleTools.Scenes {
    [InitializeOnLoad]
    public class SceneAdditionalDataLoader : Editor
    {
        
        static SceneAdditionalDataLoader()
        {
            EditorSceneManager.activeSceneChangedInEditMode += SelectAfterDelay;
        }

        static void SelectAfterDelay(Scene previousScene, Scene newScene) {
            EditorApplication.delayCall += () => SelectNewSceneReadme(previousScene, newScene);
        }

        static void SelectNewSceneReadme(Scene previousScene, Scene newScene) {
            // Find scene asset with same name as scene
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(newScene.path);
            if (sceneAsset == null) return;
                // Get asset importer for scene asset
                var importer = AssetImporter.GetAtPath(newScene.path);
                if (importer.userData == "") return;
                string guid = importer.userData;
                string path = AssetDatabase.GUIDToAssetPath(guid);
                SceneAdditionalData sceneData = AssetDatabase.LoadAssetAtPath<SceneAdditionalData>(path);
            if (sceneData == null) return;
            var readme = sceneData.readme;
            if (readme != null) {
                // If readme is set to show on load, show it
                if (sceneData.showSceneReadmeSetting.HasFlag(SceneReadmeVisibility.onSceneOpen)) {
                    Readme currentSceneReadme = sceneData.readme;
                    Selection.objects = new UnityEngine.Object[] { currentSceneReadme };
                }
            }
        }
    }
}
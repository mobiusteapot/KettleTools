using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mobtp.KettleTools.Scenes {
    [InitializeOnLoad]
    internal static class SceneDependencyAutoOpener {
        private const string TransitionKey = "Mobtp.KettleTools.Scenes.PlayModeTransition";
        private static Scene pendingScene;
        private static bool openingDependencies;
        private static bool playModeTransition;

        static SceneDependencyAutoOpener() {
            playModeTransition = SessionState.GetBool(TransitionKey, false) || EditorApplication.isPlayingOrWillChangePlaymode;
            EditorSceneManager.sceneOpened += OnSceneOpened;
            EditorSceneManager.sceneClosed += OnSceneClosed;
            EditorSceneManager.newSceneCreated += OnNewSceneCreated;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            AssemblyReloadEvents.beforeAssemblyReload += CancelPending;
            if (!EditorApplication.isPlayingOrWillChangePlaymode) EditorApplication.delayCall += ResumeEditMode;
        }

        private static bool CanOpen => !playModeTransition && !openingDependencies
            && !EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode
            && !BuildPipeline.isBuildingPlayer;

        private static void OnSceneOpened(Scene scene, OpenSceneMode mode) {
            if (mode != OpenSceneMode.Single || openingDependencies) return;
            CancelPending();
            if (!CanOpen || !scene.IsValid() || !scene.isLoaded || string.IsNullOrEmpty(scene.path)) return;
            pendingScene = scene;
            EditorApplication.delayCall += OpenPendingDependencies;
        }

        private static void OnSceneClosed(Scene scene) {
            if (scene == pendingScene) CancelPending();
        }

        private static void OnNewSceneCreated(Scene scene, NewSceneSetup setup, NewSceneMode mode) {
            if (mode == NewSceneMode.Single) CancelPending();
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state) {
            CancelPending();
            EditorApplication.delayCall -= ResumeEditMode;
            playModeTransition = true;
            SessionState.SetBool(TransitionKey, true);
            if (state == PlayModeStateChange.EnteredEditMode) EditorApplication.delayCall += ResumeEditMode;
        }

        private static void ResumeEditMode() {
            if (EditorApplication.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode) return;
            playModeTransition = false;
            SessionState.SetBool(TransitionKey, false);
        }

        private static void CancelPending() {
            EditorApplication.delayCall -= OpenPendingDependencies;
            pendingScene = default;
        }

        private static void OpenPendingDependencies() {
            Scene mainScene = pendingScene;
            CancelPending();
            if (!CanOpen || !mainScene.IsValid() || !mainScene.isLoaded) return;

            AssetImporter importer = AssetImporter.GetAtPath(mainScene.path);
            if (importer == null || string.IsNullOrEmpty(importer.userData)) return;
            string dataPath = AssetDatabase.GUIDToAssetPath(importer.userData);
            if (string.IsNullOrEmpty(dataPath)) return;
            SceneAdditionalData data = AssetDatabase.LoadAssetAtPath<SceneAdditionalData>(dataPath);
            if (data == null || !data.AutoOpenAdditionalScenesInEditor) return;

            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { mainScene.path };
            openingDependencies = true;
            try {
                foreach (SceneDependencyReference dependency in data.SceneDependencies) {
                    if (EditorApplication.isPlayingOrWillChangePlaymode || !mainScene.IsValid() || !mainScene.isLoaded) break;
                    if (dependency == null || string.IsNullOrEmpty(dependency.SceneGuid)) continue;
                    string path = AssetDatabase.GUIDToAssetPath(dependency.SceneGuid);
                    if (string.IsNullOrEmpty(path) || AssetDatabase.LoadAssetAtPath<SceneAsset>(path) == null) {
                        Debug.LogWarning("Scene dependency is missing: " + dependency.SceneGuid, data);
                        continue;
                    }
                    if (!seen.Add(path)) continue;
                    Scene existing = SceneManager.GetSceneByPath(path);
                    if (existing.IsValid() && existing.isLoaded) continue;

                    try {
                        EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
                    } catch (Exception exception) {
                        Debug.LogWarning("Could not open scene dependency '" + path + "': " + exception.Message, data);
                    }
                }
            } finally {
                openingDependencies = false;
                if (!EditorApplication.isPlayingOrWillChangePlaymode && mainScene.IsValid() && mainScene.isLoaded
                    && SceneManager.GetActiveScene() != mainScene) {
                    SceneManager.SetActiveScene(mainScene);
                }
            }
        }
    }
}

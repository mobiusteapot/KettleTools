using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using Mobtp.KettleTools;

namespace Mobtp.KettleTools.Audio {
    [CustomEditor(typeof(AudioContainer))]
    public abstract class AudioContainerEditor : Editor
    {
        SerializedProperty audioContainers;
        SerializedProperty isClipRandomized;
        SerializedProperty isVolumeRandomized;
        SerializedProperty volumeMin;
        SerializedProperty volumeMax;
        SerializedProperty volume;
        SerializedProperty isPitchRandomized;
        SerializedProperty pitchMin;
        SerializedProperty pitchMax;
        SerializedProperty pitch;

        // audio container struct properties
        SerializedProperty audioClip;
        SerializedProperty subtitles;
        int audioContainerArraySize;

        private void OnEnable() {
            audioContainers = serializedObject.FindProperty("_audioBundles");
            isClipRandomized = serializedObject.FindProperty("_isClipRandomized");
            isVolumeRandomized = serializedObject.FindProperty("_isVolumeRandomized");
            volumeMin = serializedObject.FindProperty("_volumeMin");
            volumeMax = serializedObject.FindProperty("_volumeMax");
            volume = serializedObject.FindProperty("_volume");
            isPitchRandomized = serializedObject.FindProperty("_isPitchRandomized");
            pitchMin = serializedObject.FindProperty("_pitchMin");
            pitchMax = serializedObject.FindProperty("_pitchMax");
            pitch = serializedObject.FindProperty("_pitch");
        }
        private void OnDisable() {
            serializedObject.ApplyModifiedProperties();
        }
        public override void OnInspectorGUI() {

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Audio Containers");
            audioContainers.arraySize = EditorGUILayout.IntField(audioContainers.arraySize, GUILayout.Width(20));
            if(GUILayout.Button("+", GUILayout.Width(20))) {
                audioContainers.InsertArrayElementAtIndex(audioContainers.arraySize);
            }
            if(GUILayout.Button("-", GUILayout.Width(20))) {
                audioContainers.DeleteArrayElementAtIndex(audioContainers.arraySize - 1);
            }
            EditorGUILayout.EndHorizontal();
            // Serialize reference bs
            for(int i = 0; i < audioContainers.arraySize; i++) {

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(audioContainers.GetArrayElementAtIndex(i),true);
                if(GUILayout.Button("Play", GUILayout.Width(60))) {
                    // Update object immediately
                    serializedObject.ApplyModifiedProperties();
                    StopAllClips();
                    // Debug.Log("Getting clip by serialization:" + audioContainers.GetArrayElementAtIndex(i).managedReferenceValue);
                    AudioContainer audioContainer = (AudioContainer)target;
                    PlayClip(audioContainer.GetAudioClipByIndex(i));
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.PropertyField(isClipRandomized);
            EditorGUILayout.PropertyField(isVolumeRandomized);
            IMGUIStaticExtensions.ConditionalPropertyField(isVolumeRandomized.boolValue, new (volumeMin, volumeMax), new (volume));
            EditorGUILayout.PropertyField(isVolumeRandomized);
            EditorGUILayout.PropertyField(isPitchRandomized);
            IMGUIStaticExtensions.ConditionalPropertyField(isPitchRandomized.boolValue, new (pitchMin, pitchMax), new (pitch));
        }
        // Use reflection to play or stop audio clips for in-editor previewing
        public static void PlayClip(AudioClip clip) {
            Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
            Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            MethodInfo method = audioUtilClass.GetMethod(
                "PlayPreviewClip",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new System.Type[] { typeof(AudioClip), typeof(Int32), typeof(Boolean) },
                null
            );
            method.Invoke(
                null, new object[] { 
                clip, 0, false
            });
        }
        public static void StopAllClips() {
            Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
            Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            MethodInfo method = audioUtilClass.GetMethod(
                "StopAllPreviewClips",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new System.Type[] { },
                null
            );
            method.Invoke(null, new object[] { });
        }
    }
}

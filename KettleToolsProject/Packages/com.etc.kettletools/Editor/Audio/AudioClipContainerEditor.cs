using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using ETC.KettleTools.EditorExtensions;

namespace ETC.KettleTools.Audio {
    [CanEditMultipleObjects]
    [CustomEditor(typeof(AudioClipContainer))]
    public class AudioClipContainerEditor : Editor
    {
        // Only character audio containers should have subtitles
        public bool DrawSubtitles = false;
        // Only FMOD audio containers should control pitch
        public bool DrawPitch = false;
        SerializedProperty containerType;
        SerializedProperty audioBundle;
        SerializedProperty acProps;
        SerializedProperty isClipRandomized;
        SerializedProperty isVolumeRandomized;
        SerializedProperty volumeMin;
        SerializedProperty volumeMax;
        SerializedProperty volume;
        SerializedProperty isPitchRandomized;
        SerializedProperty pitchMin;
        SerializedProperty pitchMax;
        SerializedProperty pitch;

        SerializedProperty eventReference;

        // audio container struct properties
        SerializedProperty audioClip;
        SerializedProperty subtitles;
        int audioContainerArraySize;

        private void OnEnable() {
            containerType = serializedObject.FindProperty("containerType");
            audioBundle = serializedObject.FindProperty("_audioBundles");
            acProps = serializedObject.FindProperty("acProps");
            isClipRandomized = acProps.FindPropertyRelative("_isClipRandomized");
            isVolumeRandomized = acProps.FindPropertyRelative("_isVolumeRandomized");
            volumeMin = acProps.FindPropertyRelative("_volumeMin");
            volumeMax = acProps.FindPropertyRelative("_volumeMax");
            volume = acProps.FindPropertyRelative("_volume");
            isPitchRandomized = acProps.FindPropertyRelative("_isPitchRandomized");
            pitchMin = acProps.FindPropertyRelative("_pitchMin");
            pitchMax = acProps.FindPropertyRelative("_pitchMax");
            pitch = acProps.FindPropertyRelative("_pitch");
        }
        private void OnDisable() {
            serializedObject.ApplyModifiedProperties();
        }
        public override void OnInspectorGUI() {
            serializedObject.Update();
            EditorGUILayout.PropertyField(containerType);
            using (new EditorGUILayout.HorizontalScope()) {
                EditorGUILayout.LabelField("Audio Bundles");
                audioBundle.arraySize = EditorGUILayout.IntField(audioBundle.arraySize, GUILayout.Width(20));
                if(GUILayout.Button("+", GUILayout.Width(20))) {
                    audioBundle.InsertArrayElementAtIndex(audioBundle.arraySize);
                }
                if(GUILayout.Button("-", GUILayout.Width(20))) {
                    audioBundle.DeleteArrayElementAtIndex(audioBundle.arraySize - 1);
                }
            }

            // Serialize reference bs
            for(int i = 0; i < audioBundle.arraySize; i++) {
                using (new EditorGUI.IndentLevelScope())
                using (new EditorGUILayout.HorizontalScope()) {
                    // If subtitles enabled, draw all children, if not, draw only audio clip
                    using (new EditorGUI.IndentLevelScope()) {
                        if(DrawSubtitles) {
                            // Vertical scope for subtitles
                            using (new EditorGUILayout.VerticalScope()) {
                                EditorGUILayout.PropertyField(audioBundle.GetArrayElementAtIndex(i).FindPropertyRelative("_audioClip"));
                                EditorGUILayout.PropertyField(audioBundle.GetArrayElementAtIndex(i).FindPropertyRelative("_subtitles"));
                            }
                        } else {
                            EditorGUILayout.PropertyField(audioBundle.GetArrayElementAtIndex(i).FindPropertyRelative("_audioClip"));
                        }
                        if(GUILayout.Button("Play", GUILayout.Width(60))) {
                            serializedObject.ApplyModifiedProperties();
                            StopAllClips();
                            AudioClipContainer audioContainer = (AudioClipContainer)target;
                            PlayClip(audioContainer.GetAudioClipByIndex(i));
                        }
                    }
                }
            }

            EditorGUILayout.PropertyField(isClipRandomized);
            EditorGUILayout.PropertyField(isVolumeRandomized);
            KTEditorGUI.ConditionalPropertyField(isVolumeRandomized.boolValue, new (volumeMin, volumeMax), new (volume));
            EditorGUILayout.PropertyField(isPitchRandomized);
            KTEditorGUI.ConditionalPropertyField(isPitchRandomized.boolValue, new (pitchMin, pitchMax), new (pitch));
            serializedObject.ApplyModifiedProperties();
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

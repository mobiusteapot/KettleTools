using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using ETC.KettleTools.EditorExtensions;

namespace ETC.KettleTools.Audio.FMODAddon {
    [CustomEditor(typeof(KTEventEmitter))]
    [CanEditMultipleObjects]
    public class KTEventEmitterEditor : Editor {
        SerializedProperty eventAudioContainer;
        SerializedProperty UseGameEvents;
        SerializedProperty playbackType;
        SerializedProperty EventIndex;
        SerializedProperty PlayEvent;
        SerializedProperty StopEvent;
        SerializedProperty TriggerOnce;
        SerializedProperty hasTriggered;

        public void OnEnable() {
            eventAudioContainer = serializedObject.FindProperty("eventAudioContainer");
            UseGameEvents = serializedObject.FindProperty("UseGameEvents");
            playbackType = serializedObject.FindProperty("playbackType");
            EventIndex = serializedObject.FindProperty("EventIndex");
            PlayEvent = serializedObject.FindProperty("PlayEvent");
            StopEvent = serializedObject.FindProperty("StopEvent");
            TriggerOnce = serializedObject.FindProperty("TriggerOnce");
            hasTriggered = serializedObject.FindProperty("hasTriggered");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(eventAudioContainer);
            EditorGUILayout.PropertyField(UseGameEvents);
            using (new EditorGUI.IndentLevelScope()){
                if(UseGameEvents.boolValue) {
                    EditorGUILayout.PropertyField(PlayEvent);
                    EditorGUILayout.PropertyField(StopEvent);
                    EditorGUILayout.PropertyField(playbackType);
                    switch (playbackType.enumValueIndex) {
                        case 0:
                            break;
                        case 1:
                            // indent scope
                            using (new EditorGUI.IndentLevelScope()) {
                                EditorGUILayout.PropertyField(EventIndex);
                                EditorGUILayout.Space();
                            }
                            break;
                        default:
                            break;
                    }
                    EditorGUILayout.PropertyField(TriggerOnce);
                    EditorGUILayout.PropertyField(hasTriggered);
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
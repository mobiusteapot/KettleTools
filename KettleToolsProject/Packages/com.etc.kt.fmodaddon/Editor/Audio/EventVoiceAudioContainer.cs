using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using FMODUnity;
using System.Collections.Generic;
using System.IO;

namespace ETC.KettleTools.Audio.FMODAddon {
    [CustomEditor(typeof(EventVoiceAudioContainer))]
    public class EventVoiceAudioContainerEditor : Editor
    {
        public static FMOD.Studio.EventInstance PreviewEventInstance { get; private set; }

        private void OnEnable() {
            Debug.Log("OnEnable");
            EditorUtils.LoadPreviewBanks();
        }
        private void OnDisable() {
            Debug.Log("OnDisable");
            if (PreviewEventInstance.isValid())
            {
                EditorUtils.PreviewStop(PreviewEventInstance);
            }
        }
        public override void OnInspectorGUI()
        {
            Debug.Log("Drawing default inspector");
            DrawDefaultInspector();
            // Draw button that says "Play"
            if (GUILayout.Button("Play")) {
                if (PreviewEventInstance.isValid())
                {
                    EditorUtils.PreviewStop(PreviewEventInstance);
                }
                EditorEventRef eventRef = EventManager.EventFromPath("event:/Character/Door Close");
                if(eventRef == null){
                    Debug.LogError("Event not found");
                    return;
                }
                Dictionary<string, float> paramValues = new Dictionary<string, float>();
                foreach (EditorParamRef param in eventRef.Parameters)
                {
                    paramValues.Add(param.Name, param.Default);
                }
                Debug.Log("Git gud: " + eventRef.Path + " " + eventRef.Guid);
                PreviewEventInstance = EditorUtils.PreviewEvent(eventRef, paramValues);
            }
        }
    }
}
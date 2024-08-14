using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Mobtp.KT.Core.Documentation {
    [CustomEditor(typeof(ReadmeComponent))]
    public class ReadmeComponentEditor : UnityEditor.Editor {
        private SerializedProperty readme;

        const float k_Space = 16f;

        public void OnEnable() {
            readme = serializedObject.FindProperty(nameof(Readme));
        }

        public override void OnInspectorGUI() {
            EditorGUILayout.PropertyField(readme);
            serializedObject.ApplyModifiedProperties();

            Readme targetReadme = (Readme)readme.objectReferenceValue;
            if (targetReadme == null) return;
            if (targetReadme.TextType == ReadmeTextType.External) {
                Debug.Log("External Text");
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("External Text", EditorStyles.boldLabel);
                EditorGUILayout.ObjectField(targetReadme.ExternalText, typeof(TextAsset), false);
                if (GUILayout.Button("Refresh Readme")) {
                    targetReadme.RefreshExternalText();
                }
            } else{
                Debug.Log("Internal Text");
            }
            targetReadme.DrawReadmeSections();
        }
    }
}
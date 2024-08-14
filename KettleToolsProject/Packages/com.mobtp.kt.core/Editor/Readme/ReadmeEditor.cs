using UnityEngine;
using UnityEditor;

namespace Mobtp.KT.Core.Documentation {
    [CustomEditor(typeof(Readme), true)]
    public class ReadmeEditor : Editor {
        private SerializedProperty IsLocked;
        private SerializedProperty TextType;
        private SerializedProperty ExternalText;
        private bool hasTextChanged = false;

        private void OnEnable() {
            IsLocked = serializedObject.FindProperty(nameof(Readme.IsLocked));
            TextType = serializedObject.FindProperty(nameof(Readme.TextType));
            ExternalText = serializedObject.FindProperty(nameof(Readme.ExternalText));
        }
        public override void OnInspectorGUI() {
            // If is not locked, display the lock button and text type enum popup
            if (!IsLocked.boolValue) {
                EditorGUILayout.PropertyField(TextType);
                if (TextType.enumValueIndex == (int)ReadmeTextType.External) {
                    EditorGUILayout.Space();
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(ExternalText);
                    if (EditorGUI.EndChangeCheck()) {
                        hasTextChanged = true;
                    }
                    if (GUILayout.Button("Refresh Readme")) {
                        hasTextChanged = true;
                    }
                }

                EditorGUILayout.Space();
                if (GUILayout.Button("Lock Readme")) {
                    IsLocked.boolValue = true;
                    Debug.Log("Locked " + target.name + ", Set editor to \"Debug Mode\" to toggle lock status.");
                }
            }
            serializedObject.ApplyModifiedProperties();

            Readme targetReadme = (Readme)target;
            if (targetReadme == null) return;
            if(hasTextChanged) {
                targetReadme.RefreshExternalText();
                hasTextChanged = false;
            }
            targetReadme.DrawReadmeSections();
        }
    }
}
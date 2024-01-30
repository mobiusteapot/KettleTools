using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ETC.KettleTools.Documentation {
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
            targetReadme.DrawReadmeSections();
        }
    }
}
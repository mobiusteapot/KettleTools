using UnityEngine;
using UnityEditor;

namespace Mobtp.KettleTools.Scenes {
    // Todo: Something with readme popup is causing scenes to get flagged as edited when a scene readme is opened!
    // Todo: Clean this up as part of UI toolkit conversion
    [CustomEditor(typeof(SceneAdditionalData), true)]
    public class SceneAdditionalDataEditor : Editor {
        SerializedProperty readmeProp;
        SerializedProperty showSceneReadmeProp;
        void OnEnable(){
            readmeProp = serializedObject.FindProperty("readme");
            showSceneReadmeProp = serializedObject.FindProperty("showSceneReadmeSetting");
        }

        public override void OnInspectorGUI(){
            serializedObject.Update();
            EditorGUILayout.PropertyField(readmeProp);
            EditorGUILayout.PropertyField(showSceneReadmeProp, new GUIContent("Show Readme: "));
            SerializedProperty iterator = serializedObject.GetIterator();
            if (iterator.NextVisible(true)) {
                do {
                    if (iterator.name != "m_Script") {
                        GUIContent label = iterator.name == "autoOpenAdditionalScenesInEditor" ? new GUIContent("Auto-Open Additional Scenes In Editor") : null;
                        EditorGUILayout.PropertyField(iterator, label, true);
                    }
                } while (iterator.NextVisible(false));
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
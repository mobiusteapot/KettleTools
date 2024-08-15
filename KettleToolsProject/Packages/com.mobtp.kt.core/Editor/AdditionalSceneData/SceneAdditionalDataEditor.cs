using UnityEngine;
using UnityEditor;
namespace Mobtp.KettleTools.Scenes {
    [CustomEditor(typeof(SceneAdditionalData), true)]
    public class SceneAdditionalDataEditor : Editor {
        // Todo: Something with readme popup is causing scenes to get flagged as edited when a scene readme is opened!
        SerializedProperty readmeProp;
        SerializedProperty showSceneReadmeProp;
        void OnEnable(){
            readmeProp = serializedObject.FindProperty("readme");
            showSceneReadmeProp = serializedObject.FindProperty("showSceneReadmeSetting");
        }

        public override void OnInspectorGUI(){
            EditorGUILayout.PropertyField(readmeProp);
            EditorGUILayout.PropertyField(showSceneReadmeProp, new GUIContent("Show Readme: "));
            SerializedProperty iterator = serializedObject.GetIterator();
            if (iterator.NextVisible(true)) {
                do {
                    if (iterator.name != "m_Script") {
                        EditorGUILayout.PropertyField(iterator);
                    }
                } while (iterator.NextVisible(false));
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
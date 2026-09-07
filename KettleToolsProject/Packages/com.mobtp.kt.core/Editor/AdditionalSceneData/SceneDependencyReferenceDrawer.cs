using UnityEditor;
using UnityEngine;

namespace Mobtp.KettleTools.Scenes {
    
    // Todo: do we just want to use someone else's drawer for this?
    // Why does Unity not have a better way to reference scenes yet -_-
    [CustomPropertyDrawer(typeof(SceneDependencyReference))]
    public class SceneDependencyReferenceDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, GUIContent.none, property);
            SerializedProperty guidProperty = property.FindPropertyRelative("sceneGuid");
            string path = AssetDatabase.GUIDToAssetPath(guidProperty.stringValue);
            SceneAsset scene = string.IsNullOrEmpty(path) ? null : AssetDatabase.LoadAssetAtPath<SceneAsset>(path);

            EditorGUI.BeginChangeCheck();
            SceneAsset selectedScene = (SceneAsset)EditorGUI.ObjectField(position, GUIContent.none, scene, typeof(SceneAsset), false);
            if (EditorGUI.EndChangeCheck()) {
                path = selectedScene == null ? string.Empty : AssetDatabase.GetAssetPath(selectedScene);
                guidProperty.stringValue = string.IsNullOrEmpty(path) ? string.Empty : AssetDatabase.AssetPathToGUID(path);
            }

            EditorGUI.EndProperty();
        }
    }
}

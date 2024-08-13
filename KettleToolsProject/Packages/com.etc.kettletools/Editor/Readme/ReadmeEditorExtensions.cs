using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

namespace ETC.KettleTools.Documentation {
    public static class ReadmeEditorExtensions {
        private const float maxImageWidth = 512f;
        public static void DrawReadmeSections(this Readme readme) {
            bool isBlank = true;
            if (readme == null || readme.IsInitalized == false) return;
            if (readme.icon != null) {
                readme.DrawReadmeIconHeader();
                isBlank = false;
            }
            if (readme.sections.Count > 0) isBlank = false;
            if(isBlank){
                EditorGUILayout.HelpBox("No sections or icons found. You can add sections by setting the inspector to \"Debug Mode\"", MessageType.Info);
                if(GUILayout.Button("Add Section")){
                    readme.sections.Add(new Readme.Section());
                    ToggleInspectorDebug();
                }
            }
            foreach (var section in readme.sections) {
                if (!string.IsNullOrEmpty(section.Heading)) {
                    GUILayout.Label(section.Heading, readme.HeadingStyle);
                }
                if (!string.IsNullOrEmpty(section.Text)) {
                    GUILayout.Label(section.Text, readme.BodyStyle);
                }
                if (!string.IsNullOrEmpty(section.LinkText)) {
                    if (readme.LinkLabel(new GUIContent(section.LinkText))) {
                        Application.OpenURL(section.Url);
                    }
                }
                if(section.Image != null && section.Image.texture != null){
                    // Draw image size dynamically, normalized to the inspector window
                    // Image should be centered and have a maximum width
                    var width = Mathf.Min(maxImageWidth, EditorGUIUtility.currentViewWidth - 40f);
                    var height = width / section.Image.texture.width * section.Image.texture.height;
                    GUILayout.Label(section.Image.texture, GUILayout.Width(width), GUILayout.Height(height));
                }
                GUILayout.Space(20f);
            }
        }

        public static void DrawReadmeIconHeader(this Readme readme) {
            const float k_Space = 16f;
            var iconWidth = Mathf.Min(EditorGUIUtility.currentViewWidth / 3f - 20f, 128f);

            GUILayout.BeginHorizontal("In BigTitle");
            {
                if (readme.icon != null) {
                    GUILayout.Space(k_Space);
                    GUILayout.Label(readme.icon, GUILayout.Width(iconWidth), GUILayout.Height(iconWidth));
                }
                GUILayout.Space(k_Space);
                GUILayout.BeginVertical();
                {

                    GUILayout.FlexibleSpace();
                    GUILayout.Label(readme.title, readme.TitleStyle);
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }
        static void ToggleInspectorDebug()
        {
            EditorWindow targetInspector = EditorWindow.mouseOverWindow; // "EditorWindow.focusedWindow" can be used instead
 
            if (targetInspector != null  && targetInspector.GetType().Name == "InspectorWindow")
            {
                Type type = Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetType("UnityEditor.InspectorWindow");    //Get the type of the inspector window to find out the variable/method from
                FieldInfo field = type.GetField("m_InspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);    //get the field we want to read, for the type (not our instance)
                
                InspectorMode mode = (InspectorMode)field.GetValue(targetInspector);                                    //read the value for our target inspector
                mode = (mode == InspectorMode.Normal ? InspectorMode.Debug : InspectorMode.Normal);                    //toggle the value
                
                MethodInfo method = type.GetMethod("SetMode", BindingFlags.NonPublic | BindingFlags.Instance);          //Find the method to change the mode for the type
                method.Invoke(targetInspector, new object[] {mode});                                                    //Call the function on our targetInspector, with the new mode as an object[]
            
                targetInspector.Repaint();       //refresh inspector
            }
        }
    }
}
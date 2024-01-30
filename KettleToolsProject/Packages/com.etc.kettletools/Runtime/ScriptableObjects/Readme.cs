using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace ETC.KettleTools.Documentation {
    [CreateAssetMenu(fileName = "Readme", menuName = "Readme", order = 202)]
    public class Readme : ScriptableObject {
        // Pragma is neccesary to ensure no readme object data gets included on build in any capacity
        #if UNITY_EDITOR
        public Texture2D icon;
        public string title;
        public List<Section> sections = new List<Section>();

        [Serializable]
        public class Section {
            public string heading;
            // Todo: Make a custom inspector for "edit" mode instead of the current debug mode paradigm
            [TextArea]
            public string text;
            public string linkText, url;
        }
        public void Reset() {
            Init();
            title = this.GetType().Name;
        }

        [SerializeField, HideInInspector] 
        private bool isInitalized;
        public bool IsInitalized {
            get { return isInitalized; }
            private set { isInitalized = value; }
        }
        [SerializeField, HideInInspector] 
        private GUIStyle linkStyle;
        public GUIStyle LinkStyle {
            get { return linkStyle; }
            private set { linkStyle = value; }
        }
        [SerializeField, HideInInspector] 
        private GUIStyle titleStyle;
        public GUIStyle TitleStyle {
            get { return titleStyle; }
            private set { titleStyle = value; }
        }
        [SerializeField, HideInInspector] 
        private GUIStyle headingStyle;
        public GUIStyle HeadingStyle {
            get { return headingStyle; }
            private set { headingStyle = value; }
        }
        [SerializeField, HideInInspector] 
        private GUIStyle bodyStyle;
        public GUIStyle BodyStyle {
            get { return bodyStyle; }
            private set { bodyStyle = value; }
        }
        [SerializeField, HideInInspector] 
        private GUIStyle buttonStyle;
        public GUIStyle ButtonStyle {
            get { return buttonStyle; }
            private set { buttonStyle = value; }
        }

        void Init() {
            BodyStyle = new GUIStyle(EditorStyles.label);
            BodyStyle.wordWrap = true;
            BodyStyle.fontSize = 14;
            BodyStyle.richText = true;

            TitleStyle = new GUIStyle(BodyStyle);
            TitleStyle.fontSize = 26;

            HeadingStyle = new GUIStyle(BodyStyle);
            HeadingStyle.fontStyle = FontStyle.Bold;
            HeadingStyle.fontSize = 18;

            LinkStyle = new GUIStyle(BodyStyle);
            LinkStyle.wordWrap = false;

            // Match selection color which works nicely for both light and dark skins
            LinkStyle.normal.textColor = new Color(0x00 / 255f, 0x78 / 255f, 0xDA / 255f, 1f);
            LinkStyle.stretchWidth = false;

            ButtonStyle = new GUIStyle(EditorStyles.miniButton);
            ButtonStyle.fontStyle = FontStyle.Bold;
            IsInitalized = true;
            EditorUtility.SetDirty(this);
        }
        public bool LinkLabel(GUIContent label, params GUILayoutOption[] options) {
            var position = GUILayoutUtility.GetRect(label, LinkStyle, options);

            Handles.BeginGUI();
            Handles.color = LinkStyle.normal.textColor;
            Handles.DrawLine(new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
            Handles.color = Color.white;
            Handles.EndGUI();

            EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);

            return GUI.Button(position, label, LinkStyle);
        }
        #endif
    }
}
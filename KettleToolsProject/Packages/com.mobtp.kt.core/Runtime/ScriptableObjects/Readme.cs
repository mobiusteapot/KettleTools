using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Mobtp.KT.Core.Docs {
    [CreateAssetMenu(fileName = "Readme", menuName = "Readme", order = 202)]
    public class Readme : ScriptableObject {
        // Pragma is necessary to ensure no readme object data gets included on build in any capacity
        #if UNITY_EDITOR
        public ReadmeTextType TextType = ReadmeTextType.Internal;
        public TextAsset ExternalText;
        public bool IsLocked = false;
        public Texture2D Icon;
        public string Title;
        public List<Section> Sections = new List<Section>();

        [Serializable]
        public class Section {
            public string Heading;
            [TextArea]
            public string Text;
            public string LinkText, Url;
            public Sprite Image;
        }

        public void Reset() {
            Init();
            Title = this.GetType().Name;
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

        private void Init() {
            BodyStyle = new GUIStyle(EditorStyles.label) {
                wordWrap = true,
                fontSize = 14,
                richText = true
            };

            TitleStyle = new GUIStyle(BodyStyle) {
                fontSize = 26
            };

            HeadingStyle = new GUIStyle(BodyStyle) {
                fontStyle = FontStyle.Bold,
                fontSize = 18
            };

            LinkStyle = new GUIStyle(BodyStyle) {
                wordWrap = false,
                normal = { textColor = new Color(0x00 / 255f, 0x78 / 255f, 0xDA / 255f, 1f) },
                stretchWidth = false
            };

            ButtonStyle = new GUIStyle(EditorStyles.miniButton) {
                fontStyle = FontStyle.Bold
            };

            IsInitalized = true;
            EditorUtility.SetDirty(this);
        }

        public void RefreshExternalText() {
            if (TextType == ReadmeTextType.External) {
                if (ExternalText != null) {
                    Sections.Clear();
                    ParseExternalText(ExternalText.text);
                } 
                else {
                    Debug.LogError("No text file found.");
                }
            } 
            else {
                Debug.LogWarning("Text not set to external, cannot refresh.");
            }
        }

        private void ParseExternalText(string text) {
            string[] lines = text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            Section currentSection = null;

            foreach (string line in lines) {
                if (line.StartsWith("#")) {
                    if (currentSection != null) {
                        Sections.Add(currentSection);
                    }
                    currentSection = new Section {
                        Heading = line.Substring(1)
                    };
                } // Parse images as Markdown where if there is an ! then brackets[] assume the brackets are the alt text and the parenthesis() is the url
                else if (line.Contains("!") && line.Contains("[") && line.Contains("]")) {
                    int altStart = line.IndexOf("![");
                    int altEnd = line.IndexOf("]");
                    int urlStart = line.IndexOf("(");
                    int urlEnd = line.IndexOf(")");
                    if (altStart < urlStart && urlStart < urlEnd && urlEnd < line.Length) {
                        if (currentSection != null) {
                            currentSection.Text += line.Substring(0, altStart);
                            currentSection.Image = AssetDatabase.LoadAssetAtPath<Sprite>(line.Substring(urlStart + 1, urlEnd - urlStart - 1));
                        }
                    }
                    else if (currentSection != null) {
                        currentSection.Text += line + "\n";
                    }
                }
                 // Parse links as Markdown where if there is a statement in brackets[] then parenthesis at the end() assume the brackets are the link text and the parenthsis is the url
                else if(line.Contains("[") && line.Contains("]")) {
                    int linkStart = line.IndexOf("[");
                    int linkEnd = line.IndexOf("]");
                    int urlStart = line.IndexOf("(");
                    int urlEnd = line.IndexOf(")");

                    if (linkStart < urlStart && urlStart < urlEnd && urlEnd < line.Length) {
                        if (currentSection != null) {
                            currentSection.Text += line.Substring(0, linkStart);
                            currentSection.LinkText = line.Substring(linkStart + 1, linkEnd - linkStart - 1);
                            currentSection.Url = line.Substring(urlStart + 1, urlEnd - urlStart - 1);
                        }
                    }
                    else if (currentSection != null) {
                        currentSection.Text += line + "\n";
                    }
                }
                else if (currentSection != null) {
                    currentSection.Text += line + "\n";
                }
            }

            if (currentSection != null) {
                Sections.Add(currentSection);
            }
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
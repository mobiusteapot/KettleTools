
using UnityEngine;
#if PACKAGE_TEXTMESHPRO
using TMPro;
#endif
using System.Collections.Generic;
/// <summary>
/// Utility script to display a readme on a canvas.
/// Not included if the TextMeshPro package is not installed. 
/// </summary>
namespace Mobtp.KT.Core.Documentation {

    [RequireComponent(typeof(CanvasRenderer), typeof(RectTransform))]
    public class CanvasReadmeDisplay : MonoBehaviour {
        #if PACKAGE_TEXTMESHPRO
        public CanvasReadmeSettings Settings;
        [SerializeField] private Readme readme;

        [Space]
        [SerializeField] private List<TextMeshProUGUI> headerSections;
        [SerializeField] private List<TextMeshProUGUI> bodySections;
        // Todo: Automatically update the canvas when the readme is changed

        public void PopulateCanvas() {
            ClearTextComponents();

            // Create new text components
            float currentY = 0f; // Start Y position

            foreach (Readme.Section section in readme.Sections) {
                // Create or get header component
                if (section.Heading != null) {
                    TextMeshProUGUI headerText = CreateTextComponent("Header_" + section.Heading.GetHashCode());
                    headerText.text = section.Heading;
                    Settings.ApplySettings(ref headerText, true);
                    SetRectTransform(headerText, currentY, Settings.HeaderWidth, headerText.preferredHeight + Settings.HeaderSpacing);
                    currentY -= (headerText.preferredHeight + Settings.HeaderSpacing);
                    headerSections.Add(headerText);
                }

                if (section.Text != null) {
                    TextMeshProUGUI bodyText = CreateTextComponent("Body_" + section.Text.GetHashCode());
                    bodyText.text = section.Text;
                    Settings.ApplySettings(ref bodyText, false);
                    SetRectTransform(bodyText, currentY, Settings.BodyWidth, bodyText.preferredHeight + Settings.SectionSpacing);
                    currentY -= (bodyText.preferredHeight + Settings.SectionSpacing);
                    bodySections.Add(bodyText);
                }
            }
        }

    private void ClearTextComponents()
    {
        // Destroy all existing header and body text components
        foreach (TextMeshProUGUI header in headerSections) {
            if (header != null) {
                DestroyImmediate(header.gameObject);
            }
        }
        headerSections.Clear();

        foreach (TextMeshProUGUI body in bodySections) {
            if (body != null) {
                DestroyImmediate(body.gameObject);
            }
        }
        bodySections.Clear();
    }

    private TextMeshProUGUI CreateTextComponent(string name)
    {
        GameObject textObject = new GameObject(name);
        textObject.transform.SetParent(transform, false);
        TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
        if(Settings.NotEditable){
            textComponent.hideFlags = HideFlags.NotEditable;
        }
        // Set anchor to top middle
        textComponent.rectTransform.anchorMin = new Vector2(0.5f, 1f);
        textComponent.rectTransform.anchorMax = new Vector2(0.5f, 1f);
        textComponent.rectTransform.pivot = new Vector2(0.5f, 1f);
        return textComponent;
    }

    private void SetRectTransform(TextMeshProUGUI textComponent, float currentY, float width, float height)
    {
        RectTransform rectTransform = textComponent.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, currentY);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }
    #endif
    }
}
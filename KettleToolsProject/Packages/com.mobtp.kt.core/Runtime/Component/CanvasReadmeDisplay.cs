
using UnityEngine;
// Currently assuming UI is always included
using UnityEngine.UI;
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
        // All population should be done in editor, no runtime access of any kind should occur
        #if UNITY_EDITOR
        #if PACKAGE_TEXTMESHPRO
        public CanvasReadmeSettings Settings;
        [SerializeField] private Readme readme;

        [Space]
        [SerializeField] private List<TextMeshProUGUI> headerSections;
        [SerializeField] private List<TextMeshProUGUI> bodySections;
        [SerializeField] private List<Image> imageSections;
        // Todo: Automatically update the canvas when the readme is changed

        public void PopulateCanvas() {
            ClearCanvasComponents();

            // Force update the readme if external
            if (readme.TextType == ReadmeTextType.External) {
                readme.RefreshExternalText();
            }

            // Create new text components
            float currentY = 0f; // Start Y position
            foreach (Readme.Section section in readme.Sections) {
                // Create or get header component
                if (section.Heading != null) {
                    TextMeshProUGUI headerText = CreateTextComponent("Header_" + section.Heading.GetHashCode());
                    headerText.text = section.Heading;
                    Settings.ApplySettings(ref headerText, true);
                    SetRectTransform(headerText.rectTransform, currentY, Settings.HeaderWidth, headerText.preferredHeight + Settings.HeaderSpacing);
                    currentY -= (headerText.preferredHeight + Settings.HeaderSpacing);
                    headerSections.Add(headerText);
                }

                if (section.Text != null) {
                    TextMeshProUGUI bodyText = CreateTextComponent("Body_" + section.Text.GetHashCode());
                    bodyText.text = section.Text;
                    Settings.ApplySettings(ref bodyText, false);
                    SetRectTransform(bodyText.rectTransform, currentY, Settings.BodyWidth, bodyText.preferredHeight + Settings.SectionSpacing);
                    currentY -= (bodyText.preferredHeight + Settings.SectionSpacing);
                    bodySections.Add(bodyText);
                }
                if(section.Image != null){
                    Image image = CreateImageComponent("Image_" + section.Image.GetHashCode());
                    image.sprite = section.Image;
                    // If width OR height is overriden, set the size and respect aspect ratio
                    // If both, then set the size directly
                    float aspectRatio = image.sprite.rect.width / image.sprite.rect.height;

                    float newWidth;
                    float newHeight;

                    if (Settings.OverrideImageWidth && Settings.OverrideImageHeight) {
                        newWidth = Settings.ImageWidth;
                        newHeight = Settings.ImageHeight;
                    } else if (Settings.OverrideImageWidth) {
                        newWidth = Settings.ImageWidth;
                        newHeight = newWidth / aspectRatio;
                    } else if (Settings.OverrideImageHeight) {
                        newHeight = Settings.ImageHeight;
                        newWidth = newHeight * aspectRatio;
                    } else {
                        newWidth = image.sprite.rect.width;
                        newHeight = image.sprite.rect.height;
                    }

                    SetRectTransform(image.rectTransform, currentY, newWidth, newHeight);
                    currentY -= (image.rectTransform.rect.height + Settings.SectionSpacing);
                    imageSections.Add(image);
                }
            }
        }

    private void ClearCanvasComponents()
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

        foreach (Image image in imageSections) {
            if (image != null) {
                DestroyImmediate(image.gameObject);
            }
        }
        imageSections.Clear();
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

        return textComponent;
    }

    // Create image component
    private Image CreateImageComponent(string name)
    {
        GameObject imageObject = new GameObject(name);
        imageObject.transform.SetParent(transform, false);
        Image imageComponent = imageObject.AddComponent<Image>();
        return imageComponent;
    }

    private void SetRectTransform(RectTransform rectTransform, float currentY, float width, float height)
    {
        rectTransform.anchorMin = new Vector2(0.5f, 1f);
        rectTransform.anchorMax = new Vector2(0.5f, 1f);
        rectTransform.pivot = new Vector2(0.5f, 1f);
        rectTransform.anchoredPosition = new Vector2(0, currentY);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }
    #endif
    #endif
    }
}
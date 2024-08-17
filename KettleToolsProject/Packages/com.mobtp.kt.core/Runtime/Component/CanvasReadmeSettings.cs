using UnityEngine;
#if PACKAGE_TEXTMESHPRO
using TMPro;
#endif

namespace Mobtp.KT.Core.Documentation
{

    [System.Serializable]
    public class CanvasReadmeSettings
    {
        #if PACKAGE_TEXTMESHPRO
        public float SectionSpacing = 24f;
        public float HeaderSpacing = 36f;
        public float HeadingFontSize = 24f;
        public float BodyFontSize = 16f;
        public float HeaderWidth = 250f;
        public float BodyWidth = 250f;

        public bool OverrideImageWidth = false;
        public bool OverrideImageHeight = false;
        public float ImageWidth = 250f;
        public float ImageHeight = 250f;
        
        [Header("Additional Font Settings")]
        public TMP_FontAsset FontAsset;
        public TextAlignmentOptions Alignment = TextAlignmentOptions.Center;

        public Color HeaderColor = Color.white;
        public Color BodyColor = Color.white;
        
        [Tooltip("Don't allow user to edit directly by default.\n"
            +"In most cases changes should be done to the readme itself to prevent accidental overriding.")]
        public bool NotEditable = true;

        public void ApplySettings(ref TextMeshProUGUI textComponent, bool isHeader)
        {
            textComponent.font = FontAsset;
            textComponent.fontSize = isHeader ? HeadingFontSize : BodyFontSize;
            textComponent.alignment = Alignment;
            textComponent.color = isHeader ? HeaderColor : BodyColor;
        }
        #endif
    }
}

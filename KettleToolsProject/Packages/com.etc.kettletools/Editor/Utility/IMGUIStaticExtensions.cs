using UnityEngine;
using UnityEditor;
namespace ETC.KettleTools
{
    public static class IMGUIStaticExtensions
    {
        /// <summary>
        /// If true, draws the trueProperties, and if false, draws the falseProperties.
        /// </summary>
        /// <param name="shouldDrawProperty">Should show property or not.</param>
        /// <param name="trueProperties">Properties to draw if true.</param>
        /// <param name="falseProperties">Properties to draw if false.</param>
        public static void ConditionalPropertyField(bool shouldDrawProperty, TrueSerializedProperty trueProperties, FalseSerializedProperty falseProperties)
        {
            if (shouldDrawProperty) {
                trueProperties.DrawProperties();
            } else if(falseProperties != null) {
                falseProperties.DrawProperties();
            }
        }
        /// <summary>
        /// If true, draws the trueProperties.
        /// </summary>
        /// <param name="property">Should show property or not.</param>
        /// <param name="trueProperties">Properties to draw if true.</param>
        public static void ConditionalPropertyField(bool shouldDrawProperty, TrueSerializedProperty trueProperties)
        {
            ConditionalPropertyField(shouldDrawProperty, trueProperties, null);
        }
    }
}

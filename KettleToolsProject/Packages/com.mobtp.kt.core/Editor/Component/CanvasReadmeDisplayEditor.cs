using UnityEngine;
using UnityEditor;

namespace Mobtp.KT.Core.Documentation
{
    [CustomEditor(typeof(CanvasReadmeDisplay))]
    public class CanvasReadmeDisplayEditor : Editor
    {
        // Draw default inspector and draw a button to populate the canvas
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            CanvasReadmeDisplay readmeDisplay = (CanvasReadmeDisplay)target;
            if (GUILayout.Button("Update Canvas From Readme"))
            {
                readmeDisplay.PopulateCanvas();
            }
        }
    }
}
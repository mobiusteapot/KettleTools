using UnityEditor;

namespace ETC.KettleTools.Audio {
    [CustomEditor(typeof(VoiceAudioClipContainer))]
    public class VoiceAudioClipContainerEditor : AudioClipContainerEditor
    {
        // Inherited from AudioContainerObjectEditor
        public override void OnInspectorGUI()
        {
            DrawSubtitles = true;
            base.OnInspectorGUI();
        }
    }
}
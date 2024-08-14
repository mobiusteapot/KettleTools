using UnityEditor;

namespace Mobtp.KettleTools.Audio {
    [CustomEditor(typeof(CharacterAudioContainer))]
    public class CharacterAudioContainerEditor : AudioContainerEditor
    {
        // Inherited from AudioContainerObjectEditor
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
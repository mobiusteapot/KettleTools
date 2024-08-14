using UnityEditor;

namespace Mobtp.KettleTools.Audio {
    [CustomEditor(typeof(PropAudioContainer))]
    public class PropAudioContainerEditor : AudioContainerEditor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
        }
    }
}
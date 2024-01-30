using UnityEditor;

namespace ETC.KettleTools.Audio {
    [CustomEditor(typeof(PropAudioContainer))]
    public class PropAudioContainerEditor : AudioContainerEditor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
        }
    }
}
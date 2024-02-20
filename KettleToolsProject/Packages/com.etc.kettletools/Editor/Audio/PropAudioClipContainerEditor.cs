using UnityEditor;

namespace ETC.KettleTools.Audio {
    [CustomEditor(typeof(PropAudioClipContainer))]
    public class PropAudioClipContainerEditor : AudioClipContainerEditor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
        }
    }
}
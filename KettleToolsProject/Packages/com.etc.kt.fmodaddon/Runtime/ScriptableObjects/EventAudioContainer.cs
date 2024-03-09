using UnityEngine;
using FMODUnity;

namespace ETC.KettleTools.Audio.FMODAddon {
    [CreateAssetMenu(fileName = "EventAudioContainer", menuName = "FMOD Audio/Event Audio Container", order = 224)]
    public class EventAudioContainer : AudioContainer<EventReference> {
        [SerializeField]
        private EventAudioBundle audioBundle;

        EventAudioContainerProperties ecProps;
        // This should be obtained from a unique instance of the scriptable object. 

        protected override void OnBegin() {
            // Defaults to false
            InitializeFmodRuntime();
        }
        private void OnValidate() {
        }
        public override IAudioBundle<EventReference> GetAudioBundle()
        {
            return audioBundle;
        }

        public override IAudioBundle<EventReference> GetAudioBundleByIndex(int index)
        {
            return audioBundle;
        }
        protected override int GetRandomIndex()
        {
            return 0;
        }

        public void StopAllClips() {
            audioBundle.StopAudio();
        }
        public void PlayAllClips(Transform transform) {
            throw new System.NotImplementedException();
        }
        public void SetParameterByEventIndex(int index, FMOD.Studio.PARAMETER_ID id, float value, bool ignoreseekspeed = false) {
            throw new System.NotImplementedException();
            audioBundle.SetParameter(name, value, ignoreseekspeed);
        }
        public void SetParameterByEventIndex(int index, string name, float value, bool ignoreseekspeed = false) {
            throw new System.NotImplementedException();
            audioBundle.SetParameter(name, value, ignoreseekspeed);
        }
#if UNITY_EDITOR
    public override AudioClip GetAudioClipByIndex(int index) {
        throw new System.NotImplementedException();
    }
#endif

    #region FMOD Event Logic
    private void InitializeFmodRuntime() {
        RuntimeUtils.EnforceLibraryOrder();
        if (ecProps.Preload)
        {
            audioBundle.PreloadSample();
        }
    }
    // Instantiate on an object/it's transform when played

    public void PlayEventOnObject(Transform transform) {
        EventReference audio = GetAudioBundle().GetAudio();
        RuntimeManager.PlayOneShot(audio, transform.position);
    }
    #endregion
    }
}
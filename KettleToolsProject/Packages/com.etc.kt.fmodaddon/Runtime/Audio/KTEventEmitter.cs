using System.Collections.Generic;
using UnityEngine;
using FMODUnity;


namespace ETC.KettleTools.Audio.FMODAddon {
    public class KTEventEmitter : EventHandler {
        [SerializeField]
        private EventAudioContainer eventAudioContainer;
        // Paradigm: Keep game events but only use if enabled. Hide conditionally in inspector
        [Tooltip("Game events are custom built-in FMOD events\n"
        + "that can be trigged by specific conditions.\n"
        + "You can call the 'Play' method directly for gameplay reactive behaviour.")]
        public bool UseGameEvents = false;
        public EventPlaybackType playbackType = EventPlaybackType.ByIndex;
        public int EventIndex = 0;
        public EmitterGameEvent PlayEvent = EmitterGameEvent.None;
        public EmitterGameEvent StopEvent = EmitterGameEvent.None;
        public bool TriggerOnce = false;
        [SerializeField, HideInInspector]
        private bool hasTriggered = false;
        private bool isQuitting = false;

        private void Awake() {
            hasTriggered = false;
        }
        public void PlayEventByIndex(int index) => eventAudioContainer.GetAudioBundleByIndex(index).GetAudio();
        public void StopEventByIndex(int index) => eventAudioContainer.GetAudioBundleByIndex(index).StopAudio();
        public void StopAllClips() => eventAudioContainer.StopAllClips();
        
        public void SetParameterByEventIndex(int index, string name, float value, bool ignoreseekspeed = false) {
            eventAudioContainer.SetParameterByEventIndex(index, name, value, ignoreseekspeed);
        }
        public void SetParameterByEventIndex(int index, FMOD.Studio.PARAMETER_ID id, float value, bool ignoreseekspeed = false) {
            eventAudioContainer.SetParameterByEventIndex(index, id, value, ignoreseekspeed);
        }

        private void OnApplicationQuit() {
            isQuitting = true;
        }

        protected override void OnDestroy()
        {
            if (!isQuitting)
            {
                HandleGameEvent(EmitterGameEvent.ObjectDestroy);
                // Todo: Destroy instance?
            }
        }

        protected override void HandleGameEvent(EmitterGameEvent gameEvent) {
            if (!UseGameEvents) return;
            if (PlayEvent == gameEvent) {
                if(playbackType == EventPlaybackType.ByIndex) {
                    PlayEventByIndex(EventIndex);
                } else {
                    PlayAll();
                }
                PlayEventByIndex(EventIndex);
            }
            if (StopEvent == gameEvent) {
                if(playbackType == EventPlaybackType.ByIndex) {
                    StopEventByIndex(EventIndex);
                } else {
                    StopAllClips();
                }
            }
        }

        public void PlayAll() {
            if (TriggerOnce && hasTriggered) {
                return;
            } else if (eventAudioContainer == null) {
                return;
            }
            eventAudioContainer.PlayAllClips(transform);
        }
        public bool IsPlaying(int index) {
            var e = (EventAudioBundle)eventAudioContainer.GetAudioBundleByIndex(index);
            return e.IsPlaying();
        }
    }
}
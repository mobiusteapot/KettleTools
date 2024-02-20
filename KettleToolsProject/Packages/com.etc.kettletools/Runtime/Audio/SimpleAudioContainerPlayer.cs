using UnityEngine;

namespace ETC.KettleTools.Audio {

    /// <summary>
    /// A simple example of how audio containers can be played.
    /// This component can be referenced by events on other scripts.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class SimpleAudioContainerPlayer : MonoBehaviour {
        public AudioClipContainer[] audioContainers;
        private AudioSource _audioSource;
        private void Reset() {
            _audioSource = GetComponent<AudioSource>();
            #if KT_FMOD
            Debug.LogWarning("FMOD Addon is installed, use KTAudioEmitter instead of AudioManager if intending to use FMOD's audio system.");
            #endif
        }
        // Todo: callback when each clip finishes playing?
        public void PlayContainer(int index = 0) {
            _audioSource.Play(audioContainers[index]);
        }
        public void PlayContainerOneShot(int index = 0) {
            _audioSource.PlayOneShot(audioContainers[index]);
        }
        public void PlayContainerWithSubtitle(int index = 0) {
            _audioSource.Play(audioContainers[index]);
            var abSub = audioContainers[index].GetAudioBundle().GetSubtitles();
            for(int j = 0; j < abSub.Length; j++) {
                Debug.Log(abSub[j]);
            }
        }
    }
}
using UnityEngine;

namespace Mobtp.KettleTools.Audio {
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour {
        [Tooltip("Play randomly, if false, play in order")]
        public bool PlayRandomly = false;
        [Tooltip("Finish playing all clips before repeating")]
        public bool DoNotRepeat = true;
        [Range(0.1f, 2f)]
        public float PitchMinimum = 1f;
        [Range(0.1f, 2f)]
        public float PitchMaximum = 1f;
        [Tooltip("All audio clips to play. Will only play the first one if only one is added.")]
        public AudioClip[] audioClips;
        private bool[] _hasPlayed;
        private AudioSource _audioSource;
        private void Reset() {
            _audioSource = GetComponent<AudioSource>();
        }
        // Todo: callback when each clip finishes playing?
        public void PlaySoundClip() {
            RandomizeSoundPitch();
            if(PlayRandomly) {
                PlaySoundClipsRandomly();
            } else {
                PlaySoundClipsInOrder();
            }
        }
        private void Awake(){
            _hasPlayed = new bool[audioClips.Length];
        }

        public void PlaySoundClipsRandomly() {
            // Play randomly without repeats, reset if all have been played (to play again)
            if(CheckIfAllPlayed()){
                    ResetHasPlayed();
            }
            if(DoNotRepeat){
                int randomIndex = Random.Range(0, audioClips.Length);
                if(!_hasPlayed[randomIndex]){
                    _audioSource.PlayOneShot(audioClips[randomIndex]);
                    _hasPlayed[randomIndex] = true;
                }
                else{
                    PlaySoundClipsRandomly();
                }
            }
            else{
                // Play a random clip
                _audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]);
            }
        }
        public void PlaySoundClipsInOrder(){
            // Get first clip that has not been played
            if(CheckIfAllPlayed()){
                Debug.Log("All clips have been played! No new clip to play.");
                return;
            }
            for(int i = 0; i < audioClips.Length; i++){
                if(!_hasPlayed[i]){
                    _audioSource.PlayOneShot(audioClips[i]);
                    _hasPlayed[i] = true;
                    break;
                }
            }
        }
        public void RandomizeSoundPitch(){
            _audioSource.pitch = Random.Range(PitchMinimum, PitchMaximum);
        }

        private bool CheckIfAllPlayed(){
            for(int i = 0; i < _hasPlayed.Length; i++){
                if(!_hasPlayed[i]){
                    return false;
                }
            }
            return true;
        }

        private void ResetHasPlayed(){
            for(int i = 0; i < _hasPlayed.Length; i++){
                _hasPlayed[i] = false;
            }
        }

    }
}
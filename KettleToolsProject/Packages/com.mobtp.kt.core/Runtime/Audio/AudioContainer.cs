using UnityEngine;
using Mobtp.KettleTools.Core;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Mobtp.KettleTools.Audio {
    public abstract class AudioContainer : ManagedScriptableObject {
        [SerializeField]
        private AudioBundle[] _audioBundles;
        [SerializeField]
        private bool _isClipRandomized;
        [SerializeField]
        private bool _isVolumeRandomized;
        // Todo: Custom inspector for min/max?
        [SerializeField, Range(0, 1)]
        private float _volumeMin = 0.8f;
        [SerializeField, Range(0, 1)]
        private float _volumeMax = 1;
        [SerializeField, Range(0, 1)]
        private float _volume = 1;
        [SerializeField]
        private bool _isPitchRandomized;
        [SerializeField, Range(0, 3)]
        private float _pitchMin = 0.8f;
        [SerializeField, Range(0, 3)]
        private float _pitchMax = 1;
        [field: SerializeField, Range(0, 3)]
        private float _pitch = 1;
        // Play randomized without repeating
        private bool[] _hasPlayed;
        private int _hasPlayedCount = 0;

        // Getters and setters
        public float GetPitch() => _isPitchRandomized ? Random.Range(_pitchMin, _pitchMax) : _pitch;
        public float GetVolume() => _isVolumeRandomized ? Random.Range(_volumeMin, _volumeMax) : _volume;

        // Scriptable objects don't have an "Awake", this functions similarly
        protected override void OnBegin() {
            // Defaults to false
            _hasPlayed = new bool[_audioBundles.Length];
        }

        protected override void OnEnd()
        {
            // Currently does nothing
        }


        public AudioBundle.Subtitle GetSubtitleByIndex(int audioContainerIndex, int index)
        {
            if (index < 0 || index >= GetAudioBundleByIndex(audioContainerIndex).Subtitles.Length)
            {
                Debug.LogWarning("Index out of range: " + index);
                return GetAudioBundleByIndex(audioContainerIndex).Subtitles[0];
            }
            return GetAudioBundleByIndex(audioContainerIndex).Subtitles[index];
        }

        public AudioBundle.Subtitle GetSubtitle(AudioBundle audioBundle, int index)
        {
            if (index < 0 || index >= audioBundle.Subtitles.Length)
            {
                Debug.LogWarning("Index out of range: " + index);
                return audioBundle.Subtitles[0];
            }
            return audioBundle.Subtitles[index];
        }
        public int GetSubtitleCountByIndex(int audioBundleIndex)
        {
            return GetAudioBundleByIndex(audioBundleIndex).Subtitles.Length;
        }
        public int GetSubtitleCount(AudioBundle audioBundle)
        {
            return audioBundle.Subtitles.Length;
        }
        /// <summary>
        /// Returns an Audio Bundle, respecting if the clips are to be randomized or not.
        /// </summary>
        /// <returns>A random Audio Bundle, or the first Audio Bundle if not randomized.</returns>
        public AudioBundle GetAudioBundle()
        {
            return _isClipRandomized ? _audioBundles[GetRandomIndex()] : _audioBundles[0];
        }
        /// <summary>
        /// Returns an Audio Bundle by index
        /// </summary>
        /// <param name="index">The index of the target audio bundle. Returns first bundle if not found.</param>
        /// <returns></returns>
        public AudioBundle GetAudioBundleByIndex(int index)
        {
            if (index < 0 || index >= _audioBundles.Length)
            {
                Debug.LogWarning("Index out of range: " + index);
                return _audioBundles[0];
            }
            return _audioBundles[index];
        }

        public void PlayAudio(AudioSource audioSource)
        {
            if (_audioBundles == null || _audioBundles.Length == 0)
            {
                Debug.LogWarning("No audio clips in audio container: " + name);
                return;
            }
            AudioBundle audioContainer = GetAudioBundle();
            audioSource.clip = audioContainer.AudioClip;
            audioSource.volume = GetVolume();
            audioSource.pitch = GetPitch();
            //Debug.Log("Playing audio: " + audioContainer.audioClip.name + " with volume: " + audioSource.volume);
            audioSource.Play();
        }

        public void PlayAudioOneshot(AudioSource audioSource)
        {
            if (_audioBundles == null || _audioBundles.Length == 0)
            {
                Debug.LogWarning("No audio clips in audio container: " + name);
                return;
            }
            AudioBundle audioContainer = GetAudioBundle();
            audioSource.pitch = GetPitch();
            audioSource.PlayOneShot(audioContainer.AudioClip, GetVolume());
        }
        // Get a random index, and reset random array if all have been played
        private int GetRandomIndex()
        {
            int randomIndex = Random.Range(0, _audioBundles.Length);
            if (_hasPlayedCount <= _audioBundles.Length)
            {
                _hasPlayedCount = 0;
                for (int i = 0; i < _hasPlayed.Length; i++)
                {
                    _hasPlayed[i] = false;
                }
            }
            while (_hasPlayed[randomIndex])
            {
                randomIndex = Random.Range(0, _audioBundles.Length);
            }
            _hasPlayed[randomIndex] = true;
            _hasPlayedCount++;
            return randomIndex;
        }

#if UNITY_EDITOR
    // Get the audio only for editor scripts. I don't think there is a reason to get the audio clip in game
    // (Would defeat the whole purpose of this paradigm)

    // Experimented with removing this but managed object reference serialization does not play nice
    public AudioClip GetAudioClipByIndex(int index) {
        if (index < 0 || index > _audioBundles.Length) {
            Debug.LogWarning("Index out of range: " + index);
            return _audioBundles[0].AudioClip;
        }
        return _audioBundles[index].AudioClip;
    }
    public int GetAudioClipCount() {
        return _audioBundles.Length;
    }
#endif
    }
}
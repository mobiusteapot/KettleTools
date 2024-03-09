using UnityEngine;

namespace ETC.KettleTools.Audio
{
    // Container class for traditional unity AudioClip and AudioSource types of audio playback
    [CreateAssetMenu(fileName = "AudioClipContainer", menuName = "Audio/AudioClipContainer", order = 223)]
    public class AudioClipContainer : AudioContainer<AudioClip>
    {
        #if UNITY_EDITOR
        [SerializeField] private AudioContainerType containerType;
        #endif
        [SerializeField]
        protected AudioClipContainerProperties acProps;
        [SerializeField]
        private AudioClipBundle[] _audioBundles;

        // Scriptable objects don't have an "Awake", this functions similarly

        protected override void OnBegin() {
            // Defaults to false
            acProps.HasPlayed = new bool[_audioBundles.Length];
        }

        public override IAudioBundle<AudioClip> GetAudioBundle()
        {
            return acProps.GetClipRandomized() ? _audioBundles[GetRandomIndex()] : _audioBundles[0];
        }
        public override IAudioBundle<AudioClip> GetAudioBundleByIndex(int index)
        {
            if (index < 0 || index >= _audioBundles.Length)
            {
                Debug.LogWarning("Index out of range: " + index);
                return _audioBundles[0];
            }
            return _audioBundles[index];
        }
        public void PlayAudioClip(AudioSource audioSource)
        {
            if (_audioBundles == null || _audioBundles.Length == 0)
            {
                Debug.LogWarning("No audio clips in audio container: " + name);
                return;
            }
            IAudioBundle<AudioClip> ab = GetAudioBundle();
            audioSource.clip = ab.GetAudio();
            audioSource.volume = acProps.GetVolume();
            audioSource.pitch = acProps.GetPitch();
            audioSource.Play();
        }
        public void PlayAudioClipOneshot(AudioSource audioSource)
        {
            if (_audioBundles == null || _audioBundles.Length == 0)
            {
                Debug.LogWarning("No audio clips in audio container: " + name);
                return;
            }
            IAudioBundle<AudioClip> ab = GetAudioBundle();
            audioSource.pitch = acProps.GetPitch();
            audioSource.PlayOneShot(ab.GetAudio(), acProps.GetVolume());
        }
        protected override int GetRandomIndex()
        {
            int randomIndex = Random.Range(0, _audioBundles.Length);
            if (acProps.HasPlayedCount <= _audioBundles.Length)
            {
                acProps.HasPlayedCount = 0;
                for (int i = 0; i < acProps.HasPlayed.Length; i++)
                {
                    acProps.HasPlayed[i] = false;
                }
            }
            while (acProps.HasPlayed[randomIndex])
            {
                randomIndex = Random.Range(0, _audioBundles.Length);
            }
            acProps.HasPlayed[randomIndex] = true;
            acProps.HasPlayedCount++;
            return randomIndex;
        }

#if UNITY_EDITOR
    public override AudioClip GetAudioClipByIndex(int index) {
        if (index < 0 || index > _audioBundles.Length) {
            Debug.LogWarning("Index out of range: " + index);
            return _audioBundles[0].GetAudio();
        }
        return _audioBundles[index].GetAudio();
    }
    #endif
    }
}

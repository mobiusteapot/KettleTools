using UnityEngine;

namespace ETC.KettleTools.Audio
{
    /// <summary>
    /// Audio and relevant subtitle information for the given audio clip
    /// An audio bundle is intended to hold an audio clip and any number of subtiles with timestamps.
    /// </summary>
    [System.Serializable]
    public class AudioClipBundle : IAudioBundle<AudioClip>
    {
        [SerializeField]
        private AudioClip _audioClip;
        [SerializeField]
        private Subtitle[] _subtitles;

        // Get Audio interface implementation
        public AudioClip GetAudio() => _audioClip;   
        public Subtitle[] GetSubtitles() => _subtitles;
        // Todo: Make this stop the audio clip
        public void StopAudio(){
            throw new System.NotImplementedException();
        }

    }
}

using UnityEngine;

namespace ETC.KettleTools.Audio {
    public static class AudioSourceStaticExtension
    {
        /// <summary>
        /// Plays an audio source object. Respects audio container's volume, pitch, and randomization settings.
        /// </summary>
        public static void PlayOneShot(this AudioSource audioSource, AudioContainer audioContainerObject)
        {
            audioContainerObject.PlayAudioOneshot(audioSource);
        }

        public static void Play(this AudioSource audioSource, AudioContainer audioContainerObject)
        {
            audioContainerObject.PlayAudio(audioSource);
        }
    }
}

using UnityEngine;

namespace ETC.KettleTools.Audio {
    public static class AudioSourceStaticExtension
    {
        /// <summary>
        /// Plays an audio source object. Respects audio container's volume, pitch, and randomization settings.
        /// </summary>
        public static void PlayOneShot(this AudioSource audioSource, AudioClipContainer audioContainerObject)
        {
            audioContainerObject.PlayAudioClipOneshot(audioSource);
        }

        public static void Play(this AudioSource audioSource, AudioClipContainer audioContainerObject)
        {
            audioContainerObject.PlayAudioClip(audioSource);
        }
    }
}

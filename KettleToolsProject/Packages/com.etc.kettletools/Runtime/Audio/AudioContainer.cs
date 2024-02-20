using UnityEngine;

namespace ETC.KettleTools.Audio {
    public abstract class AudioContainer<T> : ManagedScriptableObject {

        public Subtitle GetSubtitle(IAudioBundle<T> audioBundle, int index)
        {
            var abSub = audioBundle.GetSubtitles();
            if (index < 0 || index >= abSub.Length)
            {
                Debug.LogWarning("Index out of range: " + index);
                return abSub[0];
            }
            return abSub[index];
        }
        public int GetSubtitleCountByIndex(int audioBundleIndex)
        {
            var ab = GetAudioBundleByIndex(audioBundleIndex);
            return ab.GetSubtitles().Length;
        }
        public int GetSubtitleCount(IAudioBundle<T> audioBundle)
        {
            return audioBundle.GetSubtitles().Length;
        }
        /// <summary>
        /// Returns an Audio Bundle, respecting if the clips are to be randomized or not.
        /// </summary>
        /// <returns>A random Audio Bundle, or the first Audio Bundle if not randomized.</returns>
        public abstract IAudioBundle<T> GetAudioBundle();
        /// <summary>
        /// Returns an Audio Bundle.
        /// </summary>
        /// <param name="index">The index of the target audio bundle. Returns first bundle if not found.</param>
        /// <returns></returns>
        public abstract IAudioBundle<T> GetAudioBundleByIndex(int index);

        // Get a random index, and reset random array if all have been played
        protected abstract int GetRandomIndex();

#if UNITY_EDITOR
    // Get the audio only for editor scripts. I don't think there is a reason to get the audio clip in game
    // (Would defeat the whole purpose of this paradigm)

    // Experimented with removing this but managed object reference serialization does not play nice
    public abstract AudioClip GetAudioClipByIndex(int index);
#endif
    }
}
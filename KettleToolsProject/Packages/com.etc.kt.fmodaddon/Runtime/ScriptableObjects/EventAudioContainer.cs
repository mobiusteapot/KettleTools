using UnityEngine;
using FMODUnity;

namespace ETC.KettleTools.Audio.FMODAddon {
    public abstract class EventAudioContainer : AudioContainer<EventReference> {
        [SerializeField,ParamRef, Header("Parameters to apply to all audio bundles. Not implemented yet.")]
        private ParamRef[] ParamsToApply;
        [SerializeField]
        private EventAudioBundle[] audioBundles;

        public bool RandomizeClips = false;
        [HideInInspector]
        public bool[] HasPlayed;
        [HideInInspector]
        public int HasPlayedCount = 0;
        protected override void OnBegin() {
            // Defaults to false
            HasPlayed = new bool[audioBundles.Length];
        }
        public override IAudioBundle<EventReference> GetAudioBundle()
        {
            return RandomizeClips ? audioBundles[GetRandomIndex()] : audioBundles[0];
        }
        public override IAudioBundle<EventReference> GetAudioBundleByIndex(int index)
        {
            if (index < 0 || index >= audioBundles.Length)
            {
                Debug.LogWarning("Index out of range: " + index);
                return audioBundles[0];
            }
            return audioBundles[index];
        }
        protected override int GetRandomIndex()
        {
            int randomIndex = Random.Range(0, audioBundles.Length);
            if (HasPlayedCount <= audioBundles.Length)
            {
                HasPlayedCount = 0;
                for (int i = 0; i < HasPlayed.Length; i++)
                {
                    HasPlayed[i] = false;
                }
            }
            while (HasPlayed[randomIndex])
            {
                randomIndex = Random.Range(0, audioBundles.Length);
            }
            HasPlayed[randomIndex] = true;
            HasPlayedCount++;
            return randomIndex;
        }
#if UNITY_EDITOR
    public override AudioClip GetAudioClipByIndex(int index) {
        throw new System.NotImplementedException();
    }
#endif
    }
}
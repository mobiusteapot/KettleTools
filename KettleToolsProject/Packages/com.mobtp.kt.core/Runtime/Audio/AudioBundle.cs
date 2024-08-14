using UnityEngine;

namespace Mobtp.KettleTools.Audio
{
    /// <summary>
    /// Audio and relevant subtitle information for the given audio clip
    /// </summary>
    [System.Serializable]
    public class AudioBundle
    {

        [field: SerializeField]
        public AudioClip AudioClip { get; private set; }

        [System.Serializable]
        public struct Subtitle
        {
            [TextArea(1, 4)]
            public string text;
            public float stopTime;
        }
        [field: SerializeField]
        public Subtitle[] Subtitles { get; private set; }
    }
}

using UnityEngine;

namespace ETC.KettleTools.Audio
{
    [System.Serializable]
    public struct Subtitle
    {
        [TextArea(1, 4)]
        public string text;
        public float stopTime;
    }
}
using UnityEngine;
namespace ETC.KettleTools.Audio {
    // One container per TYPE of audio, for organization (only voices have subtitles right now)
    [CreateAssetMenu(fileName = "PropAudioContainer", menuName = "Audio/Prop Audio Container", order = 223)]
    public class PropAudioClipContainer : AudioClipContainer {}
}
using UnityEngine;

namespace ETC.KettleTools.Audio {
    // One container per TYPE of audio (so randomization can be invoked)
    [CreateAssetMenu(fileName = "CharacterAudioContainer", menuName = "Audio/Character Audio Container", order = 223)]
    public class VoiceAudioClipContainer : AudioClipContainer {}
}
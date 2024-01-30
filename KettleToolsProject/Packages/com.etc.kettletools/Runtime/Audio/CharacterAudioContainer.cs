using UnityEngine;

namespace ETC.KettleTools.Audio {
    // One container per TYPE of audio (so randomization can be invoked)
    [CreateAssetMenu(fileName = "CharacterAudioContainer", menuName = "Audio/CharacterAudioContainer", order = 223)]
    public class CharacterAudioContainer : AudioContainer {}
}
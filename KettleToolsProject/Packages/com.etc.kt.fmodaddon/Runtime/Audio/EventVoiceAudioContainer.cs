using UnityEngine;

namespace ETC.KettleTools.Audio.FMODAddon {
    // One container per TYPE of audio (so randomization can be invoked)
    [CreateAssetMenu(fileName = "EventCharacterAudioContainer", menuName = "FMOD Audio/Event Character Audio Container", order = 224)]
    public class EventVoiceAudioContainer : EventAudioContainer {}
}
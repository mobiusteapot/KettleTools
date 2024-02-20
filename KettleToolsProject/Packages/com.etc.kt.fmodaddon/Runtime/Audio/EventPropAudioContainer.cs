using UnityEngine;
namespace ETC.KettleTools.Audio.FMODAddon {
    // One container per TYPE of audio (so randomization can be invoked)
    [CreateAssetMenu(fileName = "EventPropAudioContainer", menuName = "FMOD Audio/Event Prop Audio Container", order = 224)]
    public class EventPropAudioContainer : EventAudioContainer {}
}
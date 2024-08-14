using UnityEngine;
namespace Mobtp.KettleTools.Audio {
    // One container per TYPE of audio (so randomization can be invoked)
    [CreateAssetMenu(fileName = "PropAudioContainer", menuName = "Audio/PropAudioContainer", order = 223)]
    public class PropAudioContainer : AudioContainer {}
}
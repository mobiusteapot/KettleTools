using UnityEngine;
using System;
namespace ETC.KettleTools.Audio.FMODAddon
{
    [Serializable]
    public class EventAudioContainerProperties { 
        // Identical to properties from an event emitter
        // Exception being no triggeronce since the script that invokes the event should handle that responsibility
        public bool AllowFadeout = true;
        public bool Preload = false;
        public bool AllowNonRigidbodyDoppler = false;
        public bool OverrideAttenuation = false;
        public float OverrideMinDistance = -1.0f;
        public float OverrideMaxDistance = -1.0f;
    }

}

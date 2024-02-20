using UnityEngine;
using FMODUnity;
namespace ETC.KettleTools.Audio.FMODAddon
{
    [System.Serializable]
    public class EventAudioBundle : IAudioBundle<EventReference>
    {

        [SerializeField]
        private EventReference eventReference;
        [SerializeField]
        public ParamRef[] parameters;
        [SerializeField]
        private Subtitle[] subtitles;
        public EventReference GetAudio() => eventReference;
        public Subtitle[] GetSubtitles() => subtitles;
        public ParamRef[] Parameters => parameters;

        // public ParamRef GetParamRefByString(string paramName)
        // {
        //     foreach (var param in Parameters)
        //     {
        //         if (param.Name == paramName)
        //         {
        //             return param;
        //         }
        //     }
        //     Debug.LogWarning("Parameter not found: " + paramName);
        //     return null;
        // }
    }
}
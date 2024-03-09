using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
namespace ETC.KettleTools.Audio.FMODAddon
{
    [System.Serializable]
    public class EventAudioBundle : IAudioBundle<EventReference>
    {
        // One transform per 
        public Transform InstanceTransform;

        [SerializeField]
        private EventReference eventReference;
        [SerializeField]
        public ParamRef[] parameters;
        [SerializeField]
        private Subtitle[] subtitles;

        public EventReference GetAudio() => eventReference;
        public Subtitle[] GetSubtitles() => subtitles;
        public ParamRef[] Parameters => parameters;
        private EventAudioContainerProperties contProp;
        public FMOD.Studio.EventDescription eventDescription;

        public FMOD.Studio.EventInstance instance;
        private static List<Transform> activeEmitterTrans = new List<Transform>();
        private List<ParamRef> cachedParams = new List<ParamRef>();
        private bool isQuitting = false;
        public bool PlayOnDestroy { get; private set; }
        private bool WasPreloaded { get; set; }
        private bool isOneshot = false;
        public bool IsActive { get; private set; }
        private float MaxDistance
        {
            get
            {
                if(contProp == null)
                {
                    return 1.0f;
                }
                if (contProp.OverrideAttenuation)
                {
                    return contProp.OverrideMaxDistance;
                }

                if (!eventDescription.isValid())
                {
                    Lookup();
                }

                float minDistance, maxDistance;
                eventDescription.getMinMaxDistance(out minDistance, out maxDistance);
                return maxDistance;
            }
        }


        public void PreloadSample(){
            Lookup();
            eventDescription.loadSampleData();
            WasPreloaded = true;
        }
        private void Lookup()
        {
            eventDescription = RuntimeManager.GetEventDescription(eventReference);

            if (eventDescription.isValid())
            {
                for (int i = 0; i < Parameters.Length; i++)
                {
                    FMOD.Studio.PARAMETER_DESCRIPTION param;
                    eventDescription.getParameterDescriptionByName(Parameters[i].Name, out param);
                    Parameters[i].ID = param.id;
                }
            }
        }

        public void PlayFromContainer(Transform playTransform, EventAudioContainerProperties properties, bool playOnce = false){
            contProp = properties;
            cachedParams.Clear();

            if (!eventDescription.isValid())
            {
                Lookup();
            }

            bool isSnapshot;
            eventDescription.isSnapshot(out isSnapshot);

            if (!isSnapshot)
            {
                eventDescription.isOneshot(out isOneshot);
            }

            bool is3D;
            eventDescription.is3D(out is3D);

            IsActive = true;

            if (is3D && !isOneshot && Settings.Instance.StopEventsOutsideMaxDistance)
            {
                RegisterActiveEmitter(playTransform);
                UpdatePlayingStatus(playTransform, true, playOnce: playOnce);
            }
            else
            {
                PlayInstance(playTransform);
            }
        }
        public bool IsPlaying()
        {
            if (instance.isValid())
            {
                FMOD.Studio.PLAYBACK_STATE playbackState;
                instance.getPlaybackState(out playbackState);
                return (playbackState != FMOD.Studio.PLAYBACK_STATE.STOPPED);
            }
            return false;
        }
        public void StopAudio(){
            foreach (var emitter in activeEmitterTrans) {
                if (emitter is not null) {
                    StopInstance(emitter, false);
                }
            }
        }

        private static void RegisterActiveEmitter(Transform emitter)
        {
            if (!activeEmitterTrans.Contains(emitter))
            {
                activeEmitterTrans.Add(emitter);
            }
        }

        private static void DeregisterActiveEmitter(Transform emitter)
        {
            activeEmitterTrans.Remove(emitter);
        }
        private static void DeregisterAllEmitters(){
            activeEmitterTrans.Clear();
        }

         private void UpdatePlayingStatus(Transform playTransform, bool force = false, bool playOnce = false)
        {
            // If at least one listener is within the max distance, ensure an event instance is playing
            bool playInstance = StudioListener.DistanceSquaredToNearestListener(playTransform.position) <= (MaxDistance * MaxDistance);

            if (force || playInstance != IsPlaying())
            {
                if (playInstance)
                {
                    PlayInstance(playTransform);
                }
                else
                {
                    StopInstance(playTransform, playOnce);
                }
            }
        }

         private void PlayInstance(Transform playTransform)
        {
            if (!instance.isValid())
            {
                instance.clearHandle();
            }

            // Let previous oneshot instances play out
            if (isOneshot && instance.isValid())
            {
                instance.release();
                instance.clearHandle();
            }

            bool is3D;
            eventDescription.is3D(out is3D);

            if (!instance.isValid())
            {
                eventDescription.createInstance(out instance);

                // Only want to update if we need to set 3D attributes
                if (is3D)
                {
                    var transform = playTransform;
#if UNITY_PHYSICS_EXIST
                    if (GetComponent<Rigidbody>())
                    {
                        Rigidbody rigidBody = GetComponent<Rigidbody>();
                        instance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject, rigidBody));
                        RuntimeManager.AttachInstanceToGameObject(instance, transform, rigidBody);
                    }
                    else
#endif
#if UNITY_PHYSICS2D_EXIST
                    if (GetComponent<Rigidbody2D>())
                    {
                        var rigidBody2D = GetComponent<Rigidbody2D>();
                        instance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject, rigidBody2D));
                        RuntimeManager.AttachInstanceToGameObject(instance, transform, rigidBody2D);
                    }
                    else
#endif
                    {
                        instance.set3DAttributes(RuntimeUtils.To3DAttributes(playTransform.gameObject));
                        RuntimeManager.AttachInstanceToGameObject(instance, transform, contProp.AllowNonRigidbodyDoppler);
                    }
                }
            }

            foreach (var param in parameters){
                instance.setParameterByID(param.ID, param.Value);
            }

            foreach (var cachedParam in cachedParams){
                instance.setParameterByID(cachedParam.ID, cachedParam.Value);
            }

            if (is3D && contProp.OverrideAttenuation){
                instance.setProperty(FMOD.Studio.EVENT_PROPERTY.MINIMUM_DISTANCE, contProp.OverrideMinDistance);
                instance.setProperty(FMOD.Studio.EVENT_PROPERTY.MAXIMUM_DISTANCE, contProp.OverrideMaxDistance);
            }

            instance.start();
        }

        public void SetParameter(string name, float value, bool ignoreseekspeed = false) {
            if (Settings.Instance.StopEventsOutsideMaxDistance && IsActive) {
                string findName = name;
                ParamRef cachedParam = cachedParams.Find(x => x.Name == findName);

                if (cachedParam == null) {
                    FMOD.Studio.PARAMETER_DESCRIPTION paramDesc;
                    eventDescription.getParameterDescriptionByName(name, out paramDesc);

                    cachedParam = new ParamRef();
                    cachedParam.ID = paramDesc.id;
                    cachedParam.Name = paramDesc.name;
                    cachedParams.Add(cachedParam);
                }

                cachedParam.Value = value;
            }

            if (instance.isValid()) {
                instance.setParameterByName(name, value, ignoreseekspeed);
            }
        }

        public void SetParameter(FMOD.Studio.PARAMETER_ID id, float value, bool ignoreseekspeed = false) {
            if (Settings.Instance.StopEventsOutsideMaxDistance && IsActive) {
                FMOD.Studio.PARAMETER_ID findId = id;
                ParamRef cachedParam = cachedParams.Find(x => x.ID.Equals(findId));

                if (cachedParam == null) {
                    FMOD.Studio.PARAMETER_DESCRIPTION paramDesc;
                    eventDescription.getParameterDescriptionByID(id, out paramDesc);

                    cachedParam = new ParamRef();
                    cachedParam.ID = paramDesc.id;
                    cachedParam.Name = paramDesc.name;
                    cachedParams.Add(cachedParam);
                }
                cachedParam.Value = value;
            }

            if (instance.isValid()) {
                instance.setParameterByID(id, value, ignoreseekspeed);
            }
        }

        private void StopInstance(Transform playTransform, bool playOnce) {
            if (playOnce) {
                DeregisterActiveEmitter(playTransform);
            }

            if (instance.isValid()) {
                instance.stop(contProp.AllowFadeout ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT : FMOD.Studio.STOP_MODE.IMMEDIATE);
                instance.release();
                if (!contProp.AllowFadeout) {
                    instance.clearHandle();
                }
            }
        }
         private void OnApplicationQuit()
        {
            isQuitting = true;
        }
        protected void OnDestroy()
        {
            if (!isQuitting) {
                if(PlayOnDestroy) {
                    foreach (var emitter in activeEmitterTrans) {
                        PlayInstance(emitter);
                    }
                }

                if (instance.isValid()) {
                    RuntimeManager.DetachInstanceFromGameObject(instance);
                    if (eventDescription.isValid() && isOneshot) {
                        instance.release();
                        instance.clearHandle();
                    }
                }

                DeregisterAllEmitters();

                if (WasPreloaded) {
                    eventDescription.unloadSampleData();
                }
            }
        }
    }
}
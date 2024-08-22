using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Mobtp.KettleTools.Core {
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public abstract class ManagedScriptableObject : ScriptableObject {
        abstract protected void OnBegin();
        abstract protected void OnEnd();
 
#if UNITY_EDITOR
        protected void OnEnable() {
            EditorApplication.playModeStateChanged += OnPlayStateChange;
        }
 
        protected void OnDisable() {
            EditorApplication.playModeStateChanged -= OnPlayStateChange;
        }
 
        void OnPlayStateChange(PlayModeStateChange state) {
            if(state == PlayModeStateChange.EnteredPlayMode) {
                OnBegin();
            }
            else if(state == PlayModeStateChange.ExitingPlayMode) {
                OnEnd();
            }
        }
#else
        protected void OnEnable() {
            OnBegin();
        }
 
        protected void OnDisable() {
            OnEnd();
        }
#endif
    }
}
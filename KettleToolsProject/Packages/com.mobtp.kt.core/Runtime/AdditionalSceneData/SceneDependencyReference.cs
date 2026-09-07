using System;
using UnityEngine;

namespace Mobtp.KettleTools.Scenes {
    [Serializable]
    public sealed class SceneDependencyReference {
        [SerializeField]
        private string sceneGuid = string.Empty;

        public string SceneGuid => sceneGuid;
    }
}

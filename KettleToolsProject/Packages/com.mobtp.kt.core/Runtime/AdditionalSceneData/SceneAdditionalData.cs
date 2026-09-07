using System.Collections.Generic;
using UnityEngine;
using Mobtp.KT.Core.Docs;

namespace Mobtp.KettleTools.Scenes {
    public class SceneAdditionalData : ScriptableObject {
        [HideInInspector]
        public Readme readme;
        [HideInInspector]
        public SceneReadmeVisibility showSceneReadmeSetting = SceneReadmeVisibility.onAssetSelect;

        [SerializeField]
        private List<SceneDependencyReference> sceneDependencies = new List<SceneDependencyReference>();

        [SerializeField]
        private bool autoOpenAdditionalScenesInEditor;

        public IReadOnlyList<SceneDependencyReference> SceneDependencies => sceneDependencies;
        public bool AutoOpenAdditionalScenesInEditor => autoOpenAdditionalScenesInEditor;
    }
}

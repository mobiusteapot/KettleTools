using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mobtp.KT.Core.Docs;

namespace Mobtp.KettleTools.Scenes {
public class SceneAdditionalData : ScriptableObject {
        // Stores all scene by scene data custom editors may need
        // HideInInspector is used to prevent auto-drawing for fields which need custom drawing logic
        [HideInInspector]
        public Readme readme;
        [HideInInspector]
        public SceneReadmeVisibility showSceneReadmeSetting = SceneReadmeVisibility.onAssetSelect;
    }
}
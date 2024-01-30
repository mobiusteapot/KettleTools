using System;
using ETC.KettleTools.Documentation;
namespace ETC.KettleTools.Scenes {
    [Flags]
    public enum SceneReadmeVisibility {
        never = 0,
        onAssetSelect = 1 << 0,
        onSceneOpen = 1 << 1,
        always = ~0,
    }
}
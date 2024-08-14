using System;
using Mobtp.KT.Core.Documentation;
namespace Mobtp.KettleTools.Scenes {
    [Flags]
    public enum SceneReadmeVisibility {
        never = 0,
        onAssetSelect = 1 << 0,
        onSceneOpen = 1 << 1,
        always = ~0,
    }
}
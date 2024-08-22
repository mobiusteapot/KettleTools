using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Mobtp.KT.Core.Docs {
    // Menu for adding monobehaviour
    [AddComponentMenu("Editor/Readme")]
    public class ReadmeComponent : MonoBehaviour {
        public Readme Readme;
    }
}
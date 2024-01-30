using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace ETC.KettleTools.Documentation {
    // Menu for adding monobehaviour
    [AddComponentMenu("Editor/Readme")]
    public class ReadmeComponent : MonoBehaviour {
        public Readme Readme;
    }
}
using UnityEngine;

namespace Mobtp.KT.Core.Utils {
    public static class ComponentUtil {
        // Iterate through all children of the gameobject and try to get the component
        // If searchAllChildren is true, then recursively search through children's children
        public static bool TryGetComponentInChildren<T>(this GameObject gameObject, out T component, bool searchAllChildren = false) where T : Component{
            component = null;
            GameObject[] children = new GameObject[gameObject.transform.childCount];
            for (int i = 0; i < gameObject.transform.childCount; i++) {
                children[i] = gameObject.transform.GetChild(i).gameObject;
            }
            for (int i = 0; i < children.Length; i++) {
                if (children[i].TryGetComponent(out component)) {
                    return true;
                }
                else if (searchAllChildren) {
                    if (children[i].TryGetComponentInChildren(out component, true)) {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool TryGetComponentInChildren<T>(this Component component, out T result, bool searchAllChildren = false) where T : Component {
            return component.gameObject.TryGetComponentInChildren(out result, searchAllChildren);
        }

        public static bool TryGetComponentsInChildren<T>(this GameObject gameObject, out T[] components, bool searchAllChildren = false) where T : Component {
            components = gameObject.GetComponentsInChildren<T>();
            if (components.Length > 0) {
                return true;
            }
            else if (searchAllChildren) {
                GameObject[] children = new GameObject[gameObject.transform.childCount];
                for (int i = 0; i < gameObject.transform.childCount; i++) {
                    children[i] = gameObject.transform.GetChild(i).gameObject;
                }
                for (int i = 0; i < children.Length; i++) {
                    if (children[i].TryGetComponentsInChildren(out components, true))
                        return true;
                }
            }
            return false;
        }
        public static bool TryGetComponentsInChildren<T>(this Component component, out T[] result, bool searchAllChildren = false) where T : Component {
            return component.gameObject.TryGetComponentsInChildren(out result, searchAllChildren);
        }
    }
}
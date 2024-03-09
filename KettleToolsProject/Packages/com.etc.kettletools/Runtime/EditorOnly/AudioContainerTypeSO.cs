using UnityEngine;

namespace ETC.KettleTools.Audio
{
    //
    /// <summary>
    /// Hack so serialized references to this object can exist.
    /// No objects should inherit from this other than AudioContainerType
    /// </summary>
    public abstract class AudioContainerType : ScriptableObject
    {
    }
}

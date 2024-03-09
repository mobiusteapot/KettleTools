using UnityEngine;
using UnityEditor;
using ETC.KettleTools.EditorExtensions;

namespace ETC.KettleTools.Audio
{
    [CreateAssetMenu(fileName = "AudioContainerType", menuName = "KettleTools/Audio/AudioContainerType")]
    public class AudioContainerTypeSO : AudioContainerType
    {
        public string Name;
        public Texture2D Icon;
        public FolderReference AudioContainerFolder;
    }
}

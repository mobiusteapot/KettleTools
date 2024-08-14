using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Codice.CM.Common;
using System.Linq;
namespace Mobtp.KettleTools
{
    public abstract class ConditionalSerializedProperty{
        public List<SerializedProperty> SerializedProperties;
        public ConditionalSerializedProperty(params SerializedProperty[] serializedProperties)
        {
            SerializedProperties = serializedProperties.ToList();
        }
        public void DrawProperties()
        {
            SerializedProperties.ForEach(p => EditorGUILayout.PropertyField(p));
        }
    }
    public class TrueSerializedProperty : ConditionalSerializedProperty {
        public TrueSerializedProperty(params SerializedProperty[] serializedProperties) : base(serializedProperties) {}
    }
    public class FalseSerializedProperty : ConditionalSerializedProperty {
        public FalseSerializedProperty(params SerializedProperty[] serializedProperties) : base(serializedProperties) {}
    }
}

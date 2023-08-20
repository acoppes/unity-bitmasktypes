using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gemserk.BitmaskTypes.Editor
{
    [CustomPropertyDrawer(typeof(BitMaskAttribute))]
    public class BitMaskAttributePropertyDrawer : PropertyDrawer
    {
        private float heightPerProperty = 19;

        private static readonly string[] maskNames =
        {
            "Type0","Type1","Type2","Type3","Type4","Type5","Type6","Type7",
            "Type8","Type9","Type10","Type11","Type12","Type13","Type14","Type15",
            "Type16","Type17","Type18","Type19","Type20","Type21","Type22","Type23",
            "Type24","Type25","Type26","Type27","Type28","Type29","Type30","Type31",
        };
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var maskAttribute = attribute as BitMaskAttribute;

            var bits = 8;

            if (maskAttribute != null)
            {
                bits = maskAttribute.bits;
            }

            var names = maskNames.Take(bits).ToArray();
            
            // if (names.Length == 0)
            // {
            //     EditorGUI.PropertyField(position, property, label);
            //     return;
            // }

            position.height = heightPerProperty;

            EditorGUI.BeginChangeCheck();
            var newValue = EditorGUI.MaskField(position, label, property.intValue, names);

            if (EditorGUI.EndChangeCheck())
            {
                property.intValue = newValue;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var names = maskNames;

            if (names.Length == 0)
            {
                return base.GetPropertyHeight(property, label);
            }

            return heightPerProperty;
        }

        public override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            return true;
        }
    }
}
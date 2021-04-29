using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gemserk.BitmaskTypes.Editor
{
    [CustomPropertyDrawer(typeof(EnumNameAttribute))]
    public class EnumNameAttributePropertyDrawer : PropertyDrawer
    {
        public struct TypeName
        {
            public int type;
            public string name;

            public TypeName(int t, string n)
            {
                type = t;
                name = n;
            }
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var enumNameAttribute = attribute as EnumNameAttribute;

            var names = new TypeName[]
            {
                new TypeName(1 << 2, "Second"),
                new TypeName(1 << 3, "Third"),
                new TypeName(1 << 1, "First"),
            };
            
            // find asset 

            var namesList = names.ToList();
            namesList.Sort((a, b) => a.type.CompareTo(b.type));
            var newNames = namesList.Select(n => n.name).ToArray();

            EditorGUI.BeginChangeCheck();
            var newValue = EditorGUI.MaskField(position, label, property.intValue, newNames);

            if (EditorGUI.EndChangeCheck())
            {
                property.intValue = newValue;
            }

            // EditorGUI.PropertyField(position, property, label);
        }

        // public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        // {
        //     return base.GetPropertyHeight(property, label);
        // }

        // public override VisualElement CreatePropertyGUI(SerializedProperty property)
        // {
        //     return base.CreatePropertyGUI(property);
        // }

        public override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            return true;
        }
    }
}
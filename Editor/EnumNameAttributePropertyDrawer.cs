using UnityEditor;
using UnityEngine;

namespace Gemserk.BitmaskTypes.Editor
{
    [CustomPropertyDrawer(typeof(EnumNameAttribute))]
    public class EnumNameAttributePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var enumNameAttribute = attribute as EnumNameAttribute;

            var assets = AssetDatabase.FindAssets($"t:{typeof(EnumNameTypeAsset).Name} {enumNameAttribute.namesAsset}");
            if (assets.Length == 0)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            var typeAsset = AssetDatabase.LoadAssetAtPath<EnumNameTypeAsset>(AssetDatabase.GUIDToAssetPath(assets[0]));

            var names = typeAsset.GetOrderedNames();

            if (names.Length == 0)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            EditorGUI.BeginChangeCheck();
            var newValue = EditorGUI.MaskField(position, label, property.intValue, names);

            if (EditorGUI.EndChangeCheck())
            {
                property.intValue = newValue;
            }
        }

        public override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            return true;
        }
    }
}
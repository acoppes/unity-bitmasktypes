using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gemserk.BitmaskTypes.Editor
{
    [CustomPropertyDrawer(typeof(MaskAttribute))]
    public class MaskAttributePropertyDrawer : PropertyDrawer
    {
        private float heightPerProperty = 19;

        private static string[] maskNames =
        {
            "Type0",
            "Type1",
        };
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var maskAttribute = attribute as MaskAttribute;

            // var assets = AssetDatabase.FindAssets($"t:{nameof(EnumNameTypeAsset)} {enumNameAttribute.namesAsset}");
            // if (assets.Length == 0)
            // {
            //     EditorGUI.PropertyField(position, property, label);
            //     return;
            // }

            // var typeAsset = AssetDatabase.LoadAssetAtPath<EnumNameTypeAsset>(AssetDatabase.GUIDToAssetPath(assets[0]));
            //
            // var names = typeAsset.GetOrderedNames();

            var names = maskNames;
            
            if (names.Length == 0)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            position.height = heightPerProperty;

            EditorGUI.BeginChangeCheck();
            var newValue = EditorGUI.MaskField(position, label, property.intValue, names);

            if (EditorGUI.EndChangeCheck())
            {
                property.intValue = newValue;
            }

            // if (typeAsset.groupTypes.Count > 0)
            // {
            //     EditorGUI.BeginChangeCheck();
            //     position.x += 20;
            //     position.width -= 20;
            //     position.y += heightPerProperty;
            //
            //     var options = new List<string>
            //     {
            //         // "Click to select"
            //     };
            //     
            //     options.AddRange(typeAsset.groupTypes.Select(g => g.name));
            //     
            //     var content = new GUIContent($"Preconfigured {typeAsset.name}", $"Overrides value with preconfigured bitmask {typeAsset.name}");
            //     var group = EditorGUI.Popup(position, content, -1, options.Select(g => new GUIContent(g)).ToArray());
            //
            //     if (EditorGUI.EndChangeCheck())
            //     {
            //         var groupType = typeAsset.groupTypes[group];
            //         property.intValue = groupType.value;
            //     }
            // }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var maskAttribute = attribute as MaskAttribute;
            
            // var assets = AssetDatabase.FindAssets($"t:{nameof(EnumNameTypeAsset)} {enumNameAttribute.namesAsset}");
            // if (assets.Length == 0)
            // {
            //     return base.GetPropertyHeight(property, label);
            // }

            // var typeAsset = AssetDatabase.LoadAssetAtPath<EnumNameTypeAsset>(AssetDatabase.GUIDToAssetPath(assets[0]));
            //
            // var names = typeAsset.GetOrderedNames();
            
            var names = maskNames;

            if (names.Length == 0)
            {
                return base.GetPropertyHeight(property, label);
            }

            return heightPerProperty;
            
            // if (typeAsset.groupTypes.Count == 0)
            //     return heightPerProperty;
            //
            // return heightPerProperty * 2;
        }

        public override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            return true;
        }
    }
}
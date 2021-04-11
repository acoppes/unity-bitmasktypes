using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gemserk.BitmaskTypes.Editor
{
    [CustomPropertyDrawer(typeof(BaseTypeMaskAttribute), true)]
    public class BaseTypeMaskPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var typeAttribute = attribute as BaseTypeMaskAttribute;

            if (typeAttribute == null)
            {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }

            if (property.serializedObject.isEditingMultipleObjects)
            {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }

            var baseSpecificType = typeAttribute.type;
            var allTypes = AssetDatabase.FindAssets($"t:{baseSpecificType.Name}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<BaseTypeAsset>)
                .ToList();
        
            allTypes.Sort((a, b) => a.bitmaskValue.CompareTo(b.bitmaskValue));

            var types = property.FindPropertyRelative("types");
            var currentMask = 0;
            for (var i = 0; i < types.arraySize; i++)
            {
                var baseType = types.GetArrayElementAtIndex(i).objectReferenceValue as BaseTypeAsset;
                // Missing reference or empty array entry.
                if (baseType == null)
                    continue;
                currentMask |= baseType.bitmaskValue;
            }
        
            EditorGUI.BeginChangeCheck();
            var newMask = EditorGUI.MaskField(position, property.displayName, currentMask, allTypes.Select(t => t.name).ToArray());

            if (EditorGUI.EndChangeCheck())
            {
                types.ClearArray();
                var i = 0;
                foreach (var baseType in allTypes)
                {
                    if ((baseType.bitmaskValue & newMask) == 0) 
                        continue;
                    types.InsertArrayElementAtIndex(i);
                    types.GetArrayElementAtIndex(i).objectReferenceValue = baseType;
                    i++;
                }
            }
        }
 
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var typeAttribute = attribute as BaseTypeMaskAttribute;

            if (typeAttribute == null)
            {
                return EditorGUI.GetPropertyHeight(property);
            }
            
            if (property.serializedObject.isEditingMultipleObjects)
            {
                return EditorGUI.GetPropertyHeight(property);
            }
        
            return 20;
        }
    }
}
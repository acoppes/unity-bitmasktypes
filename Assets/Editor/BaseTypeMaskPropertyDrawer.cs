using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomPropertyDrawer(typeof(TypeMaskAttribute), true)]
public class BaseTypeMaskPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // var open = EditorGUI.PropertyField(position, property, label, true);

        var typeAttribute = attribute as TypeMaskAttribute;

        if (typeAttribute == null)
        {
            EditorGUI.PropertyField(position, property, label, true);
            return;
        }

        var baseSpecificType = typeAttribute.type;
        var allTypes = AssetDatabase.FindAssets($"t:{baseSpecificType.Name}")
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<BaseTypeAsset>)
            .ToList();
        
        allTypes.Sort((a, b) => a.enumFlagValue.CompareTo(b.enumFlagValue));

        EditorGUI.MaskField(position, "Bit Mask", 1, allTypes.Select(t => t.name).ToArray());

        // var types = property.FindPropertyRelative("types");
        // var value = 0;
        //
        // for (var i = 0; i < types.arraySize; i++)
        // {
        //     var b = types.GetArrayElementAtIndex(i).objectReferenceValue as BaseTypeAsset;
        //     if (b == null)
        //         continue;
        //     value |= b.enumFlagValue;
        // }
        //
        // var r = new Rect(position.x, 10 + (property.CountInProperty() + 1) * 20, position.width  * 0.75f, 20);
        //
        // EditorGUI.LabelField(r,  Convert.ToString(value, 2).PadLeft(sizeof(int) * 8, '0'));
    }
 
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var typeAttribute = attribute as TypeMaskAttribute;

        if (typeAttribute == null)
        {
            return EditorGUI.GetPropertyHeight(property);
        }
        
        return 20;
    }
}
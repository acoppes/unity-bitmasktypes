using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DamageTypeFlag))]
public class BaseTypeMaskPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var open = EditorGUI.PropertyField(position, property, label, true);

        var types = property.FindPropertyRelative("types");
        var value = 0;
        
        for (var i = 0; i < types.arraySize; i++)
        {
            var b = types.GetArrayElementAtIndex(i).objectReferenceValue as BaseTypeAsset;
            if (b == null)
                continue;
            value |= b.enumFlagValue;
        }

        var r = new Rect(position.x, 10 + (property.CountInProperty() + 1) * 20, position.width  * 0.75f, 20);

        // if (open)
        // {
        //     r.y += property.CountInProperty() * 20;
        // }
        
        EditorGUI.LabelField(r,  Convert.ToString(value, 2).PadLeft(sizeof(int) * 8, '0'));
    }
 
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property) + 20;
    }
}
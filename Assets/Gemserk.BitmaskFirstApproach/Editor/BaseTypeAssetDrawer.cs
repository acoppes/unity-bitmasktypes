using System;
using UnityEditor;
using UnityEngine;

namespace Gemserk.BitmaskTypes.Editor
{
    [CustomEditor(typeof(BaseTypeAsset), true)]
    public class BaseTypeCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var typeAsset = target as BaseTypeAsset;
        
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script:", MonoScript.FromScriptableObject(typeAsset), 
                typeAsset.GetType(), false);
        
            var binary = Convert.ToString(typeAsset.bitmaskValue, 2).PadLeft(sizeof(int) * 8, '0');

            EditorGUILayout.TextField("Flag" , binary);
        
            GUI.enabled = true;
        }
    }
}
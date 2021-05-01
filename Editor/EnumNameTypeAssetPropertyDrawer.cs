using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Gemserk.BitmaskTypes.Editor
{
    [CustomEditor(typeof(EnumNameTypeAsset))]
    public class EnumNameTypeAssetCustomEditor : UnityEditor.Editor
    {
        private bool groupsOpen = false;
        
        public override void OnInspectorGUI()
        {
            // DrawDefaultInspector();
    
            var enumNameType = target as EnumNameTypeAsset;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("types"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("generateCode"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("className"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("namespaceName"));
    
            // var types = enumNameType.groupTypes;

            var displayOptions = new List<string>
            {
                "Everything"
            };
            displayOptions.AddRange(enumNameType.types.Select(t => t.name));

            var displayOptionsArray = displayOptions.ToArray();

            groupsOpen = EditorGUILayout.Foldout(groupsOpen, "Groups");
            if (groupsOpen)
            {
                EditorGUI.indentLevel++;
                var groupTypes = serializedObject.FindProperty("groupTypes");
                
                for (var i = 0; i < groupTypes.arraySize; i++)
                {
                    var g = groupTypes.GetArrayElementAtIndex(i);
                    var name = g.FindPropertyRelative("name");
                    var valueProperty = g.FindPropertyRelative("value");
                    var currentValue = valueProperty.intValue;

                    EditorGUILayout.BeginHorizontal();
                    name.stringValue = EditorGUILayout.TextField(name.stringValue);
                    currentValue = EditorGUILayout.MaskField(currentValue, displayOptionsArray);
                    valueProperty.intValue = currentValue;
                    if (GUILayout.Button("-"))
                    {
                        groupTypes.DeleteArrayElementAtIndex(i);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                
                if (GUILayout.Button("+"))
                {
                    groupTypes.InsertArrayElementAtIndex(groupTypes.arraySize);
                }

                EditorGUI.indentLevel--;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
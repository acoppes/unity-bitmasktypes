using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gemserk.BitmaskTypes.Editor
{
    [CustomEditor(typeof(EnumNameTypeAsset))]
    public class EnumNameTypeAssetCustomEditor : UnityEditor.Editor
    {
        private bool groupsOpen = true;
        
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("types"));

            var generateCodeProperty = serializedObject.FindProperty("generateCode");
            EditorGUILayout.PropertyField(generateCodeProperty);
            
            if (generateCodeProperty.boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("className"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("namespaceName"));
                
                if (GUILayout.Button("Generate"))
                {
                    EnumNameTypeAssetCodeGeneration.RegenerateAllEnumTypeCode();
                }
                
                var enumNameType = target as EnumNameTypeAsset;
                
                var displayOptions = new List<string>
                {
                    // "Everything"
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
                        
                        EditorGUILayout.LabelField(enumNameType.MaskToString(valueProperty.intValue));
                    }
                
                    if (GUILayout.Button("+"))
                    {
                        groupTypes.InsertArrayElementAtIndex(groupTypes.arraySize);
                    }

                    EditorGUI.indentLevel--;
                }
            }
    
           
            serializedObject.ApplyModifiedProperties();
        }
    }
}
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Gemserk.BitmaskTypes.Editor
{
    [CustomEditor(typeof(TypeSetAsset))]
    public class TypeSetAssetCustomEditor : UnityEditor.Editor
    {
        private static void OpenScriptToEdit(string scriptName)
        {
            var files = AssetDatabase.FindAssets($"t:Script {scriptName}");
                
            if (files.Length >0)
            {
                var path = AssetDatabase.GUIDToAssetPath(files[0]);
                AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath(path, typeof(Object)));
            }
        }
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var categoryAsset = target as TypeSetAsset;
            
            if (categoryAsset.generateCode)
            {
                if (GUILayout.Button("Generate"))
                {
                    if (categoryAsset.autoAssignedValues)
                    {
                        AssignValuesToTypes(categoryAsset);
                    }
                    
                    var folder = categoryAsset.outputFolder;
                    
                    if (string.IsNullOrEmpty(folder))
                    {
                        var path = AssetDatabase.GetAssetPath(categoryAsset);
                        folder = Path.GetDirectoryName(path);
                    }
                    else
                    {
                        folder = Path.Combine(Application.dataPath, categoryAsset.outputFolder);
                    }
                    
                    EnumNameTypeAssetCodeGeneration.GenerateTypeSetClass(categoryAsset, folder);
                }
                
                if (GUILayout.Button("Open Code"))
                {
                    OpenScriptToEdit(categoryAsset.className);
                }
            }
            
            if (GUILayout.Button("Assign Values to Types"))
            {
                AssignValuesToTypes(categoryAsset);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private static void AssignValuesToTypes(TypeSetAsset categoryAsset)
        {
            var types = categoryAsset.types;

            for (var i = 0; i < types.Count; i++)
            {
                var type = types[i];
                if (type is IntTypeAsset intType)
                {
                    intType.value = categoryAsset.isBitmask ? 1 << i : i;
                    EditorUtility.SetDirty(type);
                    AssetDatabase.SaveAssetIfDirty(type);
                }
            }
        }
    }
}
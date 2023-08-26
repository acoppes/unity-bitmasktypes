using System.IO;
using UnityEditor;
using UnityEngine;

namespace Gemserk.BitmaskTypes.Editor
{
    [CustomEditor(typeof(TypeSetAsset))]
    public class TypeSetAssetCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var categoryAsset = target as TypeSetAsset;
            
            if (categoryAsset.generateCode)
            {
                if (GUILayout.Button("Generate"))
                {
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
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
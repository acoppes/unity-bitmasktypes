using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Gemserk.BitmaskTypes.Editor
{
    public class FlagTypeAssetImporter : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            var typesToReload = new List<Type>();
        
            foreach (var importedAsset in importedAssets)
            {
                var asset = AssetDatabase.LoadAssetAtPath<BaseTypeAsset>(importedAsset);
                if (asset == null)
                    continue;

                if (typesToReload.Contains(asset.GetType()))
                    continue;

                typesToReload.Add(asset.GetType());
            }

            foreach (var typeToReload in typesToReload)
            {
                var guids = AssetDatabase.FindAssets($"t:{typeToReload}");
            
                var baseTypes = guids.Select(AssetDatabase.GUIDToAssetPath)
                    .Select(AssetDatabase.LoadAssetAtPath<BaseTypeAsset>).ToList();

                for (var i = 0; i < baseTypes.Count; i++)
                {
                    var baseType = baseTypes[i];
                    baseType.enumFlagValue = 1 << i;
                    EditorUtility.SetDirty(baseType);
                }
            }
        }
    }
}

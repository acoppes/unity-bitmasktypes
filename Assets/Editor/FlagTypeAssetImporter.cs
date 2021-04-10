using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class FlagTypeAssetImporter : AssetPostprocessor
{
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        // var typeFolders = new List<string>();
        //
        // foreach (var importedAsset in importedAssets)
        // {
        //     var asset = AssetDatabase.LoadAssetAtPath<BaseTypeAsset>(importedAsset);
        //     if (asset == null)
        //         continue;
        //     var typeFolder = Path.GetDirectoryName(importedAsset);
        //
        //     if (typeFolders.Contains(typeFolder))
        //         continue;
        //
        //     Debug.Log(asset.GetType().Name);
        //     
        //     typeFolders.Add(Path.GetDirectoryName(importedAsset));
        // }
        //
        // foreach (var typeFolder in typeFolders)
        // {
        //     var guids = AssetDatabase.FindAssets("t:BaseTypeAsset", new []
        //     {
        //         typeFolder
        //     });
        //     
        //     var baseTypes = guids.Select(AssetDatabase.GUIDToAssetPath)
        //         .Select(AssetDatabase.LoadAssetAtPath<BaseTypeAsset>).ToList();
        //
        //     for (var i = 0; i < baseTypes.Count; i++)
        //     {
        //         var baseType = baseTypes[i];
        //         baseType.enumFlagValue = 1 << i;
        //         EditorUtility.SetDirty(baseType);
        //     }
        // }
        
        var typesToReload = new List<Type>();
        
        foreach (var importedAsset in importedAssets)
        {
            var asset = AssetDatabase.LoadAssetAtPath<BaseTypeAsset>(importedAsset);
            if (asset == null)
                continue;
            
            // var typeFolder = Path.GetDirectoryName(importedAsset);

            if (typesToReload.Contains(asset.GetType()))
                continue;

            // Debug.Log(asset.GetType().Name);
            
            typesToReload.Add(asset.GetType());
            
            // typeFolders.Add(Path.GetDirectoryName(importedAsset));
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

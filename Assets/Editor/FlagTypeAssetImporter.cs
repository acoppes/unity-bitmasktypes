using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;

public class FlagTypeAssetImporter : AssetPostprocessor
{
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        var typeFolders = new List<string>();
        
        foreach (var importedAsset in importedAssets)
        {
            var asset = AssetDatabase.LoadAssetAtPath<BaseTypeAsset>(importedAsset);
            if (asset == null)
                continue;
            var typeFolder = Path.GetDirectoryName(importedAsset);

            if (typeFolders.Contains(typeFolder))
                continue;
            
            typeFolders.Add(Path.GetDirectoryName(importedAsset));
        }

        foreach (var typeFolder in typeFolders)
        {
            var guids = AssetDatabase.FindAssets("t:BaseTypeAsset", new []
            {
                typeFolder
            });
            
            var baseTypes = guids.Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<DamageTypeAsset>).ToList();

            for (var i = 0; i < baseTypes.Count; i++)
            {
                var damageType = baseTypes[i];
                damageType.enumFlagValue = 1 << i;
                EditorUtility.SetDirty(damageType);
            }
        }
    }
}

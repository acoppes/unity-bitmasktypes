using System.Linq;
using UnityEditor;

public class FlagTypeAssetImporter : AssetPostprocessor
{
    private static bool reimporting;

    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        if (reimporting)
            return;
        
        var regenerateDamageTypes = false;
        
        foreach (var importedAsset in importedAssets)
        {
            var asset = AssetDatabase.LoadAssetAtPath<DamageTypeAsset>(importedAsset);
            if (asset == null)
                continue;
            regenerateDamageTypes = true;
        }

        if (regenerateDamageTypes)
        {
            reimporting = true;
            
            var damageTypeGuids = AssetDatabase.FindAssets("t:DamageTypeAsset");
            var damageTypes = damageTypeGuids.Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<DamageTypeAsset>).ToList();

            for (var i = 0; i < damageTypes.Count; i++)
            {
                var damageType = damageTypes[i];
                damageType.enumFlagValue = 1 << i;
                EditorUtility.SetDirty(damageType);
            }
            
            // AssetDatabase.SaveAssets();

            reimporting = false;
        }
    }
}

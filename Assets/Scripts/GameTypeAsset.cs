using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName="Gemserk/GameType")]
public class GameTypeAsset : ScriptableObject
{
    [HideInInspector]
    [SerializeField]
    public List<TypeAsset> typeAssets = new List<TypeAsset>();
}

#if UNITY_EDITOR
[CustomEditor(typeof(GameTypeAsset))]
public class GameTypeEditor : Editor
{
    // TODO: on import asset, regenerate type values maybe?
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var gameTypeAsset = target as GameTypeAsset;
        
//        if (GUILayout.Button("Reassign"))
//        {
//            var types = AssetDatabase
//                .LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(gameTypeAsset))
//                .OfType<TypeAsset>().ToList();
//            
//            for (var i = 0; i < types.Count; i++)
//            {
//                types[i].value = 1 << i;
//                EditorUtility.SetDirty(types[i]);
//            }
//            
//            AssetDatabase.SaveAssets();
//        }
        
        if (GUILayout.Button("Create"))
        {
            var typeAsset = ScriptableObject.CreateInstance<TypeAsset>();
            typeAsset.name = "Type";
            typeAsset.value = 1 << gameTypeAsset.typeAssets.Count; 
            AssetDatabase.AddObjectToAsset(typeAsset, gameTypeAsset);
            AssetDatabase.SaveAssets();
            gameTypeAsset.typeAssets.Add(typeAsset);
            Selection.activeObject = typeAsset;
        }
    }
}
#endif
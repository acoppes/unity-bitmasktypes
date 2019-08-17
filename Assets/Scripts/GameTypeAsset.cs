using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName="Iron Marines/GameType")]
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
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var gameTypeAsset = target as GameTypeAsset;
        if (GUILayout.Button("Create"))
        {
            var typeAsset = ScriptableObject.CreateInstance<TypeAsset>();
            typeAsset.name = "Type";
            typeAsset.value = 1 << gameTypeAsset.typeAssets.Count; 
            AssetDatabase.AddObjectToAsset(typeAsset, gameTypeAsset);
            AssetDatabase.SaveAssets();
            gameTypeAsset.typeAssets.Add(typeAsset);
        }
    }
}
#endif
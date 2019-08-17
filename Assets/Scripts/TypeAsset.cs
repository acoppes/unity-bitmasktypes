using UnityEngine;

#if UNITY_EDITOR
using System;
using UnityEditor;
#endif

public class TypeAsset : ScriptableObject
{
    [HideInInspector]
    [SerializeField]
    public int value;

    public T ToEnum<T>() where T : Enum
    {
        return (T) Enum.ToObject(typeof(T), value);
    }
}
    
#if UNITY_EDITOR
[CustomEditor(typeof(TypeAsset))]
public class TypeAssetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var typeAsset = target as TypeAsset;

        EditorGUI.BeginChangeCheck();
        typeAsset.name = EditorGUILayout.TextField("Name", typeAsset.name);
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(typeAsset);
//                AssetDatabase.SaveAssets();
        }
        
        // EditorGUILayout.LabelField($"Flag: {Convert.ToString(typeAsset.value, 2).PadLeft(16, '0')}");
        // button to REMOVE
        if (GUILayout.Button("Remove"))
        {
//                DestroyImmediate(typeAsset, true);
            AssetDatabase.RemoveObjectFromAsset(typeAsset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        if (Event.current.isKey && Event.current.keyCode == KeyCode.Return)
        {
            AssetDatabase.SaveAssets();
        }
    }
}
#endif
using System;
using System.Linq;
using UnityEngine;

// internal for the game maybe?
[Flags]
public enum DamageTypeEnum
{
    DamageType1 = 1 << 0,
    DamageType2 = 1 << 1
}

[Serializable]
public class DamageTypeFlag
{
    // TODO: property drawer to show it nicer maybe?
    public DamageTypeAsset[] types;

    [NonSerialized]
    private bool cached;
    
    [NonSerialized]
    private int cachedValue = 0;

    public int GetEnumValue()
    {
        if (cached) 
            return cachedValue;

        for (var i = 0; i < types.Length; i++)
        {
            var typeAsset = types[i];
            cachedValue |= typeAsset.enumFlagValue;
        }

        cached = true;
        
        return cachedValue;
    }

    public bool HasFlags(int enumValue)
    {
        return (GetEnumValue() & enumValue) != 0;
    }
    
    public T ToEnum<T>() where T : Enum
    {
        return (T) Enum.ToObject(typeof(T), GetEnumValue());
    }

    public override string ToString()
    {
        return string.Join("|", types.Select(t => t.name));
    }
}

[CreateAssetMenu(menuName="Gemserk/Damage Type")]
public class DamageTypeAsset : BaseTypeAsset
{
    
}
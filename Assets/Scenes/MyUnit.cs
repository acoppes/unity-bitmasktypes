using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class DamageTypeFlag
{
    // TODO: property drawer to show it nicer maybe?
    public DamageTypeAsset[] types;

    public int GetEnumValue()
    {
        var value = 0;
        var values = types.Select(t => t.enumFlagValue).ToList();
        values.ForEach(i => value |= i);
        return value;
    }

    public bool HasFlags(int enumValue)
    {
        return (GetEnumValue() & enumValue) != 0;
    }
    
//    public T ToEnum<T>() where T : Enum
//    {
//        return (T) Enum.ToObject(typeof(T), value);
//    }
}

public class MyUnit : MonoBehaviour
{
    // TODO: a way to filter selection (maybe using a property)

    public DamageTypeFlag damageTypes;

    public DamageTypeFlag acceptDamageTypes;

    public int health;

    public KeyCode attackKey;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(attackKey))
        {
            var units = FindObjectsOfType<MyUnit>().Except(new []{this}).ToList();
            units.ForEach(u =>
            {
                if (u.acceptDamageTypes.HasFlags(damageTypes.GetEnumValue()))
                {
                    u.health--;
                }
            });
            
        }
    }
}

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class MyUnit : MonoBehaviour
{
    // TODO: a way to filter selection (maybe using a property)
 
    [TypeMask(typeof(DamageTypeAsset))]
    public DamageTypeMask damageTypeMask;

    [TypeMask(typeof(DamageTypeAsset))]
    public DamageTypeMask acceptDamageTypeMask;

    public int health;

    public KeyCode attackKey;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(attackKey))
        {
            var units = FindObjectsOfType<MyUnit>().Except(new []{this}).ToList();
            units.ForEach(u =>
            {
                if (u.acceptDamageTypeMask.HasFlags(damageTypeMask.GetEnumValue()))
                {
                    u.health--;
                }
            });
            
            Debug.Log($"{damageTypeMask.GetEnumValue()}");
            Debug.Log($"{damageTypeMask}");
        }
    }
}

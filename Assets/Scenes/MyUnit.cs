using System;
using System.Linq;
using UnityEngine;

public class MyUnit : MonoBehaviour
{
    // TODO: a way to filter selection (maybe using a property)

    public DamageTypeFlag damageTypes;

    public DamageTypeFlag acceptDamageTypes;

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
                if (u.acceptDamageTypes.HasFlags(damageTypes.GetEnumValue()))
                {
                    u.health--;
                }
            });
            
            Debug.Log($"{damageTypes.ToEnum<DamageTypeEnum>()}");
            Debug.Log($"{damageTypes}");
        }
    }
}

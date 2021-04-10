using System.Linq;
using Gemserk.BitmaskTypes;
using UnityEngine;

public class MyUnit : MonoBehaviour
{
    [TypeMask(typeof(DamageTypeAsset))]
    public BaseTypeMask damageTypeMask;

    [TypeMask(typeof(DamageTypeAsset))]
    public BaseTypeMask acceptDamageTypeMask;

    public int health;

    public KeyCode attackKey;
    
    private void Update()
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
            
            // Debug.Log($"{damageTypeMask.GetEnumValue()}");
            Debug.Log($"{damageTypeMask}");
        }
    }
}

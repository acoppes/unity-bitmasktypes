using System.Linq;
using Gemserk.BitmaskTypes;
using UnityEngine;

namespace Gemserk.Examples
{
    public class Unit : MonoBehaviour
    {
        [BaseTypeMask(typeof(DamageTypeAsset))]
        public BaseTypeMask damageTypeMask;

        [BaseTypeMask(typeof(DamageTypeAsset))]
        public BaseTypeMask acceptDamageTypeMask;

        [BaseTypeMask(typeof(MovementTypeAsset))]
        public BaseTypeMask movementType;
    
        public int health;

        public KeyCode attackKey;
    
        private void Update()
        {
            if (Input.GetKeyUp(attackKey))
            {
                var units = FindObjectsOfType<Unit>().Except(new []{this}).ToList();
                units.ForEach(u =>
                {
                    if (u.acceptDamageTypeMask.HasFlags(damageTypeMask.GetEnumValue()))
                    {
                        u.health--;
                    }
                });
            
                // Debug.Log($"{damageTypeMask.GetEnumValue()}");
                Debug.Log($"{damageTypeMask}");

                if (damageTypeMask.HasFlags((int) DamageTypeAssetEnum.ElectricDamage))
                {
                    Debug.Log("Testing has damage from code enum");
                }
            }
        }
    }
}

using Gemserk.BitmaskTypes;
using UnityEngine;

public class OneDamage : MonoBehaviour
{
    // public DamageTypeMask damageTypes;

    [TypeMask(typeof(MovementTypeAsset))]
    public BaseTypeMask movement;
}
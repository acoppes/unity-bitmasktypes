using UnityEngine;

namespace Gemserk.BitmaskTypes
{
    [CreateAssetMenu(menuName = "Gemserk/Generic Type")]
    public class GenericTypeAsset : ScriptableObject
    {
        [BitMask(32)]
        public int type;
    }
}
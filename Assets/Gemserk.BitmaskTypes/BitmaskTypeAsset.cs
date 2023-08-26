using UnityEngine;

namespace Gemserk.BitmaskTypes
{
    [CreateAssetMenu(menuName = "Gemserk/Types/Bitmask Type")]
    public class BitmaskTypeAsset : BaseIntTypeAsset
    {
        [BitMask(32)]
        public int type;
        
        public override bool Match(int value)
        {
            return type == value;
        }

        public override int GetBitmask()
        {
            return type;
        }
    }
}
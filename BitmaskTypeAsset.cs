using System;
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

        public override string GetCodeRepresentation()
        { 
            var shiftValue = (int) Math.Round(Math.Log(GetBitmask(), 2));
            return $"1 << {shiftValue}";
        }
    }
}
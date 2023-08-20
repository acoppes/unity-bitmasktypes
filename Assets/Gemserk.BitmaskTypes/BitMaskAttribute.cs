using UnityEngine;

namespace Gemserk.BitmaskTypes
{
    public class BitMaskAttribute : PropertyAttribute
    {
        public ushort bits;

        public BitMaskAttribute(ushort bits = 8)
        {
            this.bits = bits;
        }
    }
}
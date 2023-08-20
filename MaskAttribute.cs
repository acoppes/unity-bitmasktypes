using UnityEngine;

namespace Gemserk.BitmaskTypes
{
    public class MaskAttribute : PropertyAttribute
    {
        public ushort bytes;

        public MaskAttribute(ushort bytes = 1)
        {
            this.bytes = bytes;
        }
    }
}
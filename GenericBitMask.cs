using System;
using UnityEngine;

namespace Gemserk.BitmaskTypes
{
    [Serializable]
    public class GenericBitMask
    {
        [SerializeField]
        private BitmaskTypeAsset[] types;

        private bool calculated;
        private int mask;

        public int Mask
        {
            get
            {
                if (!calculated)
                {
                    mask = 0;
                    foreach (var type in types)
                    {
                        mask |= type.type;
                    }
                    calculated = true;
                }
                return mask;
            }
        }

        public bool Match(int type)
        {
            return BitMaskCheck.Match(mask, type);
        }
    }
}
using System;
using Gemserk.BitmaskTypes;
using UnityEngine;

namespace Gemserk.Examples
{
    [Flags]
    public enum CustomType
    {
        Nothing = 0,
        Everything = -1,
        Type0 = 1 << 0,
        Type1 = 1 << 1,
        Type2 = 1 << 2,
        Type3 = 1 << 3,
        Type4 = 1 << 4,
        Type5 = 1 << 5,
        Type6 = 1 << 6,
        Type7 = 1 << 7,
    }
    
    public class UnitUsingEnums : MonoBehaviour
    {
        [EnumName("damages")]
        public CustomType type1;
        
        public CustomType type2;

        [ContextMenu("Override with type2")]
        private void OverrideWithType2()
        {
            type1 = type2;
        }
        
        [ContextMenu("Override with type1")]
        private void OverrideWithType1()
        {
            type2 = type1;
        }
    }
}

using Gemserk.BitmaskTypes;
using Gemserk.ExamplesCore;
using UnityEngine;

namespace Gemserk.Examples
{
    public class UnitUsingEnums : MonoBehaviour
    {
        [EnumName("damages")]
        public ArmorType type1;
        
        public ArmorType type2;

        [EnumName("armors")]
        public int type3;

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

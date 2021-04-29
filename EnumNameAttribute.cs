using UnityEngine;

namespace Gemserk.BitmaskTypes
{
    public class EnumNameAttribute : PropertyAttribute
    {
        public string namesAsset;

        public EnumNameAttribute(string namesAsset)
        {
            this.namesAsset = namesAsset;
        }
    }
}
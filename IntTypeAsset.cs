using UnityEngine;

namespace Gemserk.BitmaskTypes
{
    [CreateAssetMenu(menuName = "Gemserk/Types/Int")]
    public class IntTypeAsset : BaseIntTypeAsset
    {
        public int value;
        public override bool Match(int value)
        {
            return this.value == value;
        }

        public override int GetBitmask()
        {
            return 1 << value;
        }
    }
}
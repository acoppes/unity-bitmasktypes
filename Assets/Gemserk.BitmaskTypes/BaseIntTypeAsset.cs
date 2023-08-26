using UnityEngine;

namespace Gemserk.BitmaskTypes
{
    public abstract class BaseIntTypeAsset : ScriptableObject
    {
        public abstract bool Match(int value);
        public abstract int GetBitmask();
    }
}
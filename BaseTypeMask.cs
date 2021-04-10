using System;
using System.Linq;

namespace Gemserk.BitmaskTypes
{
    [Serializable]
    public class BaseTypeMask
    {
        public BaseTypeAsset[] types;

        [NonSerialized]
        private bool cached;
    
        [NonSerialized]
        private int cachedValue = 0;

        public int GetEnumValue()
        {
            if (cached) 
                return cachedValue;

            for (var i = 0; i < types.Length; i++)
            {
                var typeAsset = types[i];
                // Empty entry in array or missing reference (deleted type maybe)
                if (typeAsset == null)
                    continue;
                cachedValue |= typeAsset.bitmaskValue;
            }

            cached = true;
        
            return cachedValue;
        }

        public bool HasFlags(int enumValue)
        {
            return (GetEnumValue() & enumValue) != 0;
        }
    
        public T ToEnum<T>() where T : Enum
        {
            return (T) Enum.ToObject(typeof(T), GetEnumValue());
        }

        public override string ToString()
        {
            return string.Join("|", types.Select(t => t.name));
        }
    }
}
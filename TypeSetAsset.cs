using System.Collections.Generic;
using UnityEngine;

namespace Gemserk.BitmaskTypes
{
    [CreateAssetMenu(menuName = "Gemserk/Types/Types Set")]
    public class TypeSetAsset : ScriptableObject
    {
        public List<BaseIntTypeAsset> types;
        
        public bool generateCode;
        
        public string className;
        public string namespaceName;
        public string outputFolder;

        public bool partialClass = true;
        public bool appendGeneratedToFileName = true;

        public bool isBitmask;
        
        public string GetTypeName(int value)
        {
            foreach (var type in types)
            {
                if (type == null)
                    continue;

                if (type.Match(value))
                    return type.name;
            }

            return null;
        }
        
        public void GetMaskNames(int mask, ICollection<string> names)
        {
            foreach (var type in types)
            {
                if (type == null)
                    continue;

                var bitmask = type.GetBitmask();
                
                if ((mask & bitmask) == bitmask) 
                    names.Add(type.name);
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace Gemserk.BitmaskTypes
{
    [CreateAssetMenu(menuName = "Gemserk/Generic Type Category")]
    public class GenericTypeCategoryAsset : ScriptableObject
    {
        public List<GenericTypeAsset> types;
        
        public bool generateCode;
        
        public string className;
        public string namespaceName;
        public string outputFolder;

        public bool partialClass = true;
        public bool appendGeneratedToFileName = true;
        
        public string GetTypeName(int value)
        {
            foreach (var type in types)
            {
                if (type == null)
                    continue;

                if (type.type == value)
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

                if ((mask & type.type) == type.type) 
                    names.Add(type.name);
            }
        }
    }
}
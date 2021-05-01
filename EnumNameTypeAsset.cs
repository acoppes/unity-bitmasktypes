using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gemserk.BitmaskTypes
{
    [CreateAssetMenu(menuName = "Enum Name Asset")]
    public class EnumNameTypeAsset : ScriptableObject
    {
        [Serializable]
        public struct TypeName
        {
            public string name;
        }
        
        [Serializable]
        public struct GroupTypeName
        {
            public string name;
            // [EnumName("damages")]
            public int value;
        }        

        public bool generateCode;
        public string className;
        public string namespaceName;
        
        public List<TypeName> types;
        public List<GroupTypeName> groupTypes;

        public string[] GetOrderedNames()
        {
            var listCopy = new List<TypeName>(types);
            return listCopy.Select(n => n.name).ToArray();
        }
        
        public List<TypeName> GetMaskTypes(int value)
        {
            var list = new List<TypeName>();
            
            for (var i = 0; i < types.Count; i++)
            {
                var type = types[i];
                
                if (((1 << i) & value) != 0)
                {
                    list.Add(type);
                }
            }

            return list;
        }
    }
}
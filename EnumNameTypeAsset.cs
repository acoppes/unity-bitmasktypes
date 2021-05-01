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
        
        public IEnumerable<TypeName> GetMaskTypes(int value)
        {
            return types.Where((type, i) => ((1 << i) & value) != 0).ToList();
        }

        public string MaskToString(int value)
        {
            var maskTypes = GetMaskTypes(value);
            var strings = maskTypes.Select(t => t.name).ToArray();
            return string.Join(" | ", strings);
        }
    }
}
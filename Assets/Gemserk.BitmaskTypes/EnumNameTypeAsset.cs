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
    }
}
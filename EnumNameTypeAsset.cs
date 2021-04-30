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

        public List<TypeName> types;

        public bool generateCode;
        public string className;
        public string namespaceName;

        public string[] GetOrderedNames()
        {
            var listCopy = new List<TypeName>(types);
            return listCopy.Select(n => n.name).ToArray();
        }
    }
}
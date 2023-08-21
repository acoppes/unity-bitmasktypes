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

        // public string[] GetOrderedNames()
        // {
        //     var listCopy = new List<TypeName>(types);
        //     return listCopy.Select(n => n.name).ToArray();
        // }
        //
        // public IEnumerable<TypeName> GetMaskTypes(int value)
        // {
        //     return types.Where((type, i) => ((1 << i) & value) != 0).ToList();
        // }
        //
        // public string MaskToString(int value)
        // {
        //     var maskTypes = GetMaskTypes(value);
        //     var strings = maskTypes.Select(t => t.name).ToArray();
        //     return string.Join(" | ", strings);
        // }
    }
}
using System;
using UnityEngine;

namespace Gemserk.BitmaskTypes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class TypeMaskAttribute : PropertyAttribute
    {
        public Type type;

        public TypeMaskAttribute(Type type)
        {
            this.type = type;
        }
    }
}
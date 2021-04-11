using System;
using UnityEngine;

namespace Gemserk.BitmaskTypes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class BaseTypeMaskAttribute : PropertyAttribute
    {
        public Type type;

        public BaseTypeMaskAttribute(Type type)
        {
            this.type = type;
        }
    }
}
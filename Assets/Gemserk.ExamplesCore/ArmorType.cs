using System;

namespace Gemserk.ExamplesCore
{
    [Flags]
    public enum ArmorType
    {
        Nothing = 0,
        Everything = -1,
        Type0 = 1 << 0,
        Type1 = 1 << 1,
        Type2 = 1 << 2,
        Type3 = 1 << 3,
        Type4 = 1 << 4,
        Type5 = 1 << 5,
        Type6 = 1 << 6,
        Type7 = 1 << 7
        
        // Until sizeof(int)
    }
}
namespace Gemserk.BitmaskTypes
{
    public static class BitMaskCheck
    {
        public static bool Match(int mask, int flag)
        {
            return (mask & flag) == flag;
        }        
    }
}
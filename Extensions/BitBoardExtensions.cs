using System;

namespace ChessWasm 
{
    public static class BitBoardExtensions
    {
        public static UInt64 NorthOne(this UInt64 val)
        {
            return val << 8;
        }

        public static UInt64 WestOne(this UInt64 val)
        {
            return val >> 1;
        }

        public static UInt64 SouthOne(this UInt64 val)
        {
            return val >> 8;
        }

        public static UInt64 EastOne(this UInt64 val)
        {
            return val << 1;
        }

        public static bool NthBitSet(this UInt64 value, int n)
        {
            return ((value >> n) & 1) == 1;
        }

        public static UInt64 SetNthBit(this UInt64 value, int n)
        {
            return value | ((UInt64)1 << n);
        }
    }
}
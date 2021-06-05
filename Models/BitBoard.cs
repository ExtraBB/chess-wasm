using System;

namespace ChessWasm.Models
{
    public class BitBoard
    {
        UInt64 WRooks = 0, BRooks = 0;
        UInt64 WKnights = 0, BKnights = 0;
        UInt64 WBishops = 0, BBishops = 0;
        UInt64 WKing = 0, BKing = 0;
        UInt64 WQueen = 0, BQueen = 0;
        UInt64 WPawns = 0, BPawns = 0;

        public const UInt64 AFile = 0x0101010101010101;
        public const UInt64 HFile = 0x8080808080808080;
        public const UInt64 Rank1 = 0x00000000000000FF;
        public const UInt64 Rank8 = 0xFF00000000000000;
        public const UInt64 A1H8Diagonal = 0x8040201008040201;
        public const UInt64 H1A8Diagonal = 0x0102040810204080;
        public const UInt64 LightSquares = 0x55AA55AA55AA55AA;
        public const UInt64 DarkSquares = 0xAA55AA55AA55AA55;
    }
}
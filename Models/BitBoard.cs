using System;

namespace ChessWasm.Models
{
    public class BitBoard
    {
        public UInt64 WRooks = 0x81, BRooks = 0x8100000000000000;
        public UInt64 WKnights = 0x42, BKnights = 0x4200000000000000;
        public UInt64 WBishops = 0x24, BBishops = 0x2400000000000000;
        public UInt64 WKing = 0x10, BKing = 0x1000000000000000;
        public UInt64 WQueen = 0x8, BQueen = 0x800000000000000;
        public UInt64 WPawns = Rank2, BPawns = Rank7;

        public UInt64 WhitePieces { get => WRooks | WKnights | WBishops | WKing | WQueen | WPawns; }
        public UInt64 BlackPieces { get => BRooks | BKnights | BBishops | BKing | BQueen | BPawns; }
        public UInt64 Empty { get => ~(WhitePieces | BlackPieces); }

        /*
        *  Constants
        */

        // Files
        public const UInt64 AFile = 0x0101010101010101;
        public const UInt64 BFile = 0x0202020202020202;
        public const UInt64 CFile = 0x0303030303030303;
        public const UInt64 DFile = 0x0404040404040404;
        public const UInt64 EFile = 0x0505050505050505;
        public const UInt64 FFile = 0x0606060606060606;
        public const UInt64 GFile = 0x0707070707070707;
        public const UInt64 HFile = 0x8080808080808080;

        // Ranks
        public const UInt64 Rank1 = 0x00000000000000FF;
        public const UInt64 Rank2 = 0x000000000000FF00;
        public const UInt64 Rank3 = 0x0000000000FF0000;
        public const UInt64 Rank4 = 0x00000000FF000000;
        public const UInt64 Rank5 = 0x000000FF00000000;
        public const UInt64 Rank6 = 0x0000FF0000000000;
        public const UInt64 Rank7 = 0x00FF000000000000;
        public const UInt64 Rank8 = 0xFF00000000000000;

        // Diagonals
        public const UInt64 A1H8Diagonal = 0x8040201008040201;
        public const UInt64 H1A8Diagonal = 0x0102040810204080;

        // Colored Squares
        public const UInt64 LightSquares = 0x55AA55AA55AA55AA;
        public const UInt64 DarkSquares = 0xAA55AA55AA55AA55;
    }
}
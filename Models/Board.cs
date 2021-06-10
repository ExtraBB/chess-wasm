using System;

namespace ChessWasm.Models
{
    public enum Squares {
        A1, B1, C1, D1, E1, F1, G1, H1,
        A2, B2, C2, D2, E2, F2, G2, H2,
        A3, B3, C3, D3, E3, F3, G3, H3,
        A4, B4, C4, D4, E4, F4, G4, H4,
        A5, B5, C5, D5, E5, F5, G5, H5,
        A6, B6, C6, D6, E6, F6, G6, H6,
        A7, B7, C7, D7, E7, F7, G7, H7,
        A8, B8, C8, D8, E8, F8, G8, H8,
    }

    public class Board
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
        public UInt64 Occupied { get => WhitePieces | BlackPieces; }

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

        public void MakeMove(Move move) 
        {

            // Clear to square
            WRooks = WRooks.UnsetBit(move.To);
            BRooks = BRooks.UnsetBit(move.To);
            WKnights = WKnights.UnsetBit(move.To);
            BKnights = BKnights.UnsetBit(move.To);
            WBishops = WBishops.UnsetBit(move.To);
            BBishops = BBishops.UnsetBit(move.To);
            WQueen = WQueen.UnsetBit(move.To);
            BQueen = BQueen.UnsetBit(move.To);
            WKing = WKing.UnsetBit(move.To);
            BKing = BKing.UnsetBit(move.To);
            WPawns = WPawns.UnsetBit(move.To);
            BPawns = BPawns.UnsetBit(move.To);

            // Make move for correct bitboard
            UInt64 from = 1UL << move.From;
            UInt64 to = 1UL << move.To;
            switch(move.Piece)
            {
                case Piece.WRook: WRooks = (WRooks ^ from) | to; return;
                case Piece.BRook: BRooks = (BRooks ^ from) | to; return;
                case Piece.WKnight: WKnights = (WKnights ^ from) | to; return;
                case Piece.BKnight: BKnights = (BKnights ^ from) | to; return;
                case Piece.WBishop: WBishops = (WBishops ^ from) | to; return;
                case Piece.BBishop: BBishops = (BBishops ^ from) | to; return;
                case Piece.WQueen: WQueen = (WQueen ^ from) | to; return;
                case Piece.BQueen: BQueen = (BQueen ^ from) | to; return;
                case Piece.WKing: WKing = (WKing ^ from) | to; return;
                case Piece.BKing: BKing = (BKing ^ from) | to; return;
                case Piece.WPawn: WPawns = (WPawns ^ from) | to; return;
                case Piece.BPawn: BPawns = (BPawns ^ from) | to; return;
            }
        }
    }
}
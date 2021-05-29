using System;

namespace ChessWasm.Models
{
    [Flags]
    public enum Piece
    {
        None = 0b_0000_0001,
        Pawn = 0b_0000_0010,
        Knight = 0b_0000_0100,
        Bishop = 0b_0000_1000,
        Rook = 0b_0001_0000,
        Queen = 0b_0010_0000,
        King = 0b_0100_0000,
        
        White = 0b_1000_0000,
    }
}
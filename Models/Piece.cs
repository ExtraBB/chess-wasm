using System;

namespace ChessWasm.Models
{
    [Flags]
    public enum Piece : byte
    {
        White = 0b_0000_0001,
        Black = 0b_0000_0010,
        Pawn = 0b_0000_0100,
        Knight = 0b_0000_1000,
        Bishop = 0b_0001_0000,
        Rook = 0b_0010_0000,
        Queen = 0b_0100_0000,
        King = 0b_1000_0000,

    }

    public static class PieceExtensions
    {
        public static bool CapturesDiagonal(this Piece piece) 
        {
            return piece.HasFlag(Piece.Pawn) || piece.HasFlag(Piece.Bishop) || piece.HasFlag(Piece.Queen) || piece.HasFlag(Piece.King) || piece.HasFlag(Piece.Pawn);
        }

        public static bool CapturesStraight(this Piece piece) 
        {
            return piece.HasFlag(Piece.Queen) || piece.HasFlag(Piece.King) || piece.HasFlag(Piece.Rook);
        }

        public static bool CanMoveUnlimitedSquares(this Piece piece) 
        {
            return piece.HasFlag(Piece.Queen) || piece.HasFlag(Piece.Bishop) || piece.HasFlag(Piece.Rook);
        }

        public static bool CanMoveOneSquare(this Piece piece) 
        {
            return piece.HasFlag(Piece.Pawn) || piece.HasFlag(Piece.King);
        }
    }
}
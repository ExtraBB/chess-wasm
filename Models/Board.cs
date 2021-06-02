using System;

namespace ChessWasm.Models
{
    public class BoardChangedEventArgs 
    {
        public int LastPlayer { get; set; }
    }

    public class Board 
    {
        /*
            56, 57, 58, 59, 60, 61, 62, 63
            48, 49, 50, 51, 52, 53, 54, 55
            40, 41, 42, 43, 44, 45, 46, 47
            32, 33, 34, 35, 36, 37, 38, 39
            24, 25, 26, 27, 28, 29, 30, 31
            16, 17, 18, 19, 20, 21, 22, 23
            8,  9, 10, 11, 12, 13, 14, 15
            0,  1,  2,  3,  4,  5,  6,  7
        */
        public Piece[] Squares { get; private set; }

        public Move LastMove { get; set; }

        public Board() 
        {
            Reset();
        }

        public void Reset() 
        {
            Squares = new Piece[]
            {
                Piece.Rook | Piece.Black, Piece.Knight | Piece.Black, Piece.Bishop | Piece.Black, Piece.Queen | Piece.Black, Piece.King | Piece.Black, Piece.Bishop | Piece.Black, Piece.Knight | Piece.Black, Piece.Rook | Piece.Black,
                Piece.Pawn | Piece.Black, Piece.Pawn | Piece.Black, Piece.Pawn | Piece.Black, Piece.Pawn | Piece.Black, Piece.Pawn | Piece.Black, Piece.Pawn | Piece.Black, Piece.Pawn | Piece.Black, Piece.Pawn | Piece.Black, 
                0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 
                Piece.Pawn | Piece.White, Piece.Pawn | Piece.White, Piece.Pawn | Piece.White, Piece.Pawn | Piece.White, Piece.Pawn | Piece.White, Piece.Pawn | Piece.White, Piece.Pawn | Piece.White, Piece.Pawn | Piece.White, 
                Piece.Rook | Piece.White, Piece.Knight | Piece.White, Piece.Bishop | Piece.White, Piece.Queen | Piece.White, Piece.King | Piece.White, Piece.Bishop | Piece.White, Piece.Knight | Piece.White, Piece.Rook | Piece.White
            };
        }

        public Piece MakeMove(Move move)
        {
            Squares[move.To] = Squares[move.From];
            Squares[move.From] = 0;
            LastMove = move;
            return move.Player == Piece.White ? Piece.Black : Piece.White;
        }
    }
}
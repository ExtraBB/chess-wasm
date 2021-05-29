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

        public delegate void BoardChangedEventHandler(object sender, BoardChangedEventArgs e);
        public event BoardChangedEventHandler BoardChanged;

        public Board() 
        {
            Reset();
        }

        public void Reset() 
        {
            Squares = new Piece[]
            {
                Piece.Rook, Piece.Knight, Piece.Bishop, Piece.Queen, Piece.King, Piece.Bishop, Piece.Knight, Piece.Rook,
                Piece.Pawn, Piece.Pawn, Piece.Pawn, Piece.Pawn, Piece.Pawn, Piece.Pawn, Piece.Pawn, Piece.Pawn, 
                Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, 
                Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, 
                Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, 
                Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, 
                Piece.Pawn | Piece.White, Piece.Pawn | Piece.White, Piece.Pawn | Piece.White, Piece.Pawn | Piece.White, Piece.Pawn | Piece.White, Piece.Pawn | Piece.White, Piece.Pawn | Piece.White, Piece.Pawn | Piece.White, 
                Piece.Rook | Piece.White, Piece.Knight | Piece.White, Piece.Bishop | Piece.White, Piece.Queen | Piece.White, Piece.King | Piece.White, Piece.Bishop | Piece.White, Piece.Knight | Piece.White, Piece.Rook | Piece.White
            };
        }

        public void MakeMove(int from, int to)
        {
            Squares[to] = Squares[from];
            Squares[from] = Piece.None;
            BoardChanged?.Invoke(this, new BoardChangedEventArgs() { LastPlayer = Squares[to].HasFlag(Piece.White) ? 1 : -1 });
        }
    }
}
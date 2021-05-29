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
        public sbyte[] Squares { get; private set; }

        public delegate void BoardChangedEventHandler(object sender, BoardChangedEventArgs e);
        public event BoardChangedEventHandler BoardChanged;

        public Board() 
        {
            Reset();
        }

        public void Reset() 
        {
            Squares = new sbyte[64];

            // Pawns
            for(int i = 8; i < 16; i++)
            {
                Squares[i] = Constants.PIECE_PAWN;
                Squares[i + 40] = -Constants.PIECE_PAWN;
            }

            // Rooks
            Squares[0] = Constants.PIECE_ROOK;
            Squares[7] = Constants.PIECE_ROOK;
            Squares[56] = -Constants.PIECE_ROOK;
            Squares[63] = -Constants.PIECE_ROOK;

            // Knights
            Squares[1] = Constants.PIECE_KNIGHT;
            Squares[6] = Constants.PIECE_KNIGHT;
            Squares[57] = -Constants.PIECE_KNIGHT;
            Squares[62] = -Constants.PIECE_KNIGHT;

            // Bishops
            Squares[2] = Constants.PIECE_BISHOP;
            Squares[5] = Constants.PIECE_BISHOP;
            Squares[58] = -Constants.PIECE_BISHOP;
            Squares[61] = -Constants.PIECE_BISHOP;

            // Queens
            Squares[3] = Constants.PIECE_QUEEN;
            Squares[59] = -Constants.PIECE_QUEEN;

            // Kings
            Squares[4] = Constants.PIECE_KING;
            Squares[60] = -Constants.PIECE_KING;
        }

        public void MakeMove(int from, int to)
        {
            Squares[to] = Squares[from];
            Squares[from] = 0;
            BoardChanged?.Invoke(this, new BoardChangedEventArgs() { LastPlayer = Math.Sign(Squares[to]) });
        }
    }
}
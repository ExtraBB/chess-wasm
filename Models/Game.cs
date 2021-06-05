using System.Collections.Generic;
using System.Linq;
using ChessWasm.Services;

namespace ChessWasm.Models
{
    public class Game
    {
        public Board OldBoard { get; private set; }
        public BitBoard Board { get; private set; }
        public Dictionary<int, IEnumerable<OldMove>> PossibleMoves {get; set; }
        public Piece CurrentPlayer = Piece.White;
        public Piece Winner = 0;

        public Game() 
        {
            OldBoard = new Board();
            Board = new BitBoard();
            RefreshPossibleMoves();
        }

        public void MakeMove(OldMove move) 
        {
            if(MoveService.IsLegalMove(OldBoard, move)) 
            {
                CurrentPlayer = OldBoard.MakeMove(move);
                RefreshPossibleMoves();
                CheckForEnd();
            }
        }

        private void RefreshPossibleMoves()
        {
            PossibleMoves = MoveService.GetAllPossibleMoves(OldBoard, CurrentPlayer);
        }

        public void CheckForEnd() 
        {
            if(!PossibleMoves.Values.SelectMany(moves => moves).Any()) 
            {
                Winner = CurrentPlayer.HasFlag(Piece.White) ? Piece.Black : Piece.White;
            }
        }
    }
}
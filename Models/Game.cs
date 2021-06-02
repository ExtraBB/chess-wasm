using System.Collections.Generic;
using System.Linq;
using ChessWasm.Services;

namespace ChessWasm.Models
{
    public class Game
    {
        public Board Board { get; private set; }
        public Dictionary<int, IEnumerable<Move>> PossibleMoves {get; set; }
        public Piece CurrentPlayer = Piece.White;
        public Piece Winner = 0;

        public Game() 
        {
            Board = new Board();
            RefreshPossibleMoves();
        }

        public void MakeMove(Move move) 
        {
            if(MoveService.IsLegalMove(Board, move)) 
            {
                CurrentPlayer = Board.MakeMove(move);
                RefreshPossibleMoves();
                CheckForEnd();
            }
        }

        private void RefreshPossibleMoves()
        {
            PossibleMoves = MoveService.GetAllPossibleMoves(Board, CurrentPlayer);
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
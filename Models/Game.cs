using System.Collections.Generic;
using System.Linq;
using ChessWasm.Services;

namespace ChessWasm.Models
{
    public class Game
    {
        public Board Board { get; private set; }
        public IEnumerable<Move> PossibleMoves { get; set; } = new List<Move>();
        public Player CurrentPlayer { get; set; } = Player.White;
        public Player Winner { get; set; } = 0;
        public Move LastMove { get; set; } = null;

        public Game() 
        {
            Board = new Board();
            RefreshPossibleMoves();
        }

        public void MakeMove(Move move) 
        {
            if(MoveService.IsLegalMove(Board, move, CurrentPlayer)) 
            {
                Board.MakeMove(move);
                CurrentPlayer = (Player)(-(int)CurrentPlayer);
                LastMove = move;
                RefreshPossibleMoves();
                CheckForEnd();
            }

        }

        private void RefreshPossibleMoves()
        {
            PossibleMoves = MoveService.GetAllPossibleMoves(Board, CurrentPlayer, LastMove);
        }

        public void CheckForEnd() 
        {
            if(!PossibleMoves.Any())
            {
                Winner = (Player)(-(int)CurrentPlayer);
            }
        }
    }
}
using System.Collections.Generic;
using ChessWasm.Services;

namespace ChessWasm.Models
{
    public class Game
    {
        public Board Board { get; private set; }
        public Dictionary<int, IEnumerable<Move>> PossibleMoves {get; set; }

        public Game() 
        {
            Board = new Board();
        }

        public void MakeMove(Move move) 
        {
            if(MoveService.IsLegalMove(Board, move)) 
            {
                Board.MakeMove(move);
            }
        }
    }
}
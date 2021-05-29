using System;
using ChessWasm.Models;

namespace ChessWasm.Services
{
    public static class GameService
    {
        private static Random rand = new Random();
        public static Game CurrentGame { get; private set; }

        public static Game StartGame() 
        {
            CurrentGame = new Game();
            return CurrentGame;
        }

        public static void RandomMove()
        {
            int from = 0, to = 0;
            while(from == to || CurrentGame.Board.Squares[from] == 0)
            {
                from = rand.Next(0, 64);
                to = rand.Next(0, 64);
            }
            CurrentGame.MakeMove(from, to);
        }
    }
}
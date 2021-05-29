using System;
using ChessWasm.Models;

namespace ChessWasm.Services
{
    public class GameService
    {
        public Game CurrentGame { get; private set; }
        public Game StartGame() 
        {
            CurrentGame = new Game();
            return CurrentGame;
        }
    }
}
using System;
using System.Collections.Generic;
using ChessWasm.Models;

namespace ChessWasm.Services
{
    public static class GameService
    {
        
        public static Game CurrentGame { get; private set; }

        public delegate void BoardChangedEventHandler(Board board);
        public static event BoardChangedEventHandler BoardChanged;

        public static Game StartGame() 
        {
            CurrentGame = new Game();
            BoardChanged?.Invoke(CurrentGame.OldBoard);
            return CurrentGame;
        }

        public static void ClearGame() 
        {
            CurrentGame = null;
        }

        public static void TryMakeMove(OldMove move)
        {
            CurrentGame.MakeMove(move);
            BoardChanged?.Invoke(CurrentGame.OldBoard);
        }
    }
}
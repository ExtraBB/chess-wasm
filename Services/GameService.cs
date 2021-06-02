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
            RefreshPossibleMoves();
            BoardChanged?.Invoke(CurrentGame.Board);
            return CurrentGame;
        }

        public static void RandomMove()
        {
            Move move = MoveService.GetRandomMove(CurrentGame.Board);
            CurrentGame.MakeMove(move);
            RefreshPossibleMoves();
            BoardChanged?.Invoke(CurrentGame.Board);
        }

        public static void TryMakeMove(Move move)
        {
            CurrentGame.MakeMove(move);
            RefreshPossibleMoves();
            BoardChanged?.Invoke(CurrentGame.Board);
        }

        public static void RefreshPossibleMoves()
        {
            CurrentGame.PossibleMoves = MoveService.GetAllPossibleMoves(CurrentGame.Board);
        }
    }
}
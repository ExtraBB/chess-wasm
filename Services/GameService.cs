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
            BoardChanged?.Invoke(CurrentGame.Board);
            return CurrentGame;
        }

        public static void RandomMove()
        {
            Move move = MoveService.GetRandomMove(CurrentGame.Board);
            CurrentGame.MakeMove(move.From, move.To);

            BoardChanged?.Invoke(CurrentGame.Board);
        }

        public static void TryMakeMove(int fromX, int fromY, int toX, int toY)
        {
            CurrentGame.MakeMove(fromX * 8 + fromY, toX * 8 + toY);
            BoardChanged?.Invoke(CurrentGame.Board);
        }

        public static Dictionary<int, IEnumerable<Move>> GetAllPossibleMoves()
        {
            return MoveService.GetAllPossibleMoves(CurrentGame.Board);
        }
    }
}
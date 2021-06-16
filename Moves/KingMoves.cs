using ChessWasm.Models;
using ChessWasm.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessWasm.Moves
{
    public static class KingMoves 
    {
        private static UInt64[] KingMovesLookupTable;

        public static IEnumerable<Move> CalculateWKingMoves(Board board)
        {
            if(KingMovesLookupTable == null)
            {
                GenerateKingMoves();
            }
            
            int position = board.WKing.GetLS1BIndex();
            return MoveService.CreateMovesFromBitboard(KingMovesLookupTable[position] & ~board.WhitePieces, position, Piece.WKing);
        }

        public static IEnumerable<Move> CalculateBKingMoves(Board board)
        {
            if(KingMovesLookupTable == null)
            {
                GenerateKingMoves();
            }
            
            int position = board.BKing.GetLS1BIndex();
            return MoveService.CreateMovesFromBitboard(KingMovesLookupTable[position] & ~board.BlackPieces, position, Piece.BKing);
        }

        private static void GenerateKingMoves()
        {
            KingMovesLookupTable = new UInt64[64];
            for(int i = 0; i < 64; i++)
            {
                UInt64 kingPosition = 1UL << i;
                UInt64 kingClippedH = kingPosition & ~Board.HFile; 
                UInt64 kingClippedA = kingPosition & ~Board.AFile; 

                UInt64 nw = kingClippedA << 7; 
                UInt64 n = kingPosition << 8; 
                UInt64 ne = kingClippedH << 9; 
                UInt64 e = kingClippedH << 1; 

                UInt64 se = kingClippedH >> 7; 
                UInt64 s = kingPosition >> 8; 
                UInt64 sw = kingClippedA >> 9; 
                UInt64 w = kingClippedA >> 1; 

                KingMovesLookupTable[i] = nw | n | ne | e | se | s | sw | w; 
            }
        }
    }
}
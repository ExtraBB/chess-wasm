using System;
using System.Collections.Generic;
using System.Linq;
using ChessWasm.Models;
using ChessWasm.Moves;

namespace ChessWasm.Services
{
    public static class MoveService
    {

        public static IEnumerable<Move> CreateMovesFromBitboard(UInt64 bitboard, int position, Piece piece) 
        {
            List<Move> moves = new List<Move>(32);
            for(int i = 0; i < 64; i++)
            {
                if(bitboard.NthBitSet(i))
                {
                    moves.Add(new Move(piece, position, i));
                }
            }
            return moves;
        }

        public static IEnumerable<Move> GetAllPossibleMoves(Board board, Player player, Move lastMove)
        {
            if(player == Player.Black)
            {
                return PawnMoves.CalculateBPawnMoves(board, lastMove)
                    .Concat(SlidingMoves.CalculateBBishopMoves(board))
                    .Concat(SlidingMoves.CalculateBRookMoves(board))
                    .Concat(SlidingMoves.CalculateBQueenMoves(board));
            }
            else if (player == Player.White)
            {
                return PawnMoves.CalculateWPawnMoves(board, lastMove)
                    .Concat(SlidingMoves.CalculateWBishopMoves(board))
                    .Concat(SlidingMoves.CalculateWRookMoves(board))
                    .Concat(SlidingMoves.CalculateWQueenMoves(board));
            }
            return null;
        }

        public static bool IsLegalMove(Board board, Move move) 
        {
            // TODO
            return true;
        }
    }
}
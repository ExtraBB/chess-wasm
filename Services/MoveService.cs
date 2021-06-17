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
                    .Concat(SlidingMoves.CalculateBQueenMoves(board))
                    .Concat(KingMoves.CalculateBKingMoves(board))
                    .Concat(KnightMoves.CalculateBKnightMoves(board))
                    .Where(move => IsLegalMove(board, move, Player.Black));
            }
            else if (player == Player.White)
            {
                return PawnMoves.CalculateWPawnMoves(board, lastMove)
                    .Concat(SlidingMoves.CalculateWBishopMoves(board))
                    .Concat(SlidingMoves.CalculateWRookMoves(board))
                    .Concat(SlidingMoves.CalculateWQueenMoves(board))
                    .Concat(KingMoves.CalculateWKingMoves(board))
                    .Concat(KnightMoves.CalculateWKnightMoves(board))
                    .Where(move => IsLegalMove(board, move, Player.White));
            }
            return null;
        }

        public static bool IsLegalMove(Board board, Move move, Player player) 
        {
            Board copy = board.PreviewMove(move);
            return player == Player.White
                ? !copy.SquareIsInCheck(copy.WKing, player)
                : !copy.SquareIsInCheck(copy.BKing, player);

        }

        public static ulong Perft(Board board, int max_depth, Move lastMove = null, Player player = Player.White, int depth = 0)
        {
            if(depth < max_depth)
            {
                var moves = GetAllPossibleMoves(board, player, null);
                var nextPlayer = player == Player.White ? Player.Black : Player.White;

                ulong total = 0;
                foreach(var nextTotal in moves.Select(m => Perft(board.PreviewMove(m), max_depth, m, nextPlayer, depth + 1)))
                {
                    total += nextTotal;
                }
                return total;
            }

            return 1;
        }
    }
}
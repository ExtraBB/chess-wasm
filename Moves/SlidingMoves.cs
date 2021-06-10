using ChessWasm.Services;
using ChessWasm.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessWasm.Moves
{
    public static class SlidingMoves 
    {
        public static IEnumerable<Move> CalculateWBishopMoves(Board board) 
        {
            IEnumerable<Move> moves = new List<Move>(32);

            for(int i = 0; i < 64; i++)
            {
                if(board.WBishops.NthBitSet(i))
                {
                    UInt64 attacks = MagicService.GetBishopAttacks(board.Occupied, i) & ~board.WhitePieces;
                    moves = moves.Concat(MoveService.CreateMovesFromBitboard(attacks, i, Piece.WBishop));
                }
            }

            return moves;
        }

        public static IEnumerable<Move> CalculateBBishopMoves(Board board) 
        {
            IEnumerable<Move> moves = new List<Move>(32);

            for(int i = 0; i < 64; i++)
            {
                if(board.BBishops.NthBitSet(i))
                {
                    UInt64 attacks = MagicService.GetBishopAttacks(board.Occupied, i) & ~board.BlackPieces;
                    moves = moves.Concat(MoveService.CreateMovesFromBitboard(attacks, i, Piece.BBishop));
                }
            }

            return moves;
        }

        public static IEnumerable<Move> CalculateWRookMoves(Board board) 
        {
            IEnumerable<Move> moves = new List<Move>(32);

            for(int i = 0; i < 64; i++)
            {
                if(board.WRooks.NthBitSet(i))
                {
                    UInt64 attacks = MagicService.GetRookAttacks(board.Occupied, i) & ~board.WhitePieces;
                    moves = moves.Concat(MoveService.CreateMovesFromBitboard(attacks, i, Piece.WRook));
                }
            }

            return moves;
        }

        public static IEnumerable<Move> CalculateBRookMoves(Board board) 
        {
            IEnumerable<Move> moves = new List<Move>(32);

            for(int i = 0; i < 64; i++)
            {
                if(board.BRooks.NthBitSet(i))
                {
                    UInt64 attacks = MagicService.GetRookAttacks(board.Occupied, i) & ~board.BlackPieces;
                    moves = moves.Concat(MoveService.CreateMovesFromBitboard(attacks, i, Piece.BRook));
                }
            }

            return moves;
        }

        public static IEnumerable<Move> CalculateWQueenMoves(Board board) 
        {
            IEnumerable<Move> moves = new List<Move>(32);

            for(int i = 0; i < 64; i++)
            {
                if(board.WQueen.NthBitSet(i))
                {
                    UInt64 bishopAttacks = MagicService.GetBishopAttacks(board.Occupied, i) & ~board.WhitePieces;
                    UInt64 rookAttacks = MagicService.GetRookAttacks(board.Occupied, i) & ~board.WhitePieces;
                    moves = moves.Concat(MoveService.CreateMovesFromBitboard(bishopAttacks | rookAttacks, i, Piece.WQueen));
                    break;
                }
            }

            return moves;
        }

        public static IEnumerable<Move> CalculateBQueenMoves(Board board) 
        {
            IEnumerable<Move> moves = new List<Move>(32);

            for(int i = 0; i < 64; i++)
            {
                if(board.BQueen.NthBitSet(i))
                {
                    UInt64 bishopAttacks = MagicService.GetBishopAttacks(board.Occupied, i) & ~board.BlackPieces;
                    UInt64 rookAttacks = MagicService.GetRookAttacks(board.Occupied, i) & ~board.BlackPieces;
                    moves = moves.Concat(MoveService.CreateMovesFromBitboard(bishopAttacks | rookAttacks, i, Piece.BQueen));
                    break;
                }
            }

            return moves;
        }
    }
}
using ChessWasm.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessWasm.Moves
{
    public static class PawnMoves 
    {
        public static IEnumerable<Move> CalculateWPawnMoves(BitBoard board, Move lastMove)
        {
            return CalculateWPawnPushes(board).Concat(CalculateWPawnCaptures(board, lastMove));
        }

        public static IEnumerable<Move> CalculateBPawnMoves(BitBoard board, Move lastMove)
        {
            return CalculateBPawnPushes(board).Concat(CalculateBPawnCaptures(board, lastMove));
        }

        public static IEnumerable<Move> CalculateWPawnPushes(BitBoard board) 
        {
            List<Move> moves = new List<Move>(32);

            UInt64 singlePush = board.WPawns.NorthOne() & board.Empty;
            UInt64 doublePush = singlePush.NorthOne() & board.Empty & BitBoard.Rank4;

            UInt64 promoted = singlePush & BitBoard.Rank8;
            UInt64 nonPromoted = singlePush & ~(promoted);

            for(uint i = 0; i < 64; i++)
            {
                if(nonPromoted.NthBitSet((int)i))
                {
                    moves.Add(new Move((uint)i - 8, i));
                }

                if(doublePush.NthBitSet((int)i))
                {
                    moves.Add(new Move((uint)i - 16, i));
                }

                if(promoted.NthBitSet((int) i))
                {
                    moves.Add(new Move((uint)i - 8, i, PromotionType.Queen, SpecialMove.Promotion));
                    moves.Add(new Move((uint)i - 8, i, PromotionType.Knight, SpecialMove.Promotion));
                    moves.Add(new Move((uint)i - 8, i, PromotionType.Rook, SpecialMove.Promotion));
                    moves.Add(new Move((uint)i - 8, i, PromotionType.Bishop, SpecialMove.Promotion));
                }
            }

            return moves;
        }

        public static IEnumerable<Move> CalculateBPawnPushes(BitBoard board) 
        {
            List<Move> moves = new List<Move>(32);

            UInt64 singlePush = board.BPawns.SouthOne() & board.Empty;
            UInt64 doublePush = singlePush.SouthOne() & board.Empty & BitBoard.Rank5;

            UInt64 promoted = singlePush & BitBoard.Rank1;
            UInt64 nonPromoted = singlePush & ~(promoted);

            for(uint i = 0; i < 64; i++)
            {
                if(nonPromoted.NthBitSet((int)i))
                {
                    moves.Add(new Move((uint)i + 8, i));
                }

                if(doublePush.NthBitSet((int)i))
                {
                    moves.Add(new Move((uint)i + 16, i));
                }

                if(promoted.NthBitSet((int) i))
                {
                    moves.Add(new Move((uint)i + 8, i, PromotionType.Queen, SpecialMove.Promotion));
                    moves.Add(new Move((uint)i + 8, i, PromotionType.Knight, SpecialMove.Promotion));
                    moves.Add(new Move((uint)i + 8, i, PromotionType.Rook, SpecialMove.Promotion));
                    moves.Add(new Move((uint)i + 8, i, PromotionType.Bishop, SpecialMove.Promotion));
                }
            }

            return moves;
        }

        public static IEnumerable<Move> CalculateWPawnCaptures(BitBoard board, Move lastMove) 
        {
            List<Move> moves = new List<Move>(16);

            UInt64 capturesWest = (board.WPawns << 7) & ~BitBoard.AFile & board.BlackPieces;
            UInt64 capturesEast = (board.WPawns << 9) & ~BitBoard.HFile & board.BlackPieces;

            for(uint i = 0; i < 64; i++)
            {
                if(capturesWest.NthBitSet((int)i))
                {
                    moves.Add(new Move((uint)i - 7, i));
                }

                if(capturesEast.NthBitSet((int)i))
                {
                    moves.Add(new Move((uint)i - 9, i));
                }
            }

            // En passant
            if(lastMove != null && lastMove.From - lastMove.To == 16 && board.BPawns.NthBitSet((int)lastMove.To))
            {
                UInt64 possibleCapturerWest = ((UInt64)1 << ((int)lastMove.To - 1)) & board.WPawns & ~BitBoard.AFile;
                UInt64 possibleCapturerEast = ((UInt64)1 << ((int)lastMove.To + 1)) & board.WPawns & ~BitBoard.HFile;
               
               if(possibleCapturerWest != 0)
               {
                    UInt64 destination = possibleCapturerWest << 9;
                    if((board.Empty & destination) == destination)
                    {
                        moves.Add(new Move((uint)lastMove.To - 1, (uint)lastMove.To + 9, specialMove: SpecialMove.EnPassant));
                    }
               }

               if(possibleCapturerEast != 0)
               {
                    UInt64 destination = possibleCapturerEast << 7;
                    if((board.Empty & destination) == destination)
                    {
                        moves.Add(new Move((uint)lastMove.To + 1, (uint)lastMove.To + 7, specialMove: SpecialMove.EnPassant));
                    }
               }
            }

            return moves;
        }

        public static IEnumerable<Move> CalculateBPawnCaptures(BitBoard board, Move lastMove) 
        {
            List<Move> moves = new List<Move>(16);

            UInt64 capturesWest = (board.BPawns >> 9) & ~BitBoard.AFile & board.WhitePieces;
            UInt64 capturesEast = (board.BPawns >> 7) & ~BitBoard.FFile & board.WhitePieces;

            for(uint i = 0; i < 64; i++)
            {
                if(capturesWest.NthBitSet((int)i))
                {
                    moves.Add(new Move((uint)i + 9, i));
                }

                if(capturesEast.NthBitSet((int)i))
                {
                    moves.Add(new Move((uint)i + 7, i));
                }
            }

            // En passant
            if(lastMove != null && lastMove.To - lastMove.From == 16 && board.WPawns.NthBitSet((int)lastMove.To))
            {
                UInt64 possibleCapturerWest = ((UInt64)1 << ((int)lastMove.To - 1)) & board.BPawns & ~BitBoard.AFile;
                UInt64 possibleCapturerEast = ((UInt64)1 << ((int)lastMove.To + 1)) & board.BPawns & ~BitBoard.HFile;
               
               if(possibleCapturerWest != 0)
               {
                    UInt64 destination = possibleCapturerWest >> 7;
                    if((board.Empty & destination) == destination)
                    {
                        moves.Add(new Move((uint)lastMove.To - 1, (uint)lastMove.To -7, specialMove: SpecialMove.EnPassant));
                    }
               }

               if(possibleCapturerEast != 0)
               {
                    UInt64 destination = possibleCapturerEast >> 9;
                    if((board.Empty & destination) == destination)
                    {
                        moves.Add(new Move((uint)lastMove.To + 1, (uint)lastMove.To - 9, specialMove: SpecialMove.EnPassant));
                    }
               }
            }

            return moves;
        }
    }
}
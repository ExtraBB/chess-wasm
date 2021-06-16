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
            return MoveService.CreateMovesFromBitboard(KingMovesLookupTable[position] & ~board.WhitePieces, position, Piece.WKing).Concat(GetCastlingMoves(board, Piece.WKing);
        }

        public static IEnumerable<Move> CalculateBKingMoves(Board board)
        {
            if(KingMovesLookupTable == null)
            {
                GenerateKingMoves();
            }
            
            int position = board.BKing.GetLS1BIndex();
            return MoveService.CreateMovesFromBitboard(KingMovesLookupTable[position] & ~board.BlackPieces, position, Piece.BKing).Concat(GetCastlingMoves(board, Piece.BKing));
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

        private static IEnumerable<Move> GetCastlingMoves(Board board, Piece piece)
        {
            List<Move> moves = new List<Move>(4);

            if(piece == Piece.WKing && !board.WKingMoved)
            {
                if(!board.WRookLeftMoved && !board.SquareIsInCheck(Square.B1, Piece.White) && !board.SquareIsInCheck(Square.C1, Piece.White) && !board.SquareIsInCheck(Square.D1, Piece.White))
                {
                    moves.Add(new Move(Piece.WKing, Square.E1, Square.C1, specialMove: SpecialMove.Castling));
                }
                else if(!board.WRookRightMoved && !board.SquareIsInCheck(Square.E1, Piece.White) && !board.SquareIsInCheck(Square.F1, Piece.White) && !board.SquareIsInCheck(Square.G1, Piece.White))
                {
                    moves.Add(new Move(Piece.WKing, Square.E1, Square.G1, specialMove: SpecialMove.Castling));
                }
            }
            else if(piece == Piece.BKing && !board.BKingMoved)
            {
                if(!board.BRookLeftMoved && !board.SquareIsInCheck(Square.B8, Piece.Black) && !board.SquareIsInCheck(Square.C8, Piece.Black) && !board.SquareIsInCheck(Square.D8, Piece.Black))
                {
                    moves.Add(new Move(Piece.BKing, Square.E8, Square.C8, specialMove: SpecialMove.Castling));
                }
                else if(!board.BRookRightMoved && !board.SquareIsInCheck(Square.E8, Piece.Black) && !board.SquareIsInCheck(Square.F8, Piece.Black) && !board.SquareIsInCheck(Square.G8, Piece.Black))
                {
                    moves.Add(new Move(Piece.BKing, Square.E8, Square.G8, specialMove: SpecialMove.Castling));
                }
            } 

            return moves;
        }
    }
}
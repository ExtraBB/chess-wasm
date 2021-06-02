using System;
using System.Collections.Generic;
using System.Linq;
using ChessWasm.Models;

namespace ChessWasm.Services
{
    public static class MoveService
    {
        private static Random rand = new Random();
        
        public static Move GetRandomMove(Board board)
        {
            int from = 0, to = 0;
            while(from == to || board.Squares[from] == 0)
            {
                from = rand.Next(0, 64);
                to = rand.Next(0, 64);
            }
            return new Move(){ From = from, To = to};
        }

        public static Dictionary<int, IEnumerable<Move>> GetAllPossibleMoves(Board board)
        {
            Dictionary<int, IEnumerable<Move>> result = new Dictionary<int, IEnumerable<Move>>();

            for(int i = 0; i < 64; i++) 
            {
                IEnumerable<Move> moves = CalculateMovesForPiece(i, board);
                if(moves != null) 
                {
                    result.Add(i, moves);
                }
            }

            return result;
        }

        private static IEnumerable<Move> CalculateMovesForPiece(int position, Board board)
        {
            Piece piece = board.Squares[position];
            if(piece == 0) 
            {
                return null;
            }

            if(piece.HasFlag(Piece.Knight))
            {
                return CalculateKnightMoves(position, board);
            }
            else if(piece.HasFlag(Piece.Rook))
            {
                return CalculateRookMoves(position, board);
            }
            else if(piece.HasFlag(Piece.Bishop))
            {
                return CalculateBishopMoves(position, board);
            }
            else if(piece.HasFlag(Piece.Queen))
            {
                return CalculateBishopMoves(position, board).Concat(CalculateRookMoves(position, board));
            }
            else if(piece.HasFlag(Piece.King))
            {
                return CalculateKingMoves(position, board);
            }
            else if(piece.HasFlag(Piece.Pawn))
            {
                return CalculatePawnMoves(position, board, piece.HasFlag(Piece.White));
            }
            else
            {
                return new List<Move>();
            }
        }

        private static IEnumerable<Move> CalculateKnightMoves(int position, Board board)
        {
            List<Move> moves = new List<Move>(8);
            File file = (File)(position % 8);
            int row = position / 8;
            
            if(file != File.A)
            {
                if(row > 1) moves.Add(new Move(){ From = position, To = position - 17 });
                if(row < 6) moves.Add(new Move(){ From = position, To = position + 15 });
            }

            if(file > File.B)
            {
                if(row > 0) moves.Add(new Move(){ From = position, To = position - 10 });
                if(row < 7) moves.Add(new Move(){ From = position, To = position + 6 });
            }

            if(file != File.H)
            {
                if(row < 6) moves.Add(new Move(){ From = position, To = position + 17 });
                if(row > 1) moves.Add(new Move(){ From = position, To = position - 15 });
            }

            if(file < File.G)
            {
                if(row < 7) moves.Add(new Move(){ From = position, To = position + 10 });
                if(row > 0) moves.Add(new Move(){ From = position, To = position - 6 });
            }

            var ownColor = board.Squares[position].HasFlag(Piece.White) ? Piece.White : Piece.Black;
            return moves.Where(move => !board.Squares[move.To].HasFlag(ownColor));
        }

        private static List<Move> CalculateRookMoves(int position, Board board)
        {
            List<Move> moves = new List<Move>(8);

            int file = position % 8;
            int row = position / 8;

            var opponentColor = board.Squares[position].HasFlag(Piece.White) ? Piece.Black : Piece.White;

            for(int f = file + 1; f < 8; f++) 
            {
                var newPosition = Utils.RowFileToPosition(row, f);
                if(board.Squares[newPosition] == 0) 
                {
                     moves.Add(new Move(){ From = position, To = newPosition });
                }
                else if(board.Squares[newPosition].HasFlag(opponentColor))
                {
                    moves.Add(new Move(){ From = position, To = newPosition });
                    break;
                }
                else
                {
                    break;
                }
            }

            for(int f = file - 1; f >= 0; f--) 
            {
                var newPosition = Utils.RowFileToPosition(row, f);
                if(board.Squares[newPosition] == 0) 
                {
                     moves.Add(new Move(){ From = position, To = newPosition });
                }
                else if(board.Squares[newPosition].HasFlag(opponentColor))
                {
                    moves.Add(new Move(){ From = position, To = newPosition });
                    break;
                }
                else
                {
                    break;
                }
            }

            for(int r = row + 1; r < 8; r++) 
            {
                var newPosition = Utils.RowFileToPosition(r, file);
                if(board.Squares[newPosition] == 0) 
                {
                     moves.Add(new Move(){ From = position, To = newPosition });
                }
                else if(board.Squares[newPosition].HasFlag(opponentColor))
                {
                    moves.Add(new Move(){ From = position, To = newPosition });
                    break;
                }
                else
                {
                    break;
                }
            }

            for(int r = row - 1; r >= 0; r--) 
            {
                var newPosition = Utils.RowFileToPosition(r, file);
                if(board.Squares[newPosition] == 0) 
                {
                     moves.Add(new Move(){ From = position, To = newPosition });
                }
                else if(board.Squares[newPosition].HasFlag(opponentColor))
                {
                    moves.Add(new Move(){ From = position, To = newPosition });
                    break;
                }
                else
                {
                    break;
                }
            }

            return moves;
        }

        private static List<Move> CalculateBishopMoves(int position, Board board)
        {
            List<Move> moves = new List<Move>(8);

            int file = position % 8;
            int row = position / 8;

            int newFile = file - 1;
            int newRow = row - 1;

             var ownColor = board.Squares[position].HasFlag(Piece.White) ? Piece.White : Piece.Black;

            while(newFile >= 0 && newRow >= 0) 
            {
                var newPosition = Utils.RowFileToPosition(newRow, newFile);
                if(board.Squares[newPosition].HasFlag(ownColor)) break;

                moves.Add(new Move(){ From = position, To = Utils.RowFileToPosition(newRow, newFile) });

                if(board.Squares[newPosition] == 0)
                {
                    newFile -= 1;
                    newRow -= 1;
                }
                else break;
            }

            newFile = file + 1;
            newRow = row + 1;
            while(newFile < 8 && newRow < 8) 
            {
                var newPosition = Utils.RowFileToPosition(newRow, newFile);
                if(board.Squares[newPosition].HasFlag(ownColor)) break;

                moves.Add(new Move(){ From = position, To = Utils.RowFileToPosition(newRow, newFile) });

                if(board.Squares[newPosition] == 0)
                {
                    newFile += 1;
                    newRow += 1;
                }
                else break;
            }

            
            newFile = file - 1;
            newRow = row + 1;
            while(newFile >= 0 && newRow < 8) 
            {
                var newPosition = Utils.RowFileToPosition(newRow, newFile);
                if(board.Squares[newPosition].HasFlag(ownColor)) break;

                moves.Add(new Move(){ From = position, To = Utils.RowFileToPosition(newRow, newFile) });

                if(board.Squares[newPosition] == 0)
                {
                    newFile -= 1;
                    newRow += 1;
                }
                else break;
            }

            newFile = file + 1;
            newRow = row - 1;
            while(newFile < 8 && newRow >= 0) 
            {
                var newPosition = Utils.RowFileToPosition(newRow, newFile);
                if(board.Squares[newPosition].HasFlag(ownColor)) break;

                moves.Add(new Move(){ From = position, To = Utils.RowFileToPosition(newRow, newFile) });

                if(board.Squares[newPosition] == 0)
                {
                    newFile += 1;
                    newRow -= 1;
                }
                else break;
            }

            return moves;
        }

        private static List<Move> CalculateKingMoves(int position, Board board)
        {
            List<Move> moves = new List<Move>(8);

            int file = position % 8;
            int row = position / 8;

            var opponentColor = board.Squares[position].HasFlag(Piece.White) ? Piece.Black : Piece.White;

            for(int i = -1; i <= 1; i++)
            {
                for(int j = -1; j <= 1; j++)
                {
                    if(i == 0 && j == 0) continue;

                    int newFile = file + i;
                    int newRow = row + j;
                    int to = Utils.RowFileToPosition(newRow, newFile);
                    if(newFile >= 0 && newFile < 8 && newRow >= 0 && newRow < 8 && (board.Squares[to] == 0 || board.Squares[to].HasFlag(opponentColor)))
                    {
                         moves.Add(new Move(){ From = position, To = to });
                    }
                }
            }
            return moves;
        }

        private static List<Move> CalculatePawnMoves(int position, Board board, bool white)
        {
            List<Move> moves = new List<Move>(8);

            int file = position % 8;
            int row = position / 8;

            // TODO: En Passant

            if(white && row > 0)
            {
                int to = Utils.RowFileToPosition(row - 1, file);
                if(board.Squares[to] == 0)
                {
                    moves.Add(new Move(){ From = position, To = to });
                }

                to = Utils.RowFileToPosition(row - 1, file - 1);
                if(file > 0 && board.Squares[to].HasFlag(Piece.Black))
                {
                    moves.Add(new Move(){ From = position, To = to });
                }

                to = Utils.RowFileToPosition(row - 1, file + 1);
                if(file < 7 && board.Squares[to].HasFlag(Piece.Black))
                {
                    moves.Add(new Move(){ From = position, To = to });
                }

                // Starting row
                to = Utils.RowFileToPosition(row - 2, file);
                int between = Utils.RowFileToPosition(row - 1, file);
                if(row == 6 && board.Squares[to] == 0 && board.Squares[between] == 0) 
                {
                    moves.Add(new Move(){ From = position, To = to });
                }
            } 
            else if(!white && row < 7)
            {
                int to = Utils.RowFileToPosition(row + 1, file);
                if(board.Squares[to] == 0)
                {
                    moves.Add(new Move(){ From = position, To = to });
                }

                to = Utils.RowFileToPosition(row + 1, file - 1);
                if(file > 0 && board.Squares[to].HasFlag(Piece.White))
                {
                    moves.Add(new Move(){ From = position, To = to });
                }

                to = Utils.RowFileToPosition(row + 1, file + 1);
                if(file < 7 && board.Squares[to].HasFlag(Piece.White))
                {
                    moves.Add(new Move(){ From = position, To = to });
                }

                // Starting row
                to = Utils.RowFileToPosition(row + 2, file);
                int between = Utils.RowFileToPosition(row + 1, file);
                if(row == 1 && board.Squares[to] == 0 && board.Squares[between] == 0) 
                {
                    moves.Add(new Move(){ From = position, To = to });
                }   
            } 

            return moves;
        }

        public static bool MoveIsLegal(Move move, Board board)
        {
            return MoveIsLegal(move.From, move.To, board);
        }

        private static bool MoveIsLegal(int from, int to, Board board)
        {
            // TODO: implement
            return true;
        }
    }
}
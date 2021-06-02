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

            IEnumerable<Move> moves = new List<Move>();

            if(piece.HasFlag(Piece.Knight))
            {
                moves = CalculateKnightMoves(position, board);
            }
            else if(piece.HasFlag(Piece.Rook))
            {
                moves = CalculateRookMoves(position, board);
            }
            else if(piece.HasFlag(Piece.Bishop))
            {
                moves = CalculateBishopMoves(position, board);
            }
            else if(piece.HasFlag(Piece.Queen))
            {
                moves = CalculateBishopMoves(position, board).Concat(CalculateRookMoves(position, board));
            }
            else if(piece.HasFlag(Piece.King))
            {
                moves = CalculateKingMoves(position, board);
            }
            else if(piece.HasFlag(Piece.Pawn))
            {
                moves = CalculatePawnMoves(position, board, piece.HasFlag(Piece.White));
            }

            return moves.Where(m => !KingIsInCheckAfterMove(board, m));
        }

        private static IEnumerable<Move> CalculateKnightMoves(int position, Board board)
        {
            List<Move> moves = new List<Move>(8);
            File file = (File)(position % 8);
            int row = position / 8;
            var ownColor = board.Squares[position].HasFlag(Piece.White) ? Piece.White : Piece.Black;
            
            if(file != File.A)
            {
                if(row > 1) moves.Add(new Move(){ From = position, To = position - 17, Player = ownColor });
                if(row < 6) moves.Add(new Move(){ From = position, To = position + 15, Player = ownColor });
            }

            if(file > File.B)
            {
                if(row > 0) moves.Add(new Move(){ From = position, To = position - 10, Player = ownColor });
                if(row < 7) moves.Add(new Move(){ From = position, To = position + 6, Player = ownColor });
            }

            if(file != File.H)
            {
                if(row < 6) moves.Add(new Move(){ From = position, To = position + 17, Player = ownColor });
                if(row > 1) moves.Add(new Move(){ From = position, To = position - 15, Player = ownColor });
            }

            if(file < File.G)
            {
                if(row < 7) moves.Add(new Move(){ From = position, To = position + 10, Player = ownColor });
                if(row > 0) moves.Add(new Move(){ From = position, To = position - 6, Player = ownColor });
            }

            return moves.Where(move => !board.Squares[move.To].HasFlag(ownColor));
        }

        private static List<Move> CalculateRookMoves(int position, Board board)
        {
            List<Move> moves = new List<Move>(8);

            int file = position % 8;
            int row = position / 8;

            var opponentColor = board.Squares[position].HasFlag(Piece.White) ? Piece.Black : Piece.White;
            var ownColor = board.Squares[position].HasFlag(Piece.White) ? Piece.White : Piece.Black;

            for(int f = file + 1; f < 8; f++) 
            {
                var newPosition = Utils.RowFileToPosition(row, f);
                if(board.Squares[newPosition] == 0) 
                {
                     moves.Add(new Move(){ From = position, To = newPosition, Player = ownColor });
                }
                else if(board.Squares[newPosition].HasFlag(opponentColor))
                {
                    moves.Add(new Move(){ From = position, To = newPosition, Player = ownColor });
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
                     moves.Add(new Move(){ From = position, To = newPosition, Player = ownColor });
                }
                else if(board.Squares[newPosition].HasFlag(opponentColor))
                {
                    moves.Add(new Move(){ From = position, To = newPosition, Player = ownColor });
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
                     moves.Add(new Move(){ From = position, To = newPosition, Player = ownColor });
                }
                else if(board.Squares[newPosition].HasFlag(opponentColor))
                {
                    moves.Add(new Move(){ From = position, To = newPosition, Player = ownColor });
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
                     moves.Add(new Move(){ From = position, To = newPosition, Player = ownColor });
                }
                else if(board.Squares[newPosition].HasFlag(opponentColor))
                {
                    moves.Add(new Move(){ From = position, To = newPosition, Player = ownColor });
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

                moves.Add(new Move(){ From = position, To = newPosition, Player = ownColor});

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

                moves.Add(new Move(){ From = position, To = newPosition, Player = ownColor });

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

                moves.Add(new Move(){ From = position, To = newPosition, Player = ownColor });

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

                moves.Add(new Move(){ From = position, To = newPosition, Player = ownColor });

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
            var ownColor = board.Squares[position].HasFlag(Piece.White) ? Piece.White : Piece.Black;

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
                         moves.Add(new Move(){ From = position, To = to, Player = ownColor });
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
                    moves.Add(new Move(){ From = position, To = to, Player = Piece.White });
                }

                to = Utils.RowFileToPosition(row - 1, file - 1);
                if(file > 0 && board.Squares[to].HasFlag(Piece.Black))
                {
                    moves.Add(new Move(){ From = position, To = to, Player = Piece.White });
                }

                to = Utils.RowFileToPosition(row - 1, file + 1);
                if(file < 7 && board.Squares[to].HasFlag(Piece.Black))
                {
                    moves.Add(new Move(){ From = position, To = to, Player = Piece.White });
                }

                // Starting row
                to = Utils.RowFileToPosition(row - 2, file);
                int between = Utils.RowFileToPosition(row - 1, file);
                if(row == 6 && board.Squares[to] == 0 && board.Squares[between] == 0) 
                {
                    moves.Add(new Move(){ From = position, To = to, Player = Piece.White });
                }
            } 
            else if(!white && row < 7)
            {
                int to = Utils.RowFileToPosition(row + 1, file);
                if(board.Squares[to] == 0)
                {
                    moves.Add(new Move(){ From = position, To = to, Player = Piece.Black });
                }

                to = Utils.RowFileToPosition(row + 1, file - 1);
                if(file > 0 && board.Squares[to].HasFlag(Piece.White))
                {
                    moves.Add(new Move(){ From = position, To = to, Player = Piece.Black });
                }

                to = Utils.RowFileToPosition(row + 1, file + 1);
                if(file < 7 && board.Squares[to].HasFlag(Piece.White))
                {
                    moves.Add(new Move(){ From = position, To = to, Player = Piece.Black });
                }

                // Starting row
                to = Utils.RowFileToPosition(row + 2, file);
                int between = Utils.RowFileToPosition(row + 1, file);
                if(row == 1 && board.Squares[to] == 0 && board.Squares[between] == 0) 
                {
                    moves.Add(new Move(){ From = position, To = to, Player = Piece.Black });
                }   
            } 

            return moves;
        }

        private static bool KingIsInCheckAfterMove(Board board, Move move) 
        {
            var squaresCopy = new Piece[64];
            Array.Copy(board.Squares, squaresCopy, 64);

            squaresCopy[move.To] = squaresCopy[move.From];
            squaresCopy[move.From] = 0;

            int kingPosition = Array.FindIndex(squaresCopy, piece => piece.HasFlag(move.Player) && piece.HasFlag(Piece.King));
            var opponentColor = move.Player.HasFlag(Piece.White) ? Piece.Black : Piece.White;

            // Check sliding pieces
            int checkDistance = 1;
            bool[,] directionsChecked = new bool[3,3];
            bool isInCheck = false;

            while(!isInCheck && !directionsChecked.Cast<bool>().All(b => b))
            {
                for(int i = -checkDistance; i <= checkDistance; i += checkDistance)
                {
                    if(isInCheck) break;

                    for(int j = -checkDistance; j <= checkDistance; j += checkDistance)
                    {
                        // Direction already closed
                        if(directionsChecked[Math.Sign(i) + 1, Math.Sign(j) + 1]) 
                        {
                            continue;
                        }

                        // close direction when out of bounds
                        int row = kingPosition / 8 + i;
                        int file = kingPosition % 8 + j;
                        if(row < 0 || row > 7 || file < 0 || file > 7 || (i == 0 && j == 0)) 
                        {
                            directionsChecked[Math.Sign(i) + 1, Math.Sign(j) + 1] = true;
                            continue;
                        }

                        int otherPosition = Utils.RowFileToPosition(row, file);
                        Piece other = squaresCopy[otherPosition];

                        // close direction when encountering any piece
                        if(other.HasFlag(move.Player)) 
                        {
                            directionsChecked[Math.Sign(i) + 1, Math.Sign(j) + 1] = true;
                            continue;
                        }
                        else if(other.HasFlag(opponentColor))
                        {
                            if(other.HasFlag(Piece.Queen)) 
                            {
                                isInCheck = true;
                                break;
                            }
                            else if (other.CapturesDiagonal() && i != 0 && j != 0)
                            {
                                if(checkDistance == 1 && other.CanMoveOneSquare() || checkDistance >= 1 && other.CanMoveUnlimitedSquares())
                                {
                                    isInCheck = true;
                                    break;
                                }
                            }
                            else if (other.CapturesStraight() && (i == 0 || j == 0))
                            {
                                if(checkDistance == 1 && other.CanMoveOneSquare() || checkDistance >= 1 && other.CanMoveUnlimitedSquares())
                                {
                                    isInCheck = true;
                                    break;
                                }
                            }

                            directionsChecked[Math.Sign(i) + 1, Math.Sign(j) + 1] = true;
                            continue;
                        }
                    }
                }

                checkDistance++;
            }

            // Check Knights
            if(!isInCheck)
            {
                for(int i = 0; i < 64; i++) 
                {
                    if(squaresCopy[i].HasFlag(Piece.Knight) && squaresCopy[i].HasFlag(opponentColor))
                    {
                        int[] attackSquares = new [] 
                        {
                            i + 6, i - 6,
                            i + 10, i - 10,
                            i + 15, i - 15,
                            i + 17, i - 17
                        };
                        
                        if(attackSquares.Any(square => square == kingPosition))
                        {
                            isInCheck = true;
                            break;
                        }
                    }
                }
            }

            return isInCheck;
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
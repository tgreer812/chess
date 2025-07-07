using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chesslib.Controllers
{
    /// <summary>
    /// A controller that picks a random valid move for its side.
    /// </summary>
    public class RandomController : IController
    {
        private readonly Random _random = new Random();

        public async Task<(string from, string to)?> GetMoveAsync(Game game)
        {
            // Add a small delay to make the async nature more realistic and prevent UI freezing
            await Task.Delay(100);
            
            var moves = GetAllValidMoves(game);
            if (moves.Count == 0)
                return null;
            var move = moves[_random.Next(moves.Count)];
            return (move.from, move.to);
        }

        private List<(string from, string to)> GetAllValidMoves(Game game)
        {
            var moves = new List<(string from, string to)>();
            
            // Use a constant for board size (8x8 for standard chess)
            const int boardSize = 8;
            
            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    var fromSquare = game.Board.GetSquare(row, col);
                    var piece = fromSquare.Piece;
                    if (piece == null || piece.Color != game.CurrentTurn)
                        continue;
                    
                    for (int toRow = 0; toRow < boardSize; toRow++)
                    {
                        for (int toCol = 0; toCol < boardSize; toCol++)
                        {
                            var toSquare = game.Board.GetSquare(toRow, toCol);
                            if (piece.IsValidMove(game.Board, fromSquare, toSquare))
                            {
                                moves.Add((fromSquare.AlgebraicPosition, toSquare.AlgebraicPosition));
                            }
                        }
                    }
                }
            }
            return moves;
        }
    }
}

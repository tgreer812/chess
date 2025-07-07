using System;
using System.Threading.Tasks;
using chesslib;
using chesslib.Controllers;
using chesslib.Pieces;

namespace chesslib_test
{
    /// <summary>
    /// Helper class for testing controllers.
    /// </summary>
    internal class TestHelpers
    {
        /// <summary>
        /// A simple test controller that returns predefined moves.
        /// </summary>
        public class TestController : IController
        {
            private readonly (string from, string to)[] _moves;
            private int _moveIndex = 0;
            
            public TestController(params (string from, string to)[] moves)
            {
                _moves = moves;
            }
            
            public Task<(string from, string to)?> GetMoveAsync(Game game)
            {
                if (_moveIndex >= _moves.Length)
                {
                    return Task.FromResult<(string from, string to)?>(null);
                }
                
                var move = _moves[_moveIndex++];
                return Task.FromResult<(string from, string to)?>(move);
            }
        }
        
        /// <summary>
        /// Executes a sequence of moves on the game board.
        /// </summary>
        /// <param name="game">The game to execute moves on.</param>
        /// <param name="moves">Array of from/to positions.</param>
        /// <returns>True if all moves were successful, false otherwise.</returns>
        public static bool ExecuteMoves(Game game, params (string from, string to)[] moves)
        {
            foreach (var move in moves)
            {
                bool success = game.TryMove(move.from, move.to);
                if (!success)
                {
                    return false;
                }
            }
            return true;
        }
        
        /// <summary>
        /// Sets up a board from FEN notation (simplified).
        /// </summary>
        /// <param name="board">The board to set up.</param>
        /// <param name="fen">The FEN notation string.</param>
        public static void SetupBoardFromFen(Board board, string fen)
        {
            var parts = fen.Split(' ');
            var piecePlacement = parts[0];
            var ranks = piecePlacement.Split('/');
            
            for (int rank = 0; rank < Math.Min(8, ranks.Length); rank++)
            {
                int file = 0;
                foreach (char c in ranks[rank])
                {
                    if (char.IsDigit(c))
                    {
                        file += int.Parse(c.ToString());
                    }
                    else if (file < 8)
                    {
                        var color = char.IsUpper(c) ? PieceColor.White : PieceColor.Black;
                        Piece? piece = char.ToLower(c) switch
                        {
                            'p' => new Pawn(color),
                            'r' => new Rook(color),
                            'n' => new Knight(color),
                            'b' => new Bishop(color),
                            'q' => new Queen(color),
                            'k' => new King(color),
                            _ => null
                        };
                        
                        if (piece != null)
                        {
                            board.GetSquare(rank, file).Piece = piece;
                        }
                        
                        file++;
                    }
                }
            }
        }
    }
}

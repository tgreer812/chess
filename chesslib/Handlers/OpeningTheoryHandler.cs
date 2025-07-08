using System.Collections.Generic;
using System.Linq;

namespace chesslib.Handlers
{
    /// <summary>
    /// Handles opening theory validation for educational scenarios.
    /// </summary>
    public class OpeningTheoryHandler : IGameHandler
    {
        private readonly Dictionary<string, string[]> _openingMoves;
        private readonly bool _strictMode;
        
        /// <summary>
        /// Initializes a new instance of the OpeningTheoryHandler.
        /// </summary>
        /// <param name="openingMoves">Dictionary mapping position hashes to allowed moves.</param>
        /// <param name="strictMode">If true, only theory moves are allowed. If false, provides feedback but allows all legal moves.</param>
        public OpeningTheoryHandler(Dictionary<string, string[]>? openingMoves = null, bool strictMode = false)
        {
            _openingMoves = openingMoves ?? GetDefaultOpenings();
            _strictMode = strictMode;
        }
        
        /// <summary>
        /// Validates whether a move follows opening theory.
        /// </summary>
        public MoveResult ValidateMove(Game game, Square from, Square to)
        {
            string moveNotation = $"{from.AlgebraicPosition}{to.AlgebraicPosition}";
            string positionKey = GetPositionKey(game);
            
            bool isTheoreticalMove = false;
            if (_openingMoves.TryGetValue(positionKey, out string[]? allowedMoves))
            {
                isTheoreticalMove = allowedMoves.Contains(moveNotation);
            }
            
            if (_strictMode)
            {
                if (!isTheoreticalMove)
                {
                    return MoveResult.Failure("Move not in opening theory");
                }
                return MoveResult.Success("Excellent theoretical choice!");
            }
            else
            {
                // In non-strict mode, always allow the move but provide feedback
                string feedback = isTheoreticalMove 
                    ? "Good theoretical move!" 
                    : "This move is not in the main line";
                return MoveResult.Success(feedback);
            }
        }
        
        /// <summary>
        /// Simple boolean check - in strict mode checks theory, otherwise always true.
        /// </summary>
        public bool IsValidMove(Game game, Square from, Square to)
        {
            if (!_strictMode) return true;
            return ValidateMove(game, from, to).IsValid;
        }
        
        /// <summary>
        /// Gets a position key for the current game state.
        /// </summary>
        private string GetPositionKey(Game game)
        {
            // Simple implementation - could be enhanced with proper position hashing
            return $"move_{game.MoveHistory.Count}";
        }
        
        /// <summary>
        /// Gets default opening moves for common openings.
        /// </summary>
        private Dictionary<string, string[]> GetDefaultOpenings()
        {
            return new Dictionary<string, string[]>
            {
                // First moves
                ["move_0"] = new[] { "e2e4", "d2d4", "g1f3", "c2c4" },
                // Responses to 1.e4
                ["move_1_e2e4"] = new[] { "e7e5", "c7c5", "e7e6", "c7c6" },
                // Italian Game: 1.e4 e5 2.Nf3 Nc6 3.Bc4
                ["move_2_e2e4_e7e5"] = new[] { "g1f3", "f2f4" },
                ["move_3_e2e4_e7e5_g1f3"] = new[] { "b8c6", "f7f5" },
                ["move_4_e2e4_e7e5_g1f3_b8c6"] = new[] { "f1c4", "f1b5" }
            };
        }
    }
}

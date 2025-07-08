using chesslib.Pieces;

namespace chesslib
{
    /// <summary>
    /// Represents the result of a move validation.
    /// </summary>
    public class MoveResult
    {
        /// <summary>
        /// Gets or sets whether the move is valid.
        /// </summary>
        public bool IsValid { get; set; }
        
        /// <summary>
        /// Gets or sets the reason why the move is invalid (if applicable).
        /// </summary>
        public string? InvalidReason { get; set; }
        
        /// <summary>
        /// Gets or sets whether this move puts the opponent in check.
        /// </summary>
        public bool PutsOpponentInCheck { get; set; }
        
        /// <summary>
        /// Gets or sets whether this move gets the player out of check.
        /// </summary>
        public bool GetsOutOfCheck { get; set; }
        
        /// <summary>
        /// Gets or sets additional feedback about the move (e.g., "Good theoretical choice").
        /// </summary>
        public string? Feedback { get; set; }
        
        /// <summary>
        /// Creates a successful move result.
        /// </summary>
        public static MoveResult Success(string? feedback = null)
        {
            return new MoveResult { IsValid = true, Feedback = feedback };
        }
        
        /// <summary>
        /// Creates a failed move result.
        /// </summary>
        public static MoveResult Failure(string reason)
        {
            return new MoveResult { IsValid = false, InvalidReason = reason };
        }
    }
    
    /// <summary>
    /// Interface for handling game logic and move validation.
    /// </summary>
    public interface IGameHandler
    {
        /// <summary>
        /// Validates whether a move is allowed according to this handler's rules.
        /// </summary>
        /// <param name="game">The current game state.</param>
        /// <param name="from">The source square.</param>
        /// <param name="to">The destination square.</param>
        /// <returns>A result indicating whether the move is valid and providing feedback.</returns>
        MoveResult ValidateMove(Game game, Square from, Square to);
        
        /// <summary>
        /// Simple boolean check for move validity (for backward compatibility).
        /// </summary>
        /// <param name="game">The current game state.</param>
        /// <param name="from">The source square.</param>
        /// <param name="to">The destination square.</param>
        /// <returns>True if the move is valid, false otherwise.</returns>
        bool IsValidMove(Game game, Square from, Square to);
    }
}

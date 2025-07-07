using System;
using System.Threading.Tasks;

namespace chesslib.Controllers
{
    /// <summary>
    /// Interface for a chess controller (human or AI).
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// Called by the game to request a move from this controller.
        /// </summary>
        /// <param name="game">The current game state.</param>
        /// <returns>A Task that returns a tuple of source and destination squares in algebraic notation (e.g., ("e2", "e4")), or null if no move is available.</returns>
        Task<(string from, string to)?> GetMoveAsync(Game game);
    }
}

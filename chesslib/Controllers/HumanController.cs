using System;
using System.Threading.Tasks;

namespace chesslib.Controllers
{
    /// <summary>
    /// A controller for a human player. The frontend should call SetNextMove to provide the move selected by the user.
    /// </summary>
    public class HumanController : IController
    {
        private TaskCompletionSource<(string from, string to)?>? _moveSource;
        private int _moveRequestId = 0; // Tracks the current move request
        private int _lastProcessedMoveId = 0; // Tracks the last processed move
        private (string from, string to)? _queuedMove; // Holds a move if SetNextMove is called before GetMoveAsync

        /// <summary>
        /// Gets a value indicating whether the controller is currently waiting for a move.
        /// </summary>
        public bool IsWaitingForMove => _moveSource != null && !_moveSource.Task.IsCompleted;
        
        /// <summary>
        /// Called by the frontend to provide the move selected by the user.
        /// </summary>
        /// <param name="from">The starting position in algebraic notation.</param>
        /// <param name="to">The destination position in algebraic notation.</param>
        /// <param name="moveId">Optional ID that must match the current request ID to be processed.</param>
        public void SetNextMove(string from, string to, int? moveId = null)
        {
            // If moveId is provided and doesn't match current request, ignore this move
            if (moveId.HasValue && moveId.Value != _moveRequestId)
                return;
            
            // If there's a waiting move request, complete it immediately
            if (_moveSource != null && !_moveSource.Task.IsCompleted)
            {
                // Mark this move as processed
                _lastProcessedMoveId = _moveRequestId;
                _moveSource.TrySetResult((from, to));
                return;
            }
            
            // If we've already processed a move with this ID and it's greater than 0, ignore this move
            // Allow processing if _moveRequestId is 0 (initial state)
            if (_moveRequestId > 0 && _lastProcessedMoveId >= _moveRequestId)
                return;
            
            // No waiting request, so queue the move for the next GetMoveAsync call
            _queuedMove = (from, to);
        }

        /// <summary>
        /// Called by the game to request a move from the human player.
        /// </summary>
        public Task<(string from, string to)?> GetMoveAsync(Game game)
        {
            // Increment the move request ID to ensure we get fresh input
            _moveRequestId++;
            
            // If there's a queued move from a previous SetNextMove call, return it immediately
            if (_queuedMove.HasValue)
            {
                var move = _queuedMove.Value;
                _queuedMove = null; // Clear the queued move
                _lastProcessedMoveId = _moveRequestId; // Mark as processed
                return Task.FromResult<(string from, string to)?>(move);
            }
            
            // Create a new task completion source for this move request
            _moveSource = new TaskCompletionSource<(string from, string to)?>();
            
            return _moveSource.Task;
        }
        
        /// <summary>
        /// Cancels the current move request, if any.
        /// </summary>
        public void CancelMoveRequest()
        {
            _moveSource?.TrySetResult(null);
            _moveSource = null;
        }
    }
}

using System;
using chesslib.Pieces;

namespace chesslib.Handlers
{
    /// <summary>
    /// Handles standard chess rules validation.
    /// </summary>
    public class ChessRulesHandler : IGameHandler
    {
        /// <summary>
        /// Validates whether a move is allowed according to standard chess rules.
        /// </summary>
        public MoveResult ValidateMove(Game game, Square from, Square to)
        {
            // Game over check
            if (game.IsGameOver)
                return MoveResult.Failure("Game is over");
                
            // Check if source square has a piece
            if (from.Piece == null)
                return MoveResult.Failure("No piece on source square");
                
            // Check if the piece belongs to the current player
            if (from.Piece.Color != game.CurrentTurn)
                return MoveResult.Failure("Not your piece");
                
            // Check if the move is valid according to the piece's rules
            if (!from.Piece.IsValidMove(game.Board, from, to))
                return MoveResult.Failure("Invalid move for this piece");
                
            // Check if the move would leave the king in check
            if (WouldLeaveKingInCheck(game, from, to))
                return MoveResult.Failure("Move would leave king in check");
            
            // Check if this move puts opponent in check
            bool putsOpponentInCheck = WouldPutOpponentInCheck(game, from, to);
            
            return new MoveResult
            {
                IsValid = true,
                PutsOpponentInCheck = putsOpponentInCheck,
                Feedback = putsOpponentInCheck ? "Check!" : null
            };
        }
        
        /// <summary>
        /// Simple boolean check for move validity.
        /// </summary>
        public bool IsValidMove(Game game, Square from, Square to)
        {
            return ValidateMove(game, from, to).IsValid;
        }
        
        /// <summary>
        /// Checks if making a move would leave the current player's king in check.
        /// </summary>
        private bool WouldLeaveKingInCheck(Game game, Square from, Square to)
        {
            // Save the current state
            Piece? movingPiece = from.Piece;
            Piece? capturedPiece = to.Piece;

            // Special case for en passant capture
            Square? enPassantSquare = null;
            Piece? enPassantCapturedPawn = null;
            if (movingPiece is Pawn && to == game.EnPassantCaptureSquare)
            {
                enPassantSquare = game.Board.GetSquare(from.Row, to.Column);
                enPassantCapturedPawn = enPassantSquare.Piece;
                enPassantSquare.Piece = null;
            }
            
            // Temporarily make the move
            to.Piece = movingPiece;
            from.Piece = null;
            
            bool kingInCheck = game.IsInCheck(game.CurrentTurn);
            
            // Restore the original position
            from.Piece = movingPiece;
            to.Piece = capturedPiece;
            
            // Restore en passant captured pawn if needed
            if (enPassantSquare != null && enPassantCapturedPawn != null)
            {
                enPassantSquare.Piece = enPassantCapturedPawn;
            }
            
            return kingInCheck;
        }
        
        /// <summary>
        /// Checks if making a move would put the opponent's king in check.
        /// </summary>
        private bool WouldPutOpponentInCheck(Game game, Square from, Square to)
        {
            // Save the current state
            Piece? movingPiece = from.Piece;
            Piece? capturedPiece = to.Piece;
            PieceColor opponentColor = game.CurrentTurn == PieceColor.White ? PieceColor.Black : PieceColor.White;

            // Temporarily make the move
            to.Piece = movingPiece;
            from.Piece = null;
            
            bool opponentInCheck = game.IsInCheck(opponentColor);
            
            // Restore the original position
            from.Piece = movingPiece;
            to.Piece = capturedPiece;
            
            return opponentInCheck;
        }
    }
}

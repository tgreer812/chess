using System;

namespace chesslib.Pieces
{
    /// <summary>
    /// Represents a king chess piece.
    /// </summary>
    public class King : Piece
    {
        /// <summary>
        /// Tracks whether the king has moved yet (for castling rule).
        /// </summary>
        private bool hasMoved;

        /// <summary>
        /// Initializes a new instance of the King class.
        /// </summary>
        /// <param name="color">The color of the king.</param>
        public King(PieceColor color) : base(color)
        {
            hasMoved = false;
        }

        /// <summary>
        /// Records that the king has moved.
        /// </summary>
        public void SetHasMoved()
        {
            hasMoved = true;
        }

        /// <summary>
        /// Gets whether the king has moved.
        /// </summary>
        public bool HasMoved => hasMoved;
        
        /// <summary>
        /// Validates if a move from the source square to the destination square is legal for a king.
        /// Kings move one square in any direction or castling moves.
        /// </summary>
        /// <param name="board">The current chess board.</param>
        /// <param name="sourceSquare">The source square where the king is currently located.</param>
        /// <param name="destSquare">The destination square where the king wants to move.</param>
        /// <returns>True if the move is legal, false otherwise.</returns>
        public override bool IsValidMove(Board board, Square sourceSquare, Square destSquare)
        {
            // Calculate the absolute differences in rows and columns
            int rowDiff = Math.Abs(destSquare.Row - sourceSquare.Row);
            int colDiff = Math.Abs(destSquare.Column - sourceSquare.Column);
            
            // Ensure we're not moving to the same square
            if (rowDiff == 0 && colDiff == 0)
                return false;
                
            // Check if the destination has a piece of the same color
            if (destSquare.Piece != null && destSquare.Piece.Color == Color)
                return false;
            
            // Regular king move - one square in any direction
            if (rowDiff <= 1 && colDiff <= 1)
                return true;
            
            // Check if this is a castling move
            if (rowDiff == 0 && colDiff == 2 && !hasMoved)
            {
                // Make sure the king is on the correct rank
                int castlingRank = Color == PieceColor.White ? 7 : 0;
                if (sourceSquare.Row != castlingRank)
                    return false;
                
                // Determine if this is kingside (column increasing) or queenside (column decreasing)
                int rookColumn = destSquare.Column > sourceSquare.Column ? 7 : 0;
                
                // Get the rook square
                var rookSquare = board.GetSquare(castlingRank, rookColumn);
                
                // Check if there is a rook of the same color that hasn't moved
                if (rookSquare.Piece is not Rook || rookSquare.Piece.Color != Color)
                    return false;
                    
                if (rookSquare.Piece is Rook rook && rook.HasMoved)
                    return false;
                
                // Check if there are any pieces between the king and the rook
                int startCol = Math.Min(sourceSquare.Column, rookColumn) + 1;
                int endCol = Math.Max(sourceSquare.Column, rookColumn);
                
                for (int col = startCol; col < endCol; col++)
                {
                    if (board.GetSquare(castlingRank, col).Piece != null)
                        return false;
                }
                
                // Check if the king is in check
                if (board.Game != null && board.Game.IsInCheck(Color))
                    return false;
                
                // Check if the king would pass through or end up in a check
                int step = destSquare.Column > sourceSquare.Column ? 1 : -1;
                for (int col = sourceSquare.Column + step; col != destSquare.Column + step; col += step)
                {
                    var testSquare = board.GetSquare(castlingRank, col);
                    
                    // Temporarily move the king to check if it would be in check
                    var originalPiece = testSquare.Piece;
                    var originalKingSquare = sourceSquare.Piece;
                    
                    sourceSquare.Piece = null;
                    testSquare.Piece = originalKingSquare;
                    
                    bool inCheck = false;
                    if (board.Game != null)
                        inCheck = board.Game.IsInCheck(Color);
                    
                    // Restore the original board state
                    sourceSquare.Piece = originalKingSquare;
                    testSquare.Piece = originalPiece;
                    
                    if (inCheck)
                        return false;
                }
                
                // All checks passed, castling is valid
                return true;
            }
            
            // Not a valid king move
            return false;
        }
        
        /// <summary>
        /// Returns a string representation of the king.
        /// </summary>
        /// <returns>A string representation of the king.</returns>
        public override string ToString()
        {
            return Color == PieceColor.White ? "♔" : "♚";
        }
    }
}

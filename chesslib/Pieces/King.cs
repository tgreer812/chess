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
        /// Kings move one square in any direction.
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
            
            // Kings move one square in any direction
            if (rowDiff > 1 || colDiff > 1)
                return false;
                
            // Ensure we're not moving to the same square
            if (rowDiff == 0 && colDiff == 0)
                return false;
                
            // Check if the destination has a piece of the same color
            if (destSquare.Piece != null && destSquare.Piece.Color == Color)
                return false;

            // TODO: Add castling logic
            // TODO: Add check validation (kings cannot move into check)
            
            return true;
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

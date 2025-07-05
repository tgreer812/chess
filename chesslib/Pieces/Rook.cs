using System;

namespace chesslib.Pieces
{
    /// <summary>
    /// Represents a rook chess piece.
    /// </summary>
    public class Rook : Piece
    {
        /// <summary>
        /// Tracks whether the rook has moved yet (for castling rule).
        /// </summary>
        private bool hasMoved;
        
        /// <summary>
        /// Initializes a new instance of the Rook class.
        /// </summary>
        /// <param name="color">The color of the rook.</param>
        public Rook(PieceColor color) : base(color) 
        {
            hasMoved = false;
        }
        
        /// <summary>
        /// Records that the rook has moved.
        /// </summary>
        public void SetHasMoved()
        {
            hasMoved = true;
        }
        
        /// <summary>
        /// Gets whether the rook has moved.
        /// </summary>
        public bool HasMoved => hasMoved;
        
        /// <summary>
        /// Validates if a move from the source square to the destination square is legal for a rook.
        /// Rooks move horizontally or vertically any number of squares.
        /// </summary>
        /// <param name="board">The current chess board.</param>
        /// <param name="sourceSquare">The source square where the rook is currently located.</param>
        /// <param name="destSquare">The destination square where the rook wants to move.</param>
        /// <returns>True if the move is legal, false otherwise.</returns>
        public override bool IsValidMove(Board board, Square sourceSquare, Square destSquare)
        {
            // Ensure we're not moving to the same square
            if (sourceSquare.Row == destSquare.Row && sourceSquare.Column == destSquare.Column)
                return false;
                
            // Check if we're moving along a rank (horizontal)
            if (sourceSquare.Row == destSquare.Row)
            {
                int start = Math.Min(sourceSquare.Column, destSquare.Column) + 1;
                int end = Math.Max(sourceSquare.Column, destSquare.Column);
                
                // Check for pieces in between
                for (int col = start; col < end; col++)
                {
                    if (board.GetSquare(sourceSquare.Row, col).Piece != null)
                        return false;
                }
                
                // Check if destination has a piece of the same color
                if (destSquare.Piece != null && destSquare.Piece.Color == this.Color)
                    return false;
                    
                return true;
            }
            
            // Check if we're moving along a file (vertical)
            if (sourceSquare.Column == destSquare.Column)
            {
                int start = Math.Min(sourceSquare.Row, destSquare.Row) + 1;
                int end = Math.Max(sourceSquare.Row, destSquare.Row);
                
                // Check for pieces in between
                for (int row = start; row < end; row++)
                {
                    if (board.GetSquare(row, sourceSquare.Column).Piece != null)
                        return false;
                }
                
                // Check if destination has a piece of the same color
                if (destSquare.Piece != null && destSquare.Piece.Color == this.Color)
                    return false;
                    
                return true;
            }
            
            // Not moving along a rank or file
            return false;
        }
        
        /// <summary>
        /// Returns a string representation of the rook.
        /// </summary>
        /// <returns>A string representation of the rook.</returns>
        public override string ToString()
        {
            return Color == PieceColor.White ? "♖" : "♜";
        }
    }
}

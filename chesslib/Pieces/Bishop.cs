using System;

namespace chesslib.Pieces
{
    /// <summary>
    /// Represents a bishop chess piece.
    /// </summary>
    public class Bishop : Piece
    {
        /// <summary>
        /// Initializes a new instance of the Bishop class.
        /// </summary>
        /// <param name="color">The color of the bishop.</param>
        public Bishop(PieceColor color) : base(color) { }
        
        /// <summary>
        /// Validates if a move from the source square to the destination square is legal for a bishop.
        /// Bishops move diagonally any number of squares.
        /// </summary>
        /// <param name="board">The current chess board.</param>
        /// <param name="sourceSquare">The source square where the bishop is currently located.</param>
        /// <param name="destSquare">The destination square where the bishop wants to move.</param>
        /// <returns>True if the move is legal, false otherwise.</returns>
        public override bool IsValidMove(Board board, Square sourceSquare, Square destSquare)
        {
            // Calculate the differences in rows and columns
            int rowDiff = destSquare.Row - sourceSquare.Row;
            int colDiff = destSquare.Column - sourceSquare.Column;
            
            // Ensure we're not moving to the same square
            if (rowDiff == 0 && colDiff == 0)
                return false;
                
            // Check if we're moving diagonally (absolute row difference equals absolute column difference)
            if (Math.Abs(rowDiff) != Math.Abs(colDiff))
                return false;
                
            // Determine the direction of movement
            int rowDirection = rowDiff > 0 ? 1 : -1;
            int colDirection = colDiff > 0 ? 1 : -1;
            
            // Check for pieces in between
            int steps = Math.Abs(rowDiff);
            for (int i = 1; i < steps; i++)
            {
                int checkRow = sourceSquare.Row + (i * rowDirection);
                int checkCol = sourceSquare.Column + (i * colDirection);
                
                if (board.GetSquare(checkRow, checkCol).Piece != null)
                    return false;
            }
            
            // Check if destination has a piece of the same color
            if (destSquare.Piece != null && destSquare.Piece.Color == Color)
                return false;
                
            return true;
        }
        
        /// <summary>
        /// Returns a string representation of the bishop.
        /// </summary>
        /// <returns>A string representation of the bishop.</returns>
        public override string ToString()
        {
            return Color == PieceColor.White ? "♗" : "♝";
        }
    }
}

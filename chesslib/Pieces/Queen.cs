using System;

namespace chesslib.Pieces
{
    /// <summary>
    /// Represents a queen chess piece.
    /// </summary>
    public class Queen : Piece
    {
        /// <summary>
        /// Initializes a new instance of the Queen class.
        /// </summary>
        /// <param name="color">The color of the queen.</param>
        public Queen(PieceColor color) : base(color) { }
        
        /// <summary>
        /// Validates if a move from the source square to the destination square is legal for a queen.
        /// Queens move horizontally, vertically, or diagonally any number of squares.
        /// </summary>
        /// <param name="board">The current chess board.</param>
        /// <param name="sourceSquare">The source square where the queen is currently located.</param>
        /// <param name="destSquare">The destination square where the queen wants to move.</param>
        /// <returns>True if the move is legal, false otherwise.</returns>
        public override bool IsValidMove(Board board, Square sourceSquare, Square destSquare)
        {
            // Calculate the differences in rows and columns
            int rowDiff = destSquare.Row - sourceSquare.Row;
            int colDiff = destSquare.Column - sourceSquare.Column;
            int absRowDiff = Math.Abs(rowDiff);
            int absColDiff = Math.Abs(colDiff);
            
            // Ensure we're not moving to the same square
            if (rowDiff == 0 && colDiff == 0)
                return false;
                
            // Check if the destination has a piece of the same color
            if (destSquare.Piece != null && destSquare.Piece.Color == Color)
                return false;
            
            // Queen moves like a rook (horizontally or vertically)...
            if (sourceSquare.Row == destSquare.Row || sourceSquare.Column == destSquare.Column)
            {
                // Moving horizontally
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
                    
                    return true;
                }
                
                // Moving vertically
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
                    
                    return true;
                }
            }
            
            // ...or like a bishop (diagonally)
            if (absRowDiff == absColDiff)
            {
                // Determine the direction of movement
                int rowDirection = rowDiff > 0 ? 1 : -1;
                int colDirection = colDiff > 0 ? 1 : -1;
                
                // Check for pieces in between
                for (int i = 1; i < absRowDiff; i++)
                {
                    int checkRow = sourceSquare.Row + (i * rowDirection);
                    int checkCol = sourceSquare.Column + (i * colDirection);
                    
                    if (board.GetSquare(checkRow, checkCol).Piece != null)
                        return false;
                }
                
                return true;
            }
            
            // Not moving horizontally, vertically, or diagonally
            return false;
        }
        
        /// <summary>
        /// Returns a string representation of the queen.
        /// </summary>
        /// <returns>A string representation of the queen.</returns>
        public override string ToString()
        {
            return Color == PieceColor.White ? "♕" : "♛";
        }
    }
}

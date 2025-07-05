using System;

namespace chesslib.Pieces
{
    /// <summary>
    /// Represents a knight chess piece.
    /// </summary>
    public class Knight : Piece
    {
        /// <summary>
        /// Initializes a new instance of the Knight class.
        /// </summary>
        /// <param name="color">The color of the knight.</param>
        public Knight(PieceColor color) : base(color) { }
        
        /// <summary>
        /// Validates if a move from the source square to the destination square is legal for a knight.
        /// Knights move in an L-shape: two squares in one direction and then one square perpendicular to that direction.
        /// </summary>
        /// <param name="board">The current chess board.</param>
        /// <param name="sourceSquare">The source square where the knight is currently located.</param>
        /// <param name="destSquare">The destination square where the knight wants to move.</param>
        /// <returns>True if the move is legal, false otherwise.</returns>
        public override bool IsValidMove(Board board, Square sourceSquare, Square destSquare)
        {
            // Calculate the absolute differences in rows and columns
            int rowDiff = Math.Abs(destSquare.Row - sourceSquare.Row);
            int colDiff = Math.Abs(destSquare.Column - sourceSquare.Column);
            
            // Knights move in an L-shape: (2,1) or (1,2)
            bool isLShape = (rowDiff == 2 && colDiff == 1) || (rowDiff == 1 && colDiff == 2);
            
            if (!isLShape)
                return false;
                
            // Knights can jump over pieces, so we only need to check the destination square
            // Make sure we're not capturing our own piece
            if (destSquare.Piece != null && destSquare.Piece.Color == Color)
                return false;
                
            return true;
        }
        
        /// <summary>
        /// Returns a string representation of the knight.
        /// </summary>
        /// <returns>A string representation of the knight.</returns>
        public override string ToString()
        {
            return Color == PieceColor.White ? "♘" : "♞";
        }
    }
}

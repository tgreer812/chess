using System;

namespace chesslib
{
    /// <summary>
    /// Represents a square on a chess board.
    /// </summary>
    public class Square
    {
        /// <summary>
        /// Gets the row index of the square (0-7).
        /// </summary>
        public int Row { get; }
        
        /// <summary>
        /// Gets the column index of the square (0-7).
        /// </summary>
        public int Column { get; }
        
        /// <summary>
        /// Gets the color of the square.
        /// </summary>
        public SquareColor Color { get; }
        
        /// <summary>
        /// Gets or sets the piece on this square (null if empty).
        /// </summary>
        public Piece? Piece { get; set; }
        
        /// <summary>
        /// Gets the algebraic notation for this square (e.g., "a1", "h8").
        /// </summary>
        public string AlgebraicPosition => $"{(char)('a' + Column)}{8 - Row}";

        /// <summary>
        /// Initializes a new instance of the Square class.
        /// </summary>
        /// <param name="row">The row index.</param>
        /// <param name="column">The column index.</param>
        /// <param name="color">The color of the square.</param>
        public Square(int row, int column, SquareColor color)
        {
            Row = row;
            Column = column;
            Color = color;
            Piece = null;
        }
        
        /// <summary>
        /// Returns a string representation of the square.
        /// </summary>
        /// <returns>A string representation of the square.</returns>
        public override string ToString()
        {
            if (Piece == null)
            {
                return Color == SquareColor.Light ? "□" : "■";
            }
            
            return Piece.ToString();
        }
    }
}

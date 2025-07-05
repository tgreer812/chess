using System;
using System.Collections.Generic;

namespace chesslib
{
    /// <summary>
    /// Represents a chess board with an 8x8 grid of squares.
    /// </summary>
    public class Board
    {
        /// <summary>
        /// The standard size of a chess board (8x8).
        /// </summary>
        public const int BoardSize = 8;

        /// <summary>
        /// The squares on the board represented as a 2D list.
        /// </summary>
        private readonly List<List<Square>> squares;

        /// <summary>
        /// Initializes a new instance of the Board class with empty squares.
        /// </summary>
        public Board()
        {
            // Initialize the board with empty squares
            squares = new List<List<Square>>();
            
            for (int row = 0; row < BoardSize; row++)
            {
                var rowList = new List<Square>();
                
                for (int col = 0; col < BoardSize; col++)
                {
                    // Create a new square with its position and color
                    bool isLight = (row + col) % 2 == 0;
                    rowList.Add(new Square(row, col, isLight ? SquareColor.Light : SquareColor.Dark));
                }
                
                squares.Add(rowList);
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the Board class with a specific board state.
        /// </summary>
        /// <param name="squares">The squares to initialize the board with.</param>
        /// <exception cref="ArgumentNullException">Thrown when squares is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the board dimensions are invalid.</exception>
        public Board(List<List<Square>> squares)
        {
            this.squares = squares ?? throw new ArgumentNullException(nameof(squares));
            
            // Validate board dimensions
            if (squares.Count != BoardSize)
            {
                throw new ArgumentException($"Board must have exactly {BoardSize} rows.");
            }
            
            foreach (var row in squares)
            {
                if (row.Count != BoardSize)
                {
                    throw new ArgumentException($"Each row must have exactly {BoardSize} squares.");
                }
            }
        }

        /// <summary>
        /// Gets the square at the specified position.
        /// </summary>
        /// <param name="row">The row index (0-7).</param>
        /// <param name="col">The column index (0-7).</param>
        /// <returns>The square at the specified position.</returns>
        public Square GetSquare(int row, int col)
        {
            if (row < 0 || row >= BoardSize || col < 0 || col >= BoardSize)
                throw new ArgumentOutOfRangeException("Position is outside the board boundaries.");
                
            return squares[row][col];
        }

        /// <summary>
        /// Gets the square at the specified algebraic position (e.g., "a1", "h8").
        /// </summary>
        /// <param name="algebraicPosition">The algebraic position (e.g., "a1", "h8").</param>
        /// <returns>The square at the specified position.</returns>
        public Square GetSquare(string algebraicPosition)
        {
            if (string.IsNullOrEmpty(algebraicPosition) || algebraicPosition.Length != 2)
                throw new ArgumentException("Invalid algebraic position format.");
                
            char file = algebraicPosition[0];
            char rank = algebraicPosition[1];
            
            if (file < 'a' || file > 'h' || rank < '1' || rank > '8')
                throw new ArgumentException("Invalid algebraic position.");
                
            int col = file - 'a';
            int row = BoardSize - (rank - '0');
            
            return GetSquare(row, col);
        }

        /// <summary>
        /// Gets a read-only view of the squares on the board.
        /// </summary>
        public IReadOnlyList<IReadOnlyList<Square>> Squares => squares.AsReadOnly();

        /// <summary>
        /// Returns a string representation of the board.
        /// </summary>
        /// <returns>A string representation of the board.</returns>
        public override string ToString()
        {
            var result = new System.Text.StringBuilder();
            
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    result.Append(squares[row][col].ToString());
                    result.Append(' ');
                }
                result.AppendLine();
            }
            
            return result.ToString();
        }
    }

    /// <summary>
    /// Represents the color of a square on a chess board.
    /// </summary>
    public enum SquareColor
    {
        /// <summary>
        /// Light colored square.
        /// </summary>
        Light,
        
        /// <summary>
        /// Dark colored square.
        /// </summary>
        Dark
    }
}

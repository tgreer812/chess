using System;

namespace chesslib
{
    /// <summary>
    /// Base class for chess pieces.
    /// </summary>
    public abstract class Piece
    {
        /// <summary>
        /// Gets the color of the piece.
        /// </summary>
        public PieceColor Color { get; }

        /// <summary>
        /// Initializes a new instance of the Piece class.
        /// </summary>
        /// <param name="color">The color of the piece.</param>
        protected Piece(PieceColor color)
        {
            Color = color;
        }

        /// <summary>
        /// Validates if a move from the source square to the destination square is legal for this piece.
        /// </summary>
        /// <param name="board">The current chess board.</param>
        /// <param name="sourceSquare">The source square where the piece is currently located.</param>
        /// <param name="destSquare">The destination square where the piece wants to move.</param>
        /// <returns>True if the move is legal, false otherwise.</returns>
        public abstract bool IsValidMove(Board board, Square sourceSquare, Square destSquare);

        /// <summary>
        /// Returns a string representation of the piece.
        /// </summary>
        /// <returns>A string representation of the piece.</returns>
        public abstract override string ToString();
    }

    /// <summary>
    /// Represents the color of a chess piece.
    /// </summary>
    public enum PieceColor
    {
        /// <summary>
        /// White piece.
        /// </summary>
        White,
        
        /// <summary>
        /// Black piece.
        /// </summary>
        Black
    }
}

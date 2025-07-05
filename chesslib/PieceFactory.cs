using System;
using chesslib.Pieces;

namespace chesslib
{
    /// <summary>
    /// Factory class for creating chess pieces.
    /// </summary>
    public static class PieceFactory
    {
        /// <summary>
        /// Enum representing the types of chess pieces.
        /// </summary>
        public enum PieceType
        {
            Pawn,
            Knight,
            Bishop,
            Rook,
            Queen,
            King
        }

        /// <summary>
        /// Creates a new chess piece of the specified type and color.
        /// </summary>
        /// <param name="type">The type of piece to create.</param>
        /// <param name="color">The color of the piece.</param>
        /// <returns>A new chess piece instance.</returns>
        public static Piece CreatePiece(PieceType type, PieceColor color)
        {
            return type switch
            {
                PieceType.Pawn => new Pawn(color),
                PieceType.Knight => new Knight(color),
                PieceType.Bishop => new Bishop(color),
                PieceType.Rook => new Rook(color),
                PieceType.Queen => new Queen(color),
                PieceType.King => new King(color),
                _ => throw new ArgumentException($"Invalid piece type: {type}")
            };
        }

        /// <summary>
        /// Sets up the initial board position with all pieces in their standard starting positions.
        /// </summary>
        /// <param name="board">The chess board to set up.</param>
        public static void SetupStandardGame(Board board)
        {
            // Set up pawns
            for (int col = 0; col < Board.BoardSize; col++)
            {
                board.GetSquare(1, col).Piece = CreatePiece(PieceType.Pawn, PieceColor.Black);
                board.GetSquare(6, col).Piece = CreatePiece(PieceType.Pawn, PieceColor.White);
            }
            
            // Set up rooks
            board.GetSquare(0, 0).Piece = CreatePiece(PieceType.Rook, PieceColor.Black);
            board.GetSquare(0, 7).Piece = CreatePiece(PieceType.Rook, PieceColor.Black);
            board.GetSquare(7, 0).Piece = CreatePiece(PieceType.Rook, PieceColor.White);
            board.GetSquare(7, 7).Piece = CreatePiece(PieceType.Rook, PieceColor.White);
            
            // Set up knights
            board.GetSquare(0, 1).Piece = CreatePiece(PieceType.Knight, PieceColor.Black);
            board.GetSquare(0, 6).Piece = CreatePiece(PieceType.Knight, PieceColor.Black);
            board.GetSquare(7, 1).Piece = CreatePiece(PieceType.Knight, PieceColor.White);
            board.GetSquare(7, 6).Piece = CreatePiece(PieceType.Knight, PieceColor.White);
            
            // Set up bishops
            board.GetSquare(0, 2).Piece = CreatePiece(PieceType.Bishop, PieceColor.Black);
            board.GetSquare(0, 5).Piece = CreatePiece(PieceType.Bishop, PieceColor.Black);
            board.GetSquare(7, 2).Piece = CreatePiece(PieceType.Bishop, PieceColor.White);
            board.GetSquare(7, 5).Piece = CreatePiece(PieceType.Bishop, PieceColor.White);
            
            // Set up queens
            board.GetSquare(0, 3).Piece = CreatePiece(PieceType.Queen, PieceColor.Black);
            board.GetSquare(7, 3).Piece = CreatePiece(PieceType.Queen, PieceColor.White);
            
            // Set up kings
            board.GetSquare(0, 4).Piece = CreatePiece(PieceType.King, PieceColor.Black);
            board.GetSquare(7, 4).Piece = CreatePiece(PieceType.King, PieceColor.White);
        }
    }
}

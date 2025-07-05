using System;

namespace chesslib.Pieces
{
    /// <summary>
    /// Represents a pawn chess piece.
    /// </summary>
    public class Pawn : Piece
    {
        /// <summary>
        /// Tracks whether the pawn has moved yet (for two-square initial move rule).
        /// </summary>
        private bool hasMoved;

        /// <summary>
        /// Initializes a new instance of the Pawn class.
        /// </summary>
        /// <param name="color">The color of the pawn.</param>
        public Pawn(PieceColor color) : base(color)
        {
            hasMoved = false;
        }

        /// <summary>
        /// Records that the pawn has moved.
        /// </summary>
        public void SetHasMoved()
        {
            hasMoved = true;
        }

        /// <summary>
        /// Gets whether the pawn has moved.
        /// </summary>
        public bool HasMoved => hasMoved;

        /// <summary>
        /// Validates if a move from the source square to the destination square is legal for a pawn.
        /// Pawns move forward one square, or two squares from their starting position.
        /// They capture diagonally forward one square.
        /// </summary>
        /// <param name="board">The current chess board.</param>
        /// <param name="sourceSquare">The source square where the pawn is currently located.</param>
        /// <param name="destSquare">The destination square where the pawn wants to move.</param>
        /// <returns>True if the move is legal, false otherwise.</returns>
        public override bool IsValidMove(Board board, Square sourceSquare, Square destSquare)
        {
            // Direction of movement depends on color
            int direction = Color == PieceColor.White ? -1 : 1; // White pawns move up (-1 in row index), black pawns move down (+1)
            
            // Check if the move is forward
            int rowDiff = destSquare.Row - sourceSquare.Row;
            int colDiff = Math.Abs(destSquare.Column - sourceSquare.Column);
            
            // Regular move - 1 square forward
            if (rowDiff == direction && colDiff == 0)
            {
                // Can't move forward if there's a piece in the way
                return destSquare.Piece == null;
            }
            
            // Initial move - 2 squares forward
            if (rowDiff == 2 * direction && colDiff == 0 && !hasMoved)
            {
                // Check if there's a piece in the way
                Square middleSquare = board.GetSquare(sourceSquare.Row + direction, sourceSquare.Column);
                
                // Both the middle square and destination must be empty
                return middleSquare.Piece == null && destSquare.Piece == null;
            }
            
            // Capture move - 1 square diagonally
            if (rowDiff == direction && colDiff == 1)
            {
                // Must have an opponent's piece to capture
                return destSquare.Piece != null && destSquare.Piece.Color != Color;
            }
            
            // TODO: Add en passant rule
            
            return false;
        }
        
        /// <summary>
        /// Returns a string representation of the pawn.
        /// </summary>
        /// <returns>A string representation of the pawn.</returns>
        public override string ToString()
        {
            return Color == PieceColor.White ? "♙" : "♟";
        }
    }
}

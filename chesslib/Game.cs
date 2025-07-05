using System;
using System.Collections.Generic;
using chesslib.Pieces;

namespace chesslib
{
    /// <summary>
    /// Represents a chess game with its board, state, and rules.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Gets the chess board.
        /// </summary>
        public Board Board { get; private set; }
        
        /// <summary>
        /// Gets the current player's color.
        /// </summary>
        public PieceColor CurrentTurn { get; private set; }
        
        /// <summary>
        /// Gets a value indicating whether the game is over.
        /// </summary>
        public bool IsGameOver { get; private set; }
        
        /// <summary>
        /// Gets the list of moves made in the game.
        /// </summary>
        public List<Move> MoveHistory { get; }
        
        /// <summary>
        /// Initializes a new instance of the Game class with a default board setup.
        /// </summary>
        public Game()
        {
            Board = new Board();
            CurrentTurn = PieceColor.White; // White starts
            IsGameOver = false;
            MoveHistory = new List<Move>();
            
            // Set up initial board position using the PieceFactory
            PieceFactory.SetupStandardGame(Board);
        }
        
        /// <summary>
        /// Initializes a new instance of the Game class with a custom board.
        /// </summary>
        /// <param name="board">The custom board to use.</param>
        /// <param name="startingColor">The color that starts the game.</param>
        public Game(Board board, PieceColor startingColor = PieceColor.White)
        {
            Board = board ?? throw new ArgumentNullException(nameof(board));
            CurrentTurn = startingColor;
            IsGameOver = false;
            MoveHistory = new List<Move>();
        }
        
        /// <summary>
        /// Attempts to make a move on the board.
        /// </summary>
        /// <param name="fromPos">The starting position in algebraic notation (e.g., "e2").</param>
        /// <param name="toPos">The destination position in algebraic notation (e.g., "e4").</param>
        /// <returns>True if the move was successful, false otherwise.</returns>
        public bool TryMove(string fromPos, string toPos)
        {
            try
            {
                var sourceSquare = Board.GetSquare(fromPos);
                var destSquare = Board.GetSquare(toPos);
                
                return TryMove(sourceSquare, destSquare);
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
        
        /// <summary>
        /// Attempts to make a move on the board.
        /// </summary>
        /// <param name="fromRow">The starting row (0-7).</param>
        /// <param name="fromCol">The starting column (0-7).</param>
        /// <param name="toRow">The destination row (0-7).</param>
        /// <param name="toCol">The destination column (0-7).</param>
        /// <returns>True if the move was successful, false otherwise.</returns>
        public bool TryMove(int fromRow, int fromCol, int toRow, int toCol)
        {
            try
            {
                var sourceSquare = Board.GetSquare(fromRow, fromCol);
                var destSquare = Board.GetSquare(toRow, toCol);
                
                return TryMove(sourceSquare, destSquare);
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }
        
        /// <summary>
        /// Attempts to make a move on the board using square objects.
        /// </summary>
        /// <param name="sourceSquare">The source square.</param>
        /// <param name="destSquare">The destination square.</param>
        /// <returns>True if the move was successful, false otherwise.</returns>
        public bool TryMove(Square sourceSquare, Square destSquare)
        {
            // Game over check
            if (IsGameOver)
                return false;
                
            // Check if source square has a piece
            if (sourceSquare.Piece == null)
                return false;
                
            // Check if the piece belongs to the current player
            if (sourceSquare.Piece.Color != CurrentTurn)
                return false;
                
            // Check if the move is valid according to the piece's rules
            if (!sourceSquare.Piece.IsValidMove(Board, sourceSquare, destSquare))
                return false;
                
            // Create a move record and add to history
            var move = new Move
            {
                SourceSquare = sourceSquare,
                DestSquare = destSquare,
                MovedPiece = sourceSquare.Piece,
                CapturedPiece = destSquare.Piece,
                MoveNumber = MoveHistory.Count + 1
            };
            
            // Execute the move
            destSquare.Piece = sourceSquare.Piece;
            sourceSquare.Piece = null;
            
            // Update piece's movement status if needed
            if (destSquare.Piece is Pawn pawn)
            {
                pawn.SetHasMoved();
            }
            else if (destSquare.Piece is King king)
            {
                king.SetHasMoved();
            }
            
            // Add to move history
            MoveHistory.Add(move);
            
            // Switch turns
            CurrentTurn = CurrentTurn == PieceColor.White ? PieceColor.Black : PieceColor.White;
            
            // Check if the opponent's king is in check
            bool opponentInCheck = IsInCheck(CurrentTurn);
            
            // TODO: Check for checkmate and stalemate
            
            return true;
        }
        
        /// <summary>
        /// Checks if the specified color's king is in check.
        /// </summary>
        /// <param name="kingColor">The color of the king to check.</param>
        /// <returns>True if the king is in check, false otherwise.</returns>
        public bool IsInCheck(PieceColor kingColor)
        {
            // Find the king's position
            Square? kingSquare = null;
            
            for (int row = 0; row < Board.BoardSize; row++)
            {
                for (int col = 0; col < Board.BoardSize; col++)
                {
                    var square = Board.GetSquare(row, col);
                    if (square.Piece is King king && king.Color == kingColor)
                    {
                        kingSquare = square;
                        break;
                    }
                }
                
                if (kingSquare != null)
                    break;
            }
            
            if (kingSquare == null)
                return false; // No king found (shouldn't happen in a real game)
                
            // Check if any opponent's piece can attack the king
            for (int row = 0; row < Board.BoardSize; row++)
            {
                for (int col = 0; col < Board.BoardSize; col++)
                {
                    var square = Board.GetSquare(row, col);
                    if (square.Piece != null && square.Piece.Color != kingColor)
                    {
                        // If an opponent's piece can move to the king's square, the king is in check
                        if (square.Piece.IsValidMove(Board, square, kingSquare))
                            return true;
                    }
                }
            }
            
            return false;
        }
        
        /// <summary>
        /// Returns a string representation of the game.
        /// </summary>
        /// <returns>A string representation of the game.</returns>
        public override string ToString()
        {
            return $"Current turn: {CurrentTurn}, IsGameOver: {IsGameOver}\nBoard:\n{Board}";
        }
    }
    
    /// <summary>
    /// Represents a chess move.
    /// </summary>
    public class Move
    {
        /// <summary>
        /// Gets or sets the source square of the move.
        /// </summary>
        public Square SourceSquare { get; set; }
        
        /// <summary>
        /// Gets or sets the destination square of the move.
        /// </summary>
        public Square DestSquare { get; set; }
        
        /// <summary>
        /// Gets or sets the piece that was moved.
        /// </summary>
        public Piece MovedPiece { get; set; }
        
        /// <summary>
        /// Gets or sets the piece that was captured (null if no capture).
        /// </summary>
        public Piece? CapturedPiece { get; set; }
        
        /// <summary>
        /// Gets or sets the move number in the game.
        /// </summary>
        public int MoveNumber { get; set; }
        
        /// <summary>
        /// Gets the algebraic notation of the move (e.g., "e2e4").
        /// </summary>
        public string AlgebraicNotation => $"{SourceSquare.AlgebraicPosition}{DestSquare.AlgebraicPosition}";
        
        /// <summary>
        /// Returns a string representation of the move.
        /// </summary>
        /// <returns>A string representation of the move.</returns>
        public override string ToString()
        {
            string captureStr = CapturedPiece != null ? $" captures {CapturedPiece}" : "";
            return $"{MoveNumber}. {MovedPiece} {SourceSquare.AlgebraicPosition}-{DestSquare.AlgebraicPosition}{captureStr}";
        }
    }
}

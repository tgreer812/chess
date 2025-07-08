using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using chesslib.Pieces;
using chesslib.Controllers;
using chesslib.Handlers;

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
        /// Gets the last move made in the game, or null if no moves have been made.
        /// </summary>
        public Move? LastMove => MoveHistory.Count > 0 ? MoveHistory[MoveHistory.Count - 1] : null;
        
        /// <summary>
        /// Gets or sets the square that is available for en passant capture, or null if none.
        /// </summary>
        public Square? EnPassantCaptureSquare { get; set; }
        
        /// <summary>
        /// Gets the controller for the white player.
        /// </summary>
        public IController WhiteController { get; private set; }
        
        /// <summary>
        /// Gets the controller for the black player.
        /// </summary>
        public IController BlackController { get; private set; }
        
        /// <summary>
        /// Gets the primary game handler for move validation.
        /// </summary>
        public IGameHandler PrimaryHandler { get; private set; }
        
        /// <summary>
        /// Gets the list of additional handlers for extended validation and feedback.
        /// </summary>
        public List<IGameHandler> AdditionalHandlers { get; }
        
        /// <summary>
        /// Initializes a new instance of the Game class with a default board setup.
        /// </summary>
        public Game()
        {
            Board = new Board();
            Board.Game = this;
            CurrentTurn = PieceColor.White; // White starts
            IsGameOver = false;
            MoveHistory = new List<Move>();
            EnPassantCaptureSquare = null;
            AdditionalHandlers = new List<IGameHandler>();
            
            // Default to chess rules handler
            PrimaryHandler = new ChessRulesHandler();
            
            // Set up initial board position using the PieceFactory
            PieceFactory.SetupStandardGame(Board);
            
            // Default to human controllers
            WhiteController = new HumanController();
            BlackController = new HumanController();
        }
        
        /// <summary>
        /// Initializes a new instance of the Game class with specified controllers.
        /// </summary>
        public Game(IController whiteController, IController blackController) : this()
        {
            WhiteController = whiteController ?? throw new ArgumentNullException(nameof(whiteController));
            BlackController = blackController ?? throw new ArgumentNullException(nameof(blackController));
        }
        
        /// <summary>
        /// Initializes a new instance of the Game class with a custom board.
        /// </summary>
        public Game(Board board, PieceColor startingColor = PieceColor.White) : this()
        {
            Board = board ?? throw new ArgumentNullException(nameof(board));
            Board.Game = this;
            CurrentTurn = startingColor;
        }
        
        /// <summary>
        /// Initializes a new instance of the Game class with a custom board and controllers.
        /// </summary>
        public Game(Board board, IController whiteController, IController blackController, PieceColor startingColor = PieceColor.White) : this(board, startingColor)
        {
            WhiteController = whiteController ?? throw new ArgumentNullException(nameof(whiteController));
            BlackController = blackController ?? throw new ArgumentNullException(nameof(blackController));
        }
        
        /// <summary>
        /// Initializes a new instance of the Game class with a custom handler.
        /// </summary>
        public Game(IGameHandler primaryHandler, IController? whiteController = null, IController? blackController = null)
        {
            Board = new Board();
            Board.Game = this;
            CurrentTurn = PieceColor.White;
            IsGameOver = false;
            MoveHistory = new List<Move>();
            EnPassantCaptureSquare = null;
            AdditionalHandlers = new List<IGameHandler>();
            
            PrimaryHandler = primaryHandler ?? throw new ArgumentNullException(nameof(primaryHandler));
            
            // Set up initial board position using the PieceFactory
            PieceFactory.SetupStandardGame(Board);
            
            WhiteController = whiteController ?? new HumanController();
            BlackController = blackController ?? new HumanController();
        }
        
        /// <summary>
        /// Sets the primary game handler.
        /// </summary>
        public void SetPrimaryHandler(IGameHandler handler)
        {
            PrimaryHandler = handler ?? throw new ArgumentNullException(nameof(handler));
        }
        
        /// <summary>
        /// Adds an additional handler for extended validation and feedback.
        /// </summary>
        public void AddHandler(IGameHandler handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            AdditionalHandlers.Add(handler);
        }
        
        /// <summary>
        /// Removes an additional handler.
        /// </summary>
        public void RemoveHandler(IGameHandler handler)
        {
            AdditionalHandlers.Remove(handler);
        }
        
        /// <summary>
        /// Validates a move using all handlers and returns comprehensive feedback.
        /// </summary>
        public MoveResult ValidateMove(Square from, Square to)
        {
            // Primary handler must approve the move
            var primaryResult = PrimaryHandler.ValidateMove(this, from, to);
            if (!primaryResult.IsValid)
            {
                return primaryResult;
            }
            
            // Collect feedback from additional handlers
            var feedbacks = new List<string>();
            if (!string.IsNullOrEmpty(primaryResult.Feedback))
            {
                feedbacks.Add(primaryResult.Feedback);
            }
            
            foreach (var handler in AdditionalHandlers)
            {
                var result = handler.ValidateMove(this, from, to);
                if (!string.IsNullOrEmpty(result.Feedback))
                {
                    feedbacks.Add(result.Feedback);
                }
            }
            
            return new MoveResult
            {
                IsValid = true,
                PutsOpponentInCheck = primaryResult.PutsOpponentInCheck,
                GetsOutOfCheck = primaryResult.GetsOutOfCheck,
                Feedback = feedbacks.Count > 0 ? string.Join(" ", feedbacks) : null
            };
        }
        
        /// <summary>
        /// Attempts to make a move on the board.
        /// </summary>
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
        public bool TryMove(Square sourceSquare, Square destSquare)
        {
            // Use the primary handler to validate the move
            var result = PrimaryHandler.ValidateMove(this, sourceSquare, destSquare);
            if (!result.IsValid)
            {
                return false;
            }
            
            // Create a move record
            var move = new Move
            {
                SourceSquare = sourceSquare,
                DestSquare = destSquare,
                MovedPiece = sourceSquare.Piece!,
                CapturedPiece = destSquare.Piece,
                MoveNumber = MoveHistory.Count + 1
            };
            
            // Reset the en passant capture square by default
            Square? previousEnPassantSquare = EnPassantCaptureSquare;
            EnPassantCaptureSquare = null;
            
            // Handle special move: Castling
            if (sourceSquare.Piece is King king && Math.Abs(destSquare.Column - sourceSquare.Column) == 2)
            {
                move.IsCastling = true;
                
                // Determine the rook's source and destination positions based on the king's move
                int rookSourceCol = destSquare.Column > sourceSquare.Column ? 7 : 0; // Kingside or queenside
                int rookDestCol = destSquare.Column > sourceSquare.Column ? destSquare.Column - 1 : destSquare.Column + 1;
                
                // Get the rook and move it
                var rookSourceSquare = Board.GetSquare(sourceSquare.Row, rookSourceCol);
                var rookDestSquare = Board.GetSquare(sourceSquare.Row, rookDestCol);
                
                // Move the rook
                rookDestSquare.Piece = rookSourceSquare.Piece;
                rookSourceSquare.Piece = null;
            }
            // Handle special move: En passant capture
            else if (sourceSquare.Piece is Pawn && destSquare == previousEnPassantSquare)
            {
                move.IsEnPassantCapture = true;
                
                // Remove the captured pawn
                var capturedPawnSquare = Board.GetSquare(sourceSquare.Row, destSquare.Column);
                move.CapturedPiece = capturedPawnSquare.Piece;
                capturedPawnSquare.Piece = null;
            }
            
            // Check if this is a two-square pawn advance that enables en passant
            if (sourceSquare.Piece is Pawn && Math.Abs(destSquare.Row - sourceSquare.Row) == 2)
            {
                move.IsTwoSquarePawnAdvance = true;
                
                // Set the en passant capture square for the next move
                int enPassantRow = (sourceSquare.Row + destSquare.Row) / 2; // Middle square between source and destination
                EnPassantCaptureSquare = Board.GetSquare(enPassantRow, sourceSquare.Column);
            }
            
            // Execute the move
            destSquare.Piece = sourceSquare.Piece;
            sourceSquare.Piece = null;
            
            // Update piece's movement status if needed
            if (destSquare.Piece is Pawn pawn)
            {
                pawn.SetHasMoved();
            }
            else if (destSquare.Piece is King kingPiece)
            {
                kingPiece.SetHasMoved();
            }
            else if (destSquare.Piece is Rook rookPiece)
            {
                rookPiece.SetHasMoved();
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
        /// Checks if making a move from source to destination would leave the current player's king in check.
        /// </summary>
        /// <param name="sourceSquare">The source square.</param>
        /// <param name="destSquare">The destination square.</param>
        /// <returns>True if the move would leave the king in check, false otherwise.</returns>
        private bool WouldLeaveKingInCheck(Square sourceSquare, Square destSquare)
        {
            // Save the current state
            Piece? movingPiece = sourceSquare.Piece;
            Piece? capturedPiece = destSquare.Piece;

            // Special case for en passant capture
            Square? enPassantSquare = null;
            Piece? enPassantCapturedPawn = null;
            if (movingPiece is Pawn && destSquare == EnPassantCaptureSquare)
            {
                enPassantSquare = Board.GetSquare(sourceSquare.Row, destSquare.Column);
                enPassantCapturedPawn = enPassantSquare.Piece;
                enPassantSquare.Piece = null; // Remove the pawn being captured en passant
            }
            
            // Temporarily make the move
            destSquare.Piece = movingPiece;
            sourceSquare.Piece = null;
            
            bool kingInCheck = false;
            
            // Find the king's position
            Square? kingSquare = null;
            for (int row = 0; row < Board.BoardSize; row++)
            {
                for (int col = 0; col < Board.BoardSize; col++)
                {
                    var square = Board.GetSquare(row, col);
                    if (square.Piece is King king && king.Color == CurrentTurn)
                    {
                        kingSquare = square;
                        break;
                    }
                }
                
                if (kingSquare != null)
                    break;
            }
            
            if (kingSquare != null)
            {
                // Check if any opponent piece can attack the king
                for (int row = 0; row < Board.BoardSize && !kingInCheck; row++)
                {
                    for (int col = 0; col < Board.BoardSize && !kingInCheck; col++)
                    {
                        var square = Board.GetSquare(row, col);
                        if (square.Piece != null && square.Piece.Color != CurrentTurn)
                        {
                            // Check if this piece can attack the king
                            if (square.Piece.IsValidMove(Board, square, kingSquare))
                            {
                                kingInCheck = true;
                            }
                        }
                    }
                }
            }
            
            // Restore the original position
            sourceSquare.Piece = movingPiece;
            destSquare.Piece = capturedPiece;
            
            // Restore en passant captured pawn if needed
            if (enPassantSquare != null && enPassantCapturedPawn != null)
            {
                enPassantSquare.Piece = enPassantCapturedPawn;
            }
            
            return kingInCheck;
        }
        
        /// <summary>
        /// Returns a string representation of the game.
        /// </summary>
        /// <returns>A string representation of the game.</returns>
        public override string ToString()
        {
            return $"Current turn: {CurrentTurn}, IsGameOver: {IsGameOver}\nBoard:\n{Board}";
        }
        
        /// <summary>
        /// Asks the current player's controller for the next move and executes it.
        /// </summary>
        /// <returns>A Task that completes when the move is made, with a bool indicating if a move was successfully made.</returns>
        public async Task<bool> RequestAndExecuteNextMoveAsync()
        {
            if (IsGameOver)
                return false;
                
            // Get the current player's controller
            IController currentController = CurrentTurn == PieceColor.White ? WhiteController : BlackController;
            
            // Request a move from the controller
            var move = await currentController.GetMoveAsync(this);
            if (move == null)
                return false;
                
            // Execute the move
            return TryMove(move.Value.from, move.Value.to);
        }
        
        /// <summary>
        /// Sets the controller for a specific color.
        /// </summary>
        /// <param name="controller">The controller to set.</param>
        /// <param name="color">The color to assign the controller to.</param>
        public void SetController(IController controller, PieceColor color)
        {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));
                
            if (color == PieceColor.White)
                WhiteController = controller;
            else
                BlackController = controller;
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
        public Square SourceSquare { get; set; } = null!;
        
        /// <summary>
        /// Gets or sets the destination square of the move.
        /// </summary>
        public Square DestSquare { get; set; } = null!;
        
        /// <summary>
        /// Gets or sets the piece that was moved.
        /// </summary>
        public Piece MovedPiece { get; set; } = null!;
        
        /// <summary>
        /// Gets or sets the piece that was captured (null if no capture).
        /// </summary>
        public Piece? CapturedPiece { get; set; }
        
        /// <summary>
        /// Gets or sets the move number in the game.
        /// </summary>
        public int MoveNumber { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether this move was a two-square pawn advance.
        /// </summary>
        public bool IsTwoSquarePawnAdvance { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether this move was an en passant capture.
        /// </summary>
        public bool IsEnPassantCapture { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether this move was a castling move.
        /// </summary>
        public bool IsCastling { get; set; }
        
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

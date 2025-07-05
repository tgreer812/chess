using System;
using System.Collections.Generic;
using Xunit;
using chesslib;
using chesslib.Pieces;

namespace chesslib_test.GameTests
{
    public class GameMoveTests
    {
        [Fact]
        public void Game_TryMove_ValidMove_ReturnsTrue()
        {
            // Arrange
            var game = new Game();
            
            // Act & Assert
            // Standard opening moves
            Assert.True(game.TryMove("e2", "e4")); // White pawn e2-e4
            Assert.True(game.TryMove("e7", "e5")); // Black pawn e7-e5
            Assert.True(game.TryMove("g1", "f3")); // White knight g1-f3
        }
        
        [Fact]
        public void Game_TryMove_InvalidMove_ReturnsFalse()
        {
            // Arrange
            var game = new Game();
            
            // Act & Assert
            // Try to move a piece to an invalid position
            Assert.False(game.TryMove("e2", "e5")); // White pawn e2-e5 (too far)
            Assert.False(game.TryMove("g1", "e3")); // White knight g1-e3 (not an L-shape)
        }
        
        [Fact]
        public void Game_TryMove_WrongTurn_ReturnsFalse()
        {
            // Arrange
            var game = new Game();
            
            // Act & Assert
            // Try to move black's pieces on white's turn
            Assert.False(game.TryMove("e7", "e5")); // Black pawn on white's turn
            
            // Make a valid white move
            Assert.True(game.TryMove("e2", "e4"));
            
            // Try to move white's pieces on black's turn
            Assert.False(game.TryMove("d2", "d4")); // White pawn on black's turn
        }
        
        [Fact]
        public void Game_TryMove_EmptySquare_ReturnsFalse()
        {
            // Arrange
            var game = new Game();
            
            // Act & Assert
            // Try to move from an empty square
            Assert.False(game.TryMove("e3", "e4")); // No piece at e3
        }
        
        [Fact]
        public void Game_TryMove_UpdatesBoardCorrectly()
        {
            // Arrange
            var game = new Game();
            
            // Act
            game.TryMove("e2", "e4"); // White pawn e2-e4
            
            // Assert
            // Check that the piece moved
            var sourceSquare = game.Board.GetSquare("e2");
            var destSquare = game.Board.GetSquare("e4");
            
            Assert.Null(sourceSquare.Piece);
            Assert.NotNull(destSquare.Piece);
            Assert.IsType<Pawn>(destSquare.Piece);
            Assert.Equal(PieceColor.White, destSquare.Piece.Color);
        }
        
        [Fact]
        public void Game_TryMove_UpdatesTurnCorrectly()
        {
            // Arrange
            var game = new Game();
            
            // Assert initial state
            Assert.Equal(PieceColor.White, game.CurrentTurn);
            
            // Act
            game.TryMove("e2", "e4"); // White pawn e2-e4
            
            // Assert turn changed to black
            Assert.Equal(PieceColor.Black, game.CurrentTurn);
            
            // Act
            game.TryMove("e7", "e5"); // Black pawn e7-e5
            
            // Assert turn changed back to white
            Assert.Equal(PieceColor.White, game.CurrentTurn);
        }
        
        [Fact]
        public void Game_TryMove_Capture_UpdatesBoardCorrectly()
        {
            // Arrange
            var game = new Game();
            
            // Setup a position where a capture is possible
            game.TryMove("e2", "e4"); // White pawn e2-e4
            game.TryMove("d7", "d5"); // Black pawn d7-d5
            
            // Act - Capture the pawn
            game.TryMove("e4", "d5"); // White pawn captures black pawn
            
            // Assert
            var captureSquare = game.Board.GetSquare("d5");
            Assert.NotNull(captureSquare.Piece);
            Assert.IsType<Pawn>(captureSquare.Piece);
            Assert.Equal(PieceColor.White, captureSquare.Piece.Color);
            
            // Check move history
            Assert.Equal(3, game.MoveHistory.Count);
            Assert.NotNull(game.MoveHistory[2].CapturedPiece);
            Assert.Equal(PieceColor.Black, game.MoveHistory[2].CapturedPiece.Color);
        }
        
        [Fact]
        public void Game_IsInCheck_DetectsCheckCorrectly()
        {
            // Arrange
            var board = new Board();
            
            // Setup a position where white's king is in check
            // White king at e1
            board.GetSquare("e1").Piece = new King(PieceColor.White);
            
            // Black queen at e8
            board.GetSquare("e8").Piece = new Queen(PieceColor.Black);
            
            var game = new Game(board);
            
            // Assert
            // White king should be in check from the black queen
            Assert.True(game.IsInCheck(PieceColor.White));
            Assert.False(game.IsInCheck(PieceColor.Black));
        }
        
        [Fact]
        public void Game_TryMove_PreventsMoveIntoCheck()
        {
            // Arrange - This would require implementing the additional check in TryMove
            // This is a TODO for the Game class to prevent moves that would put the king in check
            
            // TODO: Implement test once the Game.TryMove method checks for moves that would put the king in check
        }
        
        [Fact]
        public void Game_MoveHistory_RecordsCorrectly()
        {
            // Arrange
            var game = new Game();
            
            // Act - Make some moves
            game.TryMove("e2", "e4"); // White pawn e2-e4
            game.TryMove("e7", "e5"); // Black pawn e7-e5
            game.TryMove("g1", "f3"); // White knight g1-f3
            
            // Assert
            Assert.Equal(3, game.MoveHistory.Count);
            
            // Check first move details
            var firstMove = game.MoveHistory[0];
            Assert.Equal("e2e4", firstMove.AlgebraicNotation);
            Assert.Equal(1, firstMove.MoveNumber);
            Assert.IsType<Pawn>(firstMove.MovedPiece);
            Assert.Equal(PieceColor.White, firstMove.MovedPiece.Color);
            
            // Check second move details
            var secondMove = game.MoveHistory[1];
            Assert.Equal("e7e5", secondMove.AlgebraicNotation);
            Assert.Equal(2, secondMove.MoveNumber);
            Assert.IsType<Pawn>(secondMove.MovedPiece);
            Assert.Equal(PieceColor.Black, secondMove.MovedPiece.Color);
            
            // Check third move details
            var thirdMove = game.MoveHistory[2];
            Assert.Equal("g1f3", thirdMove.AlgebraicNotation);
            Assert.Equal(3, thirdMove.MoveNumber);
            Assert.IsType<Knight>(thirdMove.MovedPiece);
            Assert.Equal(PieceColor.White, thirdMove.MovedPiece.Color);
        }
    }
}

using System;
using System.Collections.Generic;
using Xunit;
using chesslib;
using chesslib.Pieces;

namespace chesslib_test.GameTests
{
    public class AdditionalMoveTests
    {
        [Fact]
        public void King_CanMoveOutOfCheck()
        {
            // Arrange
            var board = new Board();
            
            // White king at e1
            board.GetSquare("e1").Piece = new King(PieceColor.White);
            
            // Black queen at e8 (checking the white king)
            board.GetSquare("e8").Piece = new Queen(PieceColor.Black);
            
            var game = new Game(board, PieceColor.White);
            
            // Assert king is in check
            Assert.True(game.IsInCheck(PieceColor.White));
            
            // Act - King moves out of check
            bool moveResult = game.TryMove("e1", "d1");
            
            // Assert
            Assert.True(moveResult);
            Assert.Null(board.GetSquare("e1").Piece);
            Assert.IsType<King>(board.GetSquare("d1").Piece);
            Assert.False(game.IsInCheck(PieceColor.White));
        }

        [Fact]
        public void Piece_CanCaptureCheckingPiece()
        {
            // Arrange
            var board = new Board();
            
            // White king at e1
            board.GetSquare("e1").Piece = new King(PieceColor.White);
            
            // White queen at d1
            board.GetSquare("d1").Piece = new Queen(PieceColor.White);
            
            // Black rook at e3 (checking the white king)
            board.GetSquare("e3").Piece = new Rook(PieceColor.Black);
            
            var game = new Game(board, PieceColor.White);
            
            // Assert king is in check
            Assert.True(game.IsInCheck(PieceColor.White));
            
            // Act - Queen captures the rook
            bool moveResult = game.TryMove("d1", "e3");
            
            // Assert
            Assert.True(moveResult);
            Assert.Null(board.GetSquare("d1").Piece);
            Assert.IsType<Queen>(board.GetSquare("e3").Piece);
            Assert.False(game.IsInCheck(PieceColor.White));
        }

        [Fact]
        public void Piece_CanBlockCheck()
        {
            // Arrange
            var board = new Board();
            
            // White king at e1
            board.GetSquare("e1").Piece = new King(PieceColor.White);
            
            // White bishop at c3
            board.GetSquare("c3").Piece = new Bishop(PieceColor.White);
            
            // Black rook at e8 (checking the white king)
            board.GetSquare("e8").Piece = new Rook(PieceColor.Black);
            
            var game = new Game(board, PieceColor.White);
            
            // Assert king is in check
            Assert.True(game.IsInCheck(PieceColor.White));
            
            // Act - Bishop blocks the check
            bool moveResult = game.TryMove("c3", "e5");
            
            // Assert
            Assert.True(moveResult);
            Assert.Null(board.GetSquare("c3").Piece);
            Assert.IsType<Bishop>(board.GetSquare("e5").Piece);
            Assert.False(game.IsInCheck(PieceColor.White));
        }

        [Fact]
        public void Game_TracksCheck_AfterMoveIntoCheck()
        {
            // Arrange
            var board = new Board();
            
            // White king at e1
            board.GetSquare("e1").Piece = new King(PieceColor.White);
            
            // Black queen at h4
            board.GetSquare("h4").Piece = new Queen(PieceColor.Black);
            
            var game = new Game(board, PieceColor.White);
            
            // Assert initial state - king should not be in check
            Assert.False(game.IsInCheck(PieceColor.White));
            
            // Act - King moves into check (e2)
            bool moveResult = game.TryMove("e1", "e2");
            
            // Assert - Move is legal but king is now in check
            Assert.True(moveResult);
            Assert.Null(board.GetSquare("e1").Piece);
            Assert.IsType<King>(board.GetSquare("e2").Piece);
            Assert.True(game.IsInCheck(PieceColor.White));
        }

        [Fact]
        public void Game_PinnedPiece_CanMoveAlongPinLine()
        {
            // Arrange
            var board = new Board();
            
            // White king at e1
            board.GetSquare("e1").Piece = new King(PieceColor.White);
            
            // White bishop at e2 (pinned by the rook)
            board.GetSquare("e2").Piece = new Bishop(PieceColor.White);
            
            // Black rook at e8 (pinning the bishop to the king)
            board.GetSquare("e8").Piece = new Rook(PieceColor.Black);
            
            var game = new Game(board, PieceColor.White);
            
            // Act - Bishop moves along the pin line
            bool moveResult = game.TryMove("e2", "e3");
            
            // Assert
            Assert.True(moveResult);
            Assert.Null(board.GetSquare("e2").Piece);
            Assert.IsType<Bishop>(board.GetSquare("e3").Piece);
        }

        [Fact]
        public void Game_TryMove_AllowsCapturingPinningPiece()
        {
            // Arrange
            var board = new Board();
            
            // White king at e1
            board.GetSquare("e1").Piece = new King(PieceColor.White);
            
            // White bishop at e5 (pinned by the rook, but can capture it)
            board.GetSquare("e5").Piece = new Bishop(PieceColor.White);
            
            // Black rook at e8 (pinning the bishop to the king)
            board.GetSquare("e8").Piece = new Rook(PieceColor.Black);
            
            var game = new Game(board, PieceColor.White);
            
            // Act - Bishop captures the pinning rook
            bool moveResult = game.TryMove("e5", "e8");
            
            // Assert
            Assert.True(moveResult);
            Assert.Null(board.GetSquare("e5").Piece);
            Assert.IsType<Bishop>(board.GetSquare("e8").Piece);
        }
    }
}

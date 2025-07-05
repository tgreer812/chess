using System;
using System.Collections.Generic;
using Xunit;
using chesslib;
using chesslib.Pieces;

namespace chesslib_test.PieceTests
{
    public class RookTests
    {
        [Fact]
        public void Rook_MoveHorizontally_Valid()
        {
            // Arrange
            var board = new Board();
            var whiteRook = new Rook(PieceColor.White);
            
            // Place rook on the board
            var rookSquare = board.GetSquare(7, 0); // a1
            rookSquare.Piece = whiteRook;
            
            // Act & Assert
            // Rook moving horizontally to the right
            Assert.True(whiteRook.IsValidMove(board, rookSquare, board.GetSquare(7, 3))); // a1 to d1
            Assert.True(whiteRook.IsValidMove(board, rookSquare, board.GetSquare(7, 7))); // a1 to h1
        }
        
        [Fact]
        public void Rook_MoveVertically_Valid()
        {
            // Arrange
            var board = new Board();
            var whiteRook = new Rook(PieceColor.White);
            
            // Place rook on the board
            var rookSquare = board.GetSquare(7, 0); // a1
            rookSquare.Piece = whiteRook;
            
            // Act & Assert
            // Rook moving vertically upward
            Assert.True(whiteRook.IsValidMove(board, rookSquare, board.GetSquare(4, 0))); // a1 to a4
            Assert.True(whiteRook.IsValidMove(board, rookSquare, board.GetSquare(0, 0))); // a1 to a8
        }
        
        [Fact]
        public void Rook_MoveDiagonally_Invalid()
        {
            // Arrange
            var board = new Board();
            var whiteRook = new Rook(PieceColor.White);
            
            // Place rook on the board
            var rookSquare = board.GetSquare(7, 0); // a1
            rookSquare.Piece = whiteRook;
            
            // Act & Assert
            // Rook trying to move diagonally
            Assert.False(whiteRook.IsValidMove(board, rookSquare, board.GetSquare(6, 1))); // a1 to b2
            Assert.False(whiteRook.IsValidMove(board, rookSquare, board.GetSquare(3, 4))); // a1 to e5
        }
        
        [Fact]
        public void Rook_MoveToSameSquare_Invalid()
        {
            // Arrange
            var board = new Board();
            var whiteRook = new Rook(PieceColor.White);
            
            // Place rook on the board
            var rookSquare = board.GetSquare(7, 0); // a1
            rookSquare.Piece = whiteRook;
            
            // Act & Assert
            // Rook trying to move to its own square
            Assert.False(whiteRook.IsValidMove(board, rookSquare, rookSquare));
        }
        
        [Fact]
        public void Rook_MovePastPiece_Invalid()
        {
            // Arrange
            var board = new Board();
            var whiteRook = new Rook(PieceColor.White);
            var blockingPiece = new Pawn(PieceColor.White);
            
            // Place pieces on the board
            var rookSquare = board.GetSquare(7, 0); // a1
            var blockingSquare = board.GetSquare(7, 3); // d1
            rookSquare.Piece = whiteRook;
            blockingSquare.Piece = blockingPiece;
            
            // Act & Assert
            // Rook trying to move horizontally past a blocking piece
            Assert.False(whiteRook.IsValidMove(board, rookSquare, board.GetSquare(7, 5))); // a1 to f1
            
            // Change blocking piece position for vertical test
            board.GetSquare(7, 3).Piece = null;
            board.GetSquare(4, 0).Piece = blockingPiece;
            
            // Rook trying to move vertically past a blocking piece
            Assert.False(whiteRook.IsValidMove(board, rookSquare, board.GetSquare(2, 0))); // a1 to a6
        }
        
        [Fact]
        public void Rook_CaptureOpponentPiece_Valid()
        {
            // Arrange
            var board = new Board();
            var whiteRook = new Rook(PieceColor.White);
            var blackPawn1 = new Pawn(PieceColor.Black);
            var blackPawn2 = new Pawn(PieceColor.Black);
            
            // Place pieces on the board
            var rookSquare = board.GetSquare(7, 0); // a1
            var targetSquare1 = board.GetSquare(7, 5); // f1
            var targetSquare2 = board.GetSquare(3, 0); // a5
            rookSquare.Piece = whiteRook;
            targetSquare1.Piece = blackPawn1;
            targetSquare2.Piece = blackPawn2;
            
            // Act & Assert
            // Rook capturing horizontally
            Assert.True(whiteRook.IsValidMove(board, rookSquare, targetSquare1));
            
            // Rook capturing vertically
            Assert.True(whiteRook.IsValidMove(board, rookSquare, targetSquare2));
        }
        
        [Fact]
        public void Rook_CaptureSameColorPiece_Invalid()
        {
            // Arrange
            var board = new Board();
            var whiteRook = new Rook(PieceColor.White);
            var whitePawn1 = new Pawn(PieceColor.White);
            var whitePawn2 = new Pawn(PieceColor.White);
            
            // Place pieces on the board
            var rookSquare = board.GetSquare(7, 0); // a1
            var targetSquare1 = board.GetSquare(7, 5); // f1
            var targetSquare2 = board.GetSquare(3, 0); // a5
            rookSquare.Piece = whiteRook;
            targetSquare1.Piece = whitePawn1;
            targetSquare2.Piece = whitePawn2;
            
            // Act & Assert
            // Rook trying to capture same color pieces
            Assert.False(whiteRook.IsValidMove(board, rookSquare, targetSquare1));
            Assert.False(whiteRook.IsValidMove(board, rookSquare, targetSquare2));
        }
        
        [Fact]
        public void Rook_MoveToEmptySquare_Valid()
        {
            // Arrange
            var board = new Board();
            var whiteRook = new Rook(PieceColor.White);
            
            // Place rook on the board
            var rookSquare = board.GetSquare(4, 4); // e4 (middle of the board)
            rookSquare.Piece = whiteRook;
            
            // Act & Assert
            // Rook moving to empty squares in all four directions
            Assert.True(whiteRook.IsValidMove(board, rookSquare, board.GetSquare(4, 0))); // Left
            Assert.True(whiteRook.IsValidMove(board, rookSquare, board.GetSquare(4, 7))); // Right
            Assert.True(whiteRook.IsValidMove(board, rookSquare, board.GetSquare(0, 4))); // Up
            Assert.True(whiteRook.IsValidMove(board, rookSquare, board.GetSquare(7, 4))); // Down
        }
    }
}

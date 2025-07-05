using System;
using System.Collections.Generic;
using Xunit;
using chesslib;
using chesslib.Pieces;

namespace chesslib_test.PieceTests
{
    public class BishopTests
    {
        [Fact]
        public void Bishop_MoveDiagonally_Valid()
        {
            // Arrange
            var board = new Board();
            var whiteBishop = new Bishop(PieceColor.White);
            
            // Place bishop on the board
            var bishopSquare = board.GetSquare(7, 2); // c1
            bishopSquare.Piece = whiteBishop;
            
            // Act & Assert
            // Bishop moving diagonally in all directions
            Assert.True(whiteBishop.IsValidMove(board, bishopSquare, board.GetSquare(6, 1))); // c1 to b2
            Assert.True(whiteBishop.IsValidMove(board, bishopSquare, board.GetSquare(5, 0))); // c1 to a3
            Assert.True(whiteBishop.IsValidMove(board, bishopSquare, board.GetSquare(6, 3))); // c1 to d2
            Assert.True(whiteBishop.IsValidMove(board, bishopSquare, board.GetSquare(4, 5))); // c1 to f4
        }
        
        [Fact]
        public void Bishop_MoveHorizontallyOrVertically_Invalid()
        {
            // Arrange
            var board = new Board();
            var whiteBishop = new Bishop(PieceColor.White);
            
            // Place bishop on the board
            var bishopSquare = board.GetSquare(7, 2); // c1
            bishopSquare.Piece = whiteBishop;
            
            // Act & Assert
            // Bishop trying to move horizontally or vertically
            Assert.False(whiteBishop.IsValidMove(board, bishopSquare, board.GetSquare(7, 5))); // c1 to f1 (horizontal)
            Assert.False(whiteBishop.IsValidMove(board, bishopSquare, board.GetSquare(3, 2))); // c1 to c5 (vertical)
        }
        
        [Fact]
        public void Bishop_MoveNonDiagonally_Invalid()
        {
            // Arrange
            var board = new Board();
            var whiteBishop = new Bishop(PieceColor.White);
            
            // Place bishop on the board
            var bishopSquare = board.GetSquare(4, 4); // e4
            bishopSquare.Piece = whiteBishop;
            
            // Act & Assert
            // Bishop trying to move in an L-shape or other non-diagonal pattern
            Assert.False(whiteBishop.IsValidMove(board, bishopSquare, board.GetSquare(2, 5))); // e4 to f6 (non-diagonal)
            Assert.False(whiteBishop.IsValidMove(board, bishopSquare, board.GetSquare(3, 1))); // e4 to b5 (non-diagonal)
        }
        
        [Fact]
        public void Bishop_MoveToSameSquare_Invalid()
        {
            // Arrange
            var board = new Board();
            var whiteBishop = new Bishop(PieceColor.White);
            
            // Place bishop on the board
            var bishopSquare = board.GetSquare(7, 2); // c1
            bishopSquare.Piece = whiteBishop;
            
            // Act & Assert
            // Bishop trying to move to its own square
            Assert.False(whiteBishop.IsValidMove(board, bishopSquare, bishopSquare));
        }
        
        [Fact]
        public void Bishop_MovePastPiece_Invalid()
        {
            // Arrange
            var board = new Board();
            var whiteBishop = new Bishop(PieceColor.White);
            var blockingPiece = new Pawn(PieceColor.White);
            
            // Place pieces on the board
            var bishopSquare = board.GetSquare(7, 2); // c1
            var blockingSquare = board.GetSquare(5, 4); // e3
            bishopSquare.Piece = whiteBishop;
            blockingSquare.Piece = blockingPiece;
            
            // Act & Assert
            // Bishop trying to move diagonally past a blocking piece
            Assert.False(whiteBishop.IsValidMove(board, bishopSquare, board.GetSquare(3, 6))); // c1 to g5
        }
        
        [Fact]
        public void Bishop_CaptureOpponentPiece_Valid()
        {
            // Arrange
            var board = new Board();
            var whiteBishop = new Bishop(PieceColor.White);
            var blackPawn = new Pawn(PieceColor.Black);
            
            // Place pieces on the board
            var bishopSquare = board.GetSquare(7, 2); // c1
            var targetSquare = board.GetSquare(5, 4); // e3
            bishopSquare.Piece = whiteBishop;
            targetSquare.Piece = blackPawn;
            
            // Act & Assert
            // Bishop capturing diagonally
            Assert.True(whiteBishop.IsValidMove(board, bishopSquare, targetSquare));
        }
        
        [Fact]
        public void Bishop_CaptureSameColorPiece_Invalid()
        {
            // Arrange
            var board = new Board();
            var whiteBishop = new Bishop(PieceColor.White);
            var whitePawn = new Pawn(PieceColor.White);
            
            // Place pieces on the board
            var bishopSquare = board.GetSquare(7, 2); // c1
            var targetSquare = board.GetSquare(5, 4); // e3
            bishopSquare.Piece = whiteBishop;
            targetSquare.Piece = whitePawn;
            
            // Act & Assert
            // Bishop trying to capture same color piece
            Assert.False(whiteBishop.IsValidMove(board, bishopSquare, targetSquare));
        }
        
        [Fact]
        public void Bishop_MoveToEmptySquare_Valid()
        {
            // Arrange
            var board = new Board();
            var whiteBishop = new Bishop(PieceColor.White);
            
            // Place bishop in the middle of the board
            var bishopSquare = board.GetSquare(4, 4); // e4
            bishopSquare.Piece = whiteBishop;
            
            // Act & Assert
            // Bishop moving to empty squares in all four diagonal directions
            Assert.True(whiteBishop.IsValidMove(board, bishopSquare, board.GetSquare(2, 2))); // e4 to c6 (up-left)
            Assert.True(whiteBishop.IsValidMove(board, bishopSquare, board.GetSquare(2, 6))); // e4 to g6 (up-right)
            Assert.True(whiteBishop.IsValidMove(board, bishopSquare, board.GetSquare(6, 2))); // e4 to c2 (down-left)
            Assert.True(whiteBishop.IsValidMove(board, bishopSquare, board.GetSquare(6, 6))); // e4 to g2 (down-right)
        }
    }
}

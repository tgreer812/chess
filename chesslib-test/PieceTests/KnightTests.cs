using System;
using System.Collections.Generic;
using Xunit;
using chesslib;
using chesslib.Pieces;

namespace chesslib_test.PieceTests
{
    public class KnightTests
    {
        [Fact]
        public void Knight_MoveLShape_Valid()
        {
            // Arrange
            var board = new Board();
            var whiteKnight = new Knight(PieceColor.White);
            
            // Place knight on the board
            var knightSquare = board.GetSquare(7, 1); // b1
            knightSquare.Piece = whiteKnight;
            
            // Act & Assert
            // Knight moving in L-shapes (2,1) and (1,2)
            Assert.True(whiteKnight.IsValidMove(board, knightSquare, board.GetSquare(5, 0))); // b1 to a3
            Assert.True(whiteKnight.IsValidMove(board, knightSquare, board.GetSquare(5, 2))); // b1 to c3
            Assert.True(whiteKnight.IsValidMove(board, knightSquare, board.GetSquare(6, 3))); // b1 to d2
        }
        
        [Fact]
        public void Knight_MoveFromCenterBoard_Valid()
        {
            // Arrange
            var board = new Board();
            var whiteKnight = new Knight(PieceColor.White);
            
            // Place knight in the middle of the board
            var knightSquare = board.GetSquare(4, 4); // e4
            knightSquare.Piece = whiteKnight;
            
            // Act & Assert
            // Knight moving in all eight possible L-shape directions from the center
            Assert.True(whiteKnight.IsValidMove(board, knightSquare, board.GetSquare(2, 3))); // e4 to d6
            Assert.True(whiteKnight.IsValidMove(board, knightSquare, board.GetSquare(2, 5))); // e4 to f6
            Assert.True(whiteKnight.IsValidMove(board, knightSquare, board.GetSquare(3, 2))); // e4 to c5
            Assert.True(whiteKnight.IsValidMove(board, knightSquare, board.GetSquare(3, 6))); // e4 to g5
            Assert.True(whiteKnight.IsValidMove(board, knightSquare, board.GetSquare(5, 2))); // e4 to c3
            Assert.True(whiteKnight.IsValidMove(board, knightSquare, board.GetSquare(5, 6))); // e4 to g3
            Assert.True(whiteKnight.IsValidMove(board, knightSquare, board.GetSquare(6, 3))); // e4 to d2
            Assert.True(whiteKnight.IsValidMove(board, knightSquare, board.GetSquare(6, 5))); // e4 to f2
        }
        
        [Fact]
        public void Knight_MoveHorizontallyOrVertically_Invalid()
        {
            // Arrange
            var board = new Board();
            var whiteKnight = new Knight(PieceColor.White);
            
            // Place knight on the board
            var knightSquare = board.GetSquare(7, 1); // b1
            knightSquare.Piece = whiteKnight;
            
            // Act & Assert
            // Knight trying to move horizontally or vertically
            Assert.False(whiteKnight.IsValidMove(board, knightSquare, board.GetSquare(7, 3))); // b1 to d1 (horizontal)
            Assert.False(whiteKnight.IsValidMove(board, knightSquare, board.GetSquare(5, 1))); // b1 to b3 (vertical)
        }
        
        [Fact]
        public void Knight_MoveDiagonally_Invalid()
        {
            // Arrange
            var board = new Board();
            var whiteKnight = new Knight(PieceColor.White);
            
            // Place knight on the board
            var knightSquare = board.GetSquare(7, 1); // b1
            knightSquare.Piece = whiteKnight;
            
            // Act & Assert
            // Knight trying to move diagonally
            Assert.False(whiteKnight.IsValidMove(board, knightSquare, board.GetSquare(6, 0))); // b1 to a2 (diagonal)
            Assert.False(whiteKnight.IsValidMove(board, knightSquare, board.GetSquare(5, 3))); // b1 to d3 (diagonal)
        }
        
        [Fact]
        public void Knight_MoveToSameSquare_Invalid()
        {
            // Arrange
            var board = new Board();
            var whiteKnight = new Knight(PieceColor.White);
            
            // Place knight on the board
            var knightSquare = board.GetSquare(7, 1); // b1
            knightSquare.Piece = whiteKnight;
            
            // Act & Assert
            // Knight trying to move to its own square
            Assert.False(whiteKnight.IsValidMove(board, knightSquare, knightSquare));
        }
        
        [Fact]
        public void Knight_JumpOverPieces_Valid()
        {
            // Arrange
            var board = new Board();
            var whiteKnight = new Knight(PieceColor.White);
            var blockingPiece1 = new Pawn(PieceColor.White);
            var blockingPiece2 = new Pawn(PieceColor.Black);
            
            // Place pieces on the board
            var knightSquare = board.GetSquare(7, 1); // b1
            var blockingSquare1 = board.GetSquare(6, 1); // b2
            var blockingSquare2 = board.GetSquare(6, 0); // a2
            knightSquare.Piece = whiteKnight;
            blockingSquare1.Piece = blockingPiece1;
            blockingSquare2.Piece = blockingPiece2;
            
            // Act & Assert
            // Knight should be able to jump over other pieces
            Assert.True(whiteKnight.IsValidMove(board, knightSquare, board.GetSquare(5, 0))); // b1 to a3 (jumping over a2)
            Assert.True(whiteKnight.IsValidMove(board, knightSquare, board.GetSquare(5, 2))); // b1 to c3 (jumping over b2)
        }
        
        [Fact]
        public void Knight_CaptureOpponentPiece_Valid()
        {
            // Arrange
            var board = new Board();
            var whiteKnight = new Knight(PieceColor.White);
            var blackPawn = new Pawn(PieceColor.Black);
            
            // Place pieces on the board
            var knightSquare = board.GetSquare(7, 1); // b1
            var targetSquare = board.GetSquare(5, 2); // c3
            knightSquare.Piece = whiteKnight;
            targetSquare.Piece = blackPawn;
            
            // Act & Assert
            // Knight capturing opponent's piece
            Assert.True(whiteKnight.IsValidMove(board, knightSquare, targetSquare));
        }
        
        [Fact]
        public void Knight_CaptureSameColorPiece_Invalid()
        {
            // Arrange
            var board = new Board();
            var whiteKnight = new Knight(PieceColor.White);
            var whitePawn = new Pawn(PieceColor.White);
            
            // Place pieces on the board
            var knightSquare = board.GetSquare(7, 1); // b1
            var targetSquare = board.GetSquare(5, 2); // c3
            knightSquare.Piece = whiteKnight;
            targetSquare.Piece = whitePawn;
            
            // Act & Assert
            // Knight trying to capture same color piece
            Assert.False(whiteKnight.IsValidMove(board, knightSquare, targetSquare));
        }
    }
}

using System;
using System.Collections.Generic;
using Xunit;
using chesslib;
using chesslib.Pieces;

namespace chesslib_test.PieceTests
{
    public class QueenTests
    {
        [Fact]
        public void Queen_MoveHorizontally_Valid()
        {
            // Arrange
            var board = new Board();
            var whiteQueen = new Queen(PieceColor.White);
            
            // Place queen on the board
            var queenSquare = board.GetSquare(7, 3); // d1
            queenSquare.Piece = whiteQueen;
            
            // Act & Assert
            // Queen moving horizontally in both directions
            Assert.True(whiteQueen.IsValidMove(board, queenSquare, board.GetSquare(7, 0))); // d1 to a1
            Assert.True(whiteQueen.IsValidMove(board, queenSquare, board.GetSquare(7, 7))); // d1 to h1
        }
        
        [Fact]
        public void Queen_MoveVertically_Valid()
        {
            // Arrange
            var board = new Board();
            var whiteQueen = new Queen(PieceColor.White);
            
            // Place queen on the board
            var queenSquare = board.GetSquare(7, 3); // d1
            queenSquare.Piece = whiteQueen;
            
            // Act & Assert
            // Queen moving vertically
            Assert.True(whiteQueen.IsValidMove(board, queenSquare, board.GetSquare(0, 3))); // d1 to d8
            Assert.True(whiteQueen.IsValidMove(board, queenSquare, board.GetSquare(4, 3))); // d1 to d4
        }
        
        [Fact]
        public void Queen_MoveDiagonally_Valid()
        {
            // Arrange
            var board = new Board();
            var whiteQueen = new Queen(PieceColor.White);
            
            // Place queen on the board
            var queenSquare = board.GetSquare(7, 3); // d1
            queenSquare.Piece = whiteQueen;
            
            // Act & Assert
            // Queen moving diagonally
            Assert.True(whiteQueen.IsValidMove(board, queenSquare, board.GetSquare(4, 0))); // d1 to a4
            Assert.True(whiteQueen.IsValidMove(board, queenSquare, board.GetSquare(3, 7))); // d1 to h5
        }
        
        [Fact]
        public void Queen_MoveNonStraightLine_Invalid()
        {
            // Arrange
            var board = new Board();
            var whiteQueen = new Queen(PieceColor.White);
            
            // Place queen on the board
            var queenSquare = board.GetSquare(7, 3); // d1
            queenSquare.Piece = whiteQueen;
            
            // Act & Assert
            // Queen trying to move in an L-shape or other non-straight line pattern
            Assert.False(whiteQueen.IsValidMove(board, queenSquare, board.GetSquare(5, 4))); // d1 to e3 (non-straight)
            Assert.False(whiteQueen.IsValidMove(board, queenSquare, board.GetSquare(6, 0))); // d1 to a2 (non-straight)
        }
        
        [Fact]
        public void Queen_MoveToSameSquare_Invalid()
        {
            // Arrange
            var board = new Board();
            var whiteQueen = new Queen(PieceColor.White);
            
            // Place queen on the board
            var queenSquare = board.GetSquare(7, 3); // d1
            queenSquare.Piece = whiteQueen;
            
            // Act & Assert
            // Queen trying to move to its own square
            Assert.False(whiteQueen.IsValidMove(board, queenSquare, queenSquare));
        }
        
        [Fact]
        public void Queen_MovePastPiece_Invalid()
        {
            // Arrange
            var board = new Board();
            var whiteQueen = new Queen(PieceColor.White);
            var blockingPiece = new Pawn(PieceColor.White);
            
            // Place pieces on the board
            var queenSquare = board.GetSquare(7, 3); // d1
            var blockingSquare = board.GetSquare(5, 3); // d3
            queenSquare.Piece = whiteQueen;
            blockingSquare.Piece = blockingPiece;
            
            // Act & Assert
            // Queen trying to move vertically past a blocking piece
            Assert.False(whiteQueen.IsValidMove(board, queenSquare, board.GetSquare(2, 3))); // d1 to d6
            
            // Reposition for horizontal test
            board.GetSquare(5, 3).Piece = null;
            board.GetSquare(7, 5).Piece = blockingPiece;
            
            // Queen trying to move horizontally past a blocking piece
            Assert.False(whiteQueen.IsValidMove(board, queenSquare, board.GetSquare(7, 7))); // d1 to h1
            
            // Reposition for diagonal test
            board.GetSquare(7, 5).Piece = null;
            board.GetSquare(5, 5).Piece = blockingPiece;
            
            // Queen trying to move diagonally past a blocking piece
            Assert.False(whiteQueen.IsValidMove(board, queenSquare, board.GetSquare(3, 7))); // d1 to h5
        }
        
        [Fact]
        public void Queen_CaptureOpponentPiece_Valid()
        {
            // Arrange
            var board = new Board();
            var whiteQueen = new Queen(PieceColor.White);
            var blackPawn1 = new Pawn(PieceColor.Black);
            var blackPawn2 = new Pawn(PieceColor.Black);
            var blackPawn3 = new Pawn(PieceColor.Black);
            
            // Place pieces on the board
            var queenSquare = board.GetSquare(7, 3); // d1
            var targetSquare1 = board.GetSquare(7, 6); // g1 (horizontal)
            var targetSquare2 = board.GetSquare(3, 3); // d5 (vertical)
            var targetSquare3 = board.GetSquare(5, 5); // f3 (diagonal)
            queenSquare.Piece = whiteQueen;
            targetSquare1.Piece = blackPawn1;
            targetSquare2.Piece = blackPawn2;
            targetSquare3.Piece = blackPawn3;
            
            // Act & Assert
            // Queen capturing horizontally, vertically, and diagonally
            Assert.True(whiteQueen.IsValidMove(board, queenSquare, targetSquare1));
            Assert.True(whiteQueen.IsValidMove(board, queenSquare, targetSquare2));
            Assert.True(whiteQueen.IsValidMove(board, queenSquare, targetSquare3));
        }
        
        [Fact]
        public void Queen_CaptureSameColorPiece_Invalid()
        {
            // Arrange
            var board = new Board();
            var whiteQueen = new Queen(PieceColor.White);
            var whitePawn = new Pawn(PieceColor.White);
            
            // Place pieces on the board
            var queenSquare = board.GetSquare(7, 3); // d1
            var targetSquare = board.GetSquare(5, 3); // d3
            queenSquare.Piece = whiteQueen;
            targetSquare.Piece = whitePawn;
            
            // Act & Assert
            // Queen trying to capture same color piece
            Assert.False(whiteQueen.IsValidMove(board, queenSquare, targetSquare));
        }
        
        [Fact]
        public void Queen_MoveFromCenterBoard_Valid()
        {
            // Arrange
            var board = new Board();
            var whiteQueen = new Queen(PieceColor.White);
            
            // Place queen in the middle of the board
            var queenSquare = board.GetSquare(4, 4); // e4
            queenSquare.Piece = whiteQueen;
            
            // Act & Assert
            // Queen moving in all eight directions from the center
            Assert.True(whiteQueen.IsValidMove(board, queenSquare, board.GetSquare(4, 0))); // e4 to a4 (left)
            Assert.True(whiteQueen.IsValidMove(board, queenSquare, board.GetSquare(4, 7))); // e4 to h4 (right)
            Assert.True(whiteQueen.IsValidMove(board, queenSquare, board.GetSquare(0, 4))); // e4 to e8 (up)
            Assert.True(whiteQueen.IsValidMove(board, queenSquare, board.GetSquare(7, 4))); // e4 to e1 (down)
            Assert.True(whiteQueen.IsValidMove(board, queenSquare, board.GetSquare(1, 1))); // e4 to b7 (up-left)
            Assert.True(whiteQueen.IsValidMove(board, queenSquare, board.GetSquare(1, 7))); // e4 to h7 (up-right)
            Assert.True(whiteQueen.IsValidMove(board, queenSquare, board.GetSquare(7, 1))); // e4 to b1 (down-left)
            Assert.True(whiteQueen.IsValidMove(board, queenSquare, board.GetSquare(7, 7))); // e4 to h1 (down-right)
        }
    }
}

using System;
using System.Collections.Generic;
using Xunit;
using chesslib;
using chesslib.Pieces;

namespace chesslib_test.PieceTests
{
    public class KingTests
    {
        [Fact]
        public void King_MoveOneSquare_AllDirections_Valid()
        {
            // Arrange
            var board = new Board();
            var whiteKing = new King(PieceColor.White);
            
            // Place king on the board in a position where all moves are possible
            var kingSquare = board.GetSquare(4, 4); // e4
            kingSquare.Piece = whiteKing;
            
            // Act & Assert
            // King moving one square in all eight directions
            Assert.True(whiteKing.IsValidMove(board, kingSquare, board.GetSquare(3, 3))); // e4 to d5 (up-left)
            Assert.True(whiteKing.IsValidMove(board, kingSquare, board.GetSquare(3, 4))); // e4 to e5 (up)
            Assert.True(whiteKing.IsValidMove(board, kingSquare, board.GetSquare(3, 5))); // e4 to f5 (up-right)
            Assert.True(whiteKing.IsValidMove(board, kingSquare, board.GetSquare(4, 3))); // e4 to d4 (left)
            Assert.True(whiteKing.IsValidMove(board, kingSquare, board.GetSquare(4, 5))); // e4 to f4 (right)
            Assert.True(whiteKing.IsValidMove(board, kingSquare, board.GetSquare(5, 3))); // e4 to d3 (down-left)
            Assert.True(whiteKing.IsValidMove(board, kingSquare, board.GetSquare(5, 4))); // e4 to e3 (down)
            Assert.True(whiteKing.IsValidMove(board, kingSquare, board.GetSquare(5, 5))); // e4 to f3 (down-right)
        }
        
        [Fact]
        public void King_MoveMoreThanOneSquare_Invalid()
        {
            // Arrange
            var board = new Board();
            var whiteKing = new King(PieceColor.White);
            
            // Place king on the board
            var kingSquare = board.GetSquare(7, 4); // e1
            kingSquare.Piece = whiteKing;
            
            // Act & Assert
            // King trying to move more than one square
            Assert.False(whiteKing.IsValidMove(board, kingSquare, board.GetSquare(5, 4))); // e1 to e3 (two squares up)
            Assert.False(whiteKing.IsValidMove(board, kingSquare, board.GetSquare(7, 6))); // e1 to g1 (two squares right)
            Assert.False(whiteKing.IsValidMove(board, kingSquare, board.GetSquare(5, 6))); // e1 to g3 (diagonal two squares)
        }
        
        [Fact]
        public void King_MoveToSameSquare_Invalid()
        {
            // Arrange
            var board = new Board();
            var whiteKing = new King(PieceColor.White);
            
            // Place king on the board
            var kingSquare = board.GetSquare(7, 4); // e1
            kingSquare.Piece = whiteKing;
            
            // Act & Assert
            // King trying to move to its own square
            Assert.False(whiteKing.IsValidMove(board, kingSquare, kingSquare));
        }
        
        [Fact]
        public void King_CaptureOpponentPiece_Valid()
        {
            // Arrange
            var board = new Board();
            var whiteKing = new King(PieceColor.White);
            var blackPawn = new Pawn(PieceColor.Black);
            
            // Place pieces on the board
            var kingSquare = board.GetSquare(7, 4); // e1
            var targetSquare = board.GetSquare(6, 4); // e2
            kingSquare.Piece = whiteKing;
            targetSquare.Piece = blackPawn;
            
            // Act & Assert
            // King capturing opponent's piece
            Assert.True(whiteKing.IsValidMove(board, kingSquare, targetSquare));
        }
        
        [Fact]
        public void King_CaptureSameColorPiece_Invalid()
        {
            // Arrange
            var board = new Board();
            var whiteKing = new King(PieceColor.White);
            var whitePawn = new Pawn(PieceColor.White);
            
            // Place pieces on the board
            var kingSquare = board.GetSquare(7, 4); // e1
            var targetSquare = board.GetSquare(6, 4); // e2
            kingSquare.Piece = whiteKing;
            targetSquare.Piece = whitePawn;
            
            // Act & Assert
            // King trying to capture same color piece
            Assert.False(whiteKing.IsValidMove(board, kingSquare, targetSquare));
        }
        
        [Fact]
        public void King_MoveFromCorner_Valid()
        {
            // Arrange
            var board = new Board();
            var whiteKing = new King(PieceColor.White);
            
            // Place king in the corner
            var kingSquare = board.GetSquare(7, 0); // a1
            kingSquare.Piece = whiteKing;
            
            // Act & Assert
            // King moving from corner (only three valid directions)
            Assert.True(whiteKing.IsValidMove(board, kingSquare, board.GetSquare(6, 0))); // a1 to a2 (up)
            Assert.True(whiteKing.IsValidMove(board, kingSquare, board.GetSquare(6, 1))); // a1 to b2 (up-right)
            Assert.True(whiteKing.IsValidMove(board, kingSquare, board.GetSquare(7, 1))); // a1 to b1 (right)
        }
        
        [Fact]
        public void King_HasMoved_TracksCorrectly()
        {
            // Arrange
            var king = new King(PieceColor.White);
            
            // Assert - Initial state
            Assert.False(king.HasMoved);
            
            // Act - Set hasMoved
            king.SetHasMoved();
            
            // Assert - After moving
            Assert.True(king.HasMoved);
        }
        
        // Castling tests
        [Fact]
        public void King_CastlingKingside_Valid_WhiteKing()
        {
            // Arrange
            var board = new Board();
            var whiteKing = new King(PieceColor.White);
            var whiteRook = new Rook(PieceColor.White);
            
            // Place king and rook in initial positions
            var kingSquare = board.GetSquare(7, 4); // e1
            var rookSquare = board.GetSquare(7, 7); // h1
            kingSquare.Piece = whiteKing;
            rookSquare.Piece = whiteRook;
            
            // Make sure squares between are empty
            board.GetSquare(7, 5).Piece = null; // f1
            board.GetSquare(7, 6).Piece = null; // g1
            
            // Act & Assert
            // King castling kingside (e1 to g1)
            Assert.True(whiteKing.IsValidMove(board, kingSquare, board.GetSquare(7, 6))); // This will fail until castling is implemented
        }
        
        [Fact]
        public void King_CastlingKingside_Valid_BlackKing()
        {
            // Arrange
            var board = new Board();
            var blackKing = new King(PieceColor.Black);
            var blackRook = new Rook(PieceColor.Black);
            
            // Place king and rook in initial positions
            var kingSquare = board.GetSquare(0, 4); // e8
            var rookSquare = board.GetSquare(0, 7); // h8
            kingSquare.Piece = blackKing;
            rookSquare.Piece = blackRook;
            
            // Make sure squares between are empty
            board.GetSquare(0, 5).Piece = null; // f8
            board.GetSquare(0, 6).Piece = null; // g8
            
            // Act & Assert
            // King castling kingside (e8 to g8)
            Assert.True(blackKing.IsValidMove(board, kingSquare, board.GetSquare(0, 6))); // This will fail until castling is implemented
        }
        
        [Fact]
        public void King_CastlingQueenside_Valid_WhiteKing()
        {
            // Arrange
            var board = new Board();
            var whiteKing = new King(PieceColor.White);
            var whiteRook = new Rook(PieceColor.White);
            
            // Place king and rook in initial positions
            var kingSquare = board.GetSquare(7, 4); // e1
            var rookSquare = board.GetSquare(7, 0); // a1
            kingSquare.Piece = whiteKing;
            rookSquare.Piece = whiteRook;
            
            // Make sure squares between are empty
            board.GetSquare(7, 1).Piece = null; // b1
            board.GetSquare(7, 2).Piece = null; // c1
            board.GetSquare(7, 3).Piece = null; // d1
            
            // Act & Assert
            // King castling queenside (e1 to c1)
            Assert.True(whiteKing.IsValidMove(board, kingSquare, board.GetSquare(7, 2))); // This will fail until castling is implemented
        }
        
        [Fact]
        public void King_CastlingQueenside_Valid_BlackKing()
        {
            // Arrange
            var board = new Board();
            var blackKing = new King(PieceColor.Black);
            var blackRook = new Rook(PieceColor.Black);
            
            // Place king and rook in initial positions
            var kingSquare = board.GetSquare(0, 4); // e8
            var rookSquare = board.GetSquare(0, 0); // a8
            kingSquare.Piece = blackKing;
            rookSquare.Piece = blackRook;
            
            // Make sure squares between are empty
            board.GetSquare(0, 1).Piece = null; // b8
            board.GetSquare(0, 2).Piece = null; // c8
            board.GetSquare(0, 3).Piece = null; // d8
            
            // Act & Assert
            // King castling queenside (e8 to c8)
            Assert.True(blackKing.IsValidMove(board, kingSquare, board.GetSquare(0, 2))); // This will fail until castling is implemented
        }
        
        [Fact]
        public void King_Castling_Invalid_KingHasMoved()
        {
            // Arrange
            var board = new Board();
            var whiteKing = new King(PieceColor.White);
            var whiteRook = new Rook(PieceColor.White);
            
            // Place king and rook in initial positions
            var kingSquare = board.GetSquare(7, 4); // e1
            var rookSquare = board.GetSquare(7, 7); // h1
            kingSquare.Piece = whiteKing;
            rookSquare.Piece = whiteRook;
            
            // Make sure squares between are empty
            board.GetSquare(7, 5).Piece = null; // f1
            board.GetSquare(7, 6).Piece = null; // g1
            
            // King has already moved
            whiteKing.SetHasMoved();
            
            // Act & Assert
            // King castling kingside should be invalid because king has moved
            Assert.False(whiteKing.IsValidMove(board, kingSquare, board.GetSquare(7, 6)));
        }
        
        [Fact]
        public void King_Castling_Invalid_PieceInBetween()
        {
            // Arrange
            var board = new Board();
            var whiteKing = new King(PieceColor.White);
            var whiteRook = new Rook(PieceColor.White);
            var whiteKnight = new Knight(PieceColor.White);
            
            // Place king and rook in initial positions
            var kingSquare = board.GetSquare(7, 4); // e1
            var rookSquare = board.GetSquare(7, 7); // h1
            kingSquare.Piece = whiteKing;
            rookSquare.Piece = whiteRook;
            
            // Place a piece between king and rook
            board.GetSquare(7, 5).Piece = null; // f1
            board.GetSquare(7, 6).Piece = whiteKnight; // g1 has a knight
            
            // Act & Assert
            // King castling kingside should be invalid because there's a piece in the way
            Assert.False(whiteKing.IsValidMove(board, kingSquare, board.GetSquare(7, 6)));
        }
        
        [Fact]
        public void King_Castling_Invalid_KingInCheck()
        {
            // Arrange
            var board = new Board();
            var whiteKing = new King(PieceColor.White);
            var whiteRook = new Rook(PieceColor.White);
            var blackRook = new Rook(PieceColor.Black);
            
            // Place king and rook in initial positions
            var kingSquare = board.GetSquare(7, 4); // e1
            var rookSquare = board.GetSquare(7, 7); // h1
            kingSquare.Piece = whiteKing;
            rookSquare.Piece = whiteRook;
            
            // Make sure squares between are empty
            board.GetSquare(7, 5).Piece = null; // f1
            board.GetSquare(7, 6).Piece = null; // g1
            
            // Place enemy rook attacking the king
            board.GetSquare(0, 4).Piece = blackRook; // e8 - putting king in check
            
            // Create a Game instance to check if the king is in check
            var game = new Game(board);
            
            // Act & Assert
            // Check that the king is indeed in check
            Assert.True(game.IsInCheck(PieceColor.White));
            
            // King castling kingside should be invalid because king is in check
            Assert.False(whiteKing.IsValidMove(board, kingSquare, board.GetSquare(7, 6)));
        }
        
        [Fact]
        public void King_Castling_Invalid_KingPassesThroughCheck()
        {
            // Arrange
            var board = new Board();
            var game = new Game(board);
            var whiteKing = new King(PieceColor.White);
            var whiteRook = new Rook(PieceColor.White);
            var blackRook = new Rook(PieceColor.Black);
            
            // Place king and rook in initial positions
            var kingSquare = board.GetSquare(7, 4); // e1
            var rookSquare = board.GetSquare(7, 7); // h1
            kingSquare.Piece = whiteKing;
            rookSquare.Piece = whiteRook;
            
            // Make sure squares between are empty
            board.GetSquare(7, 5).Piece = null; // f1
            board.GetSquare(7, 6).Piece = null; // g1
            
            // Place enemy rook attacking f1 (king must pass through)
            board.GetSquare(0, 5).Piece = blackRook; // f8 - attacking f1
            
            // Act & Assert
            // King castling kingside should be invalid because king passes through check
            Assert.False(whiteKing.IsValidMove(board, kingSquare, board.GetSquare(7, 6)));
        }
        
        [Fact]
        public void King_Castling_Invalid_RookHasMoved()
        {
            // Arrange
            var board = new Board();
            var whiteKing = new King(PieceColor.White);
            var whiteRook = new Rook(PieceColor.White);
            
            // Place king and rook in initial positions
            var kingSquare = board.GetSquare(7, 4); // e1
            var rookSquare = board.GetSquare(7, 7); // h1
            kingSquare.Piece = whiteKing;
            rookSquare.Piece = whiteRook;
            
            // Make sure squares between are empty
            board.GetSquare(7, 5).Piece = null; // f1
            board.GetSquare(7, 6).Piece = null; // g1
            
            // Simulate rook having moved
            whiteRook.SetHasMoved();
            
            // Act & Assert
            // King castling kingside should be invalid because rook has moved
            Assert.False(whiteKing.IsValidMove(board, kingSquare, board.GetSquare(7, 6)));
        }
        
        [Fact] 
        public void Game_CastlingKingside_MovesRookCorrectly()
        {
            // Arrange
            var board = new Board();
            var whiteKing = new King(PieceColor.White);
            var whiteRook = new Rook(PieceColor.White);
            
            // Place king and rook in initial positions
            var kingSquare = board.GetSquare(7, 4); // e1
            var rookSquare = board.GetSquare(7, 7); // h1
            kingSquare.Piece = whiteKing;
            rookSquare.Piece = whiteRook;
            
            // Make sure squares between are empty
            board.GetSquare(7, 5).Piece = null; // f1
            board.GetSquare(7, 6).Piece = null; // g1
            
            var game = new Game(board);
            
            // Act
            // Try to castle kingside
            bool moveResult = game.TryMove(kingSquare, board.GetSquare(7, 6)); // e1 to g1
            
            // Assert
            // This should fail until castling is implemented
            Assert.True(moveResult);
            
            // Check king's new position
            Assert.Equal(whiteKing, board.GetSquare(7, 6).Piece); // King at g1
            
            // Check rook's new position
            Assert.Equal(whiteRook, board.GetSquare(7, 5).Piece); // Rook at f1
        }
        
        [Fact] 
        public void Game_CastlingQueenside_MovesRookCorrectly()
        {
            // Arrange
            var board = new Board();
            var whiteKing = new King(PieceColor.White);
            var whiteRook = new Rook(PieceColor.White);
            
            // Place king and rook in initial positions
            var kingSquare = board.GetSquare(7, 4); // e1
            var rookSquare = board.GetSquare(7, 0); // a1
            kingSquare.Piece = whiteKing;
            rookSquare.Piece = whiteRook;
            
            // Make sure squares between are empty
            board.GetSquare(7, 1).Piece = null; // b1
            board.GetSquare(7, 2).Piece = null; // c1
            board.GetSquare(7, 3).Piece = null; // d1
            
            var game = new Game(board);
            
            // Act
            // Try to castle queenside
            bool moveResult = game.TryMove(kingSquare, board.GetSquare(7, 2)); // e1 to c1
            
            // Assert
            // This should fail until castling is implemented
            Assert.True(moveResult);
            
            // Check king's new position
            Assert.Equal(whiteKing, board.GetSquare(7, 2).Piece); // King at c1
            
            // Check rook's new position
            Assert.Equal(whiteRook, board.GetSquare(7, 3).Piece); // Rook at d1
        }
    }
}

using System;
using System.Collections.Generic;
using Xunit;
using chesslib;
using chesslib.Pieces;

namespace chesslib_test.PieceTests
{
    public class PawnTests
    {
        [Fact]
        public void Pawn_MoveOneSquare_Forward_Valid()
        {
            // Arrange
            var board = new Board();
            var whitePawn = new Pawn(PieceColor.White);
            var blackPawn = new Pawn(PieceColor.Black);
            
            // Place pawns on the board
            var whiteStartSquare = board.GetSquare(6, 3); // d2
            var blackStartSquare = board.GetSquare(1, 3); // d7
            whiteStartSquare.Piece = whitePawn;
            blackStartSquare.Piece = blackPawn;
            
            // Act & Assert
            // White pawn moving up (decreasing row)
            Assert.True(whitePawn.IsValidMove(board, whiteStartSquare, board.GetSquare(5, 3)));
            
            // Black pawn moving down (increasing row)
            Assert.True(blackPawn.IsValidMove(board, blackStartSquare, board.GetSquare(2, 3)));
        }
        
        [Fact]
        public void Pawn_MoveTwoSquares_FirstMove_Valid()
        {
            // Arrange
            var board = new Board();
            var whitePawn = new Pawn(PieceColor.White);
            var blackPawn = new Pawn(PieceColor.Black);
            
            // Place pawns on the board
            var whiteStartSquare = board.GetSquare(6, 3); // d2
            var blackStartSquare = board.GetSquare(1, 3); // d7
            whiteStartSquare.Piece = whitePawn;
            blackStartSquare.Piece = blackPawn;
            
            // Act & Assert
            // White pawn moving up two squares on first move
            Assert.True(whitePawn.IsValidMove(board, whiteStartSquare, board.GetSquare(4, 3)));
            
            // Black pawn moving down two squares on first move
            Assert.True(blackPawn.IsValidMove(board, blackStartSquare, board.GetSquare(3, 3)));
        }
        
        [Fact]
        public void Pawn_MoveTwoSquares_AfterFirstMove_Invalid()
        {
            // Arrange
            var board = new Board();
            var whitePawn = new Pawn(PieceColor.White);
            var blackPawn = new Pawn(PieceColor.Black);
            
            // Place pawns on the board
            var whiteStartSquare = board.GetSquare(6, 3); // d2
            var blackStartSquare = board.GetSquare(1, 3); // d7
            whiteStartSquare.Piece = whitePawn;
            blackStartSquare.Piece = blackPawn;
            
            // Make the first move to change hasMoved status
            whitePawn.SetHasMoved();
            blackPawn.SetHasMoved();
            
            // Act & Assert
            // Attempting to move two squares after the first move
            Assert.False(whitePawn.IsValidMove(board, whiteStartSquare, board.GetSquare(4, 3)));
            Assert.False(blackPawn.IsValidMove(board, blackStartSquare, board.GetSquare(3, 3)));
        }
        
        [Fact]
        public void Pawn_MoveForward_Blocked_Invalid()
        {
            // Arrange
            var board = new Board();
            var whitePawn = new Pawn(PieceColor.White);
            var blockingPiece = new Pawn(PieceColor.Black);
            
            // Place pieces on the board
            var whiteStartSquare = board.GetSquare(6, 3); // d2
            var blockingSquare = board.GetSquare(5, 3); // d3
            whiteStartSquare.Piece = whitePawn;
            blockingSquare.Piece = blockingPiece;
            
            // Act & Assert
            // White pawn trying to move to a square occupied by another piece
            Assert.False(whitePawn.IsValidMove(board, whiteStartSquare, blockingSquare));
            
            // White pawn trying to move two squares with a piece blocking the path
            Assert.False(whitePawn.IsValidMove(board, whiteStartSquare, board.GetSquare(4, 3)));
        }
        
        [Fact]
        public void Pawn_MoveBackward_Invalid()
        {
            // Arrange
            var board = new Board();
            var whitePawn = new Pawn(PieceColor.White);
            var blackPawn = new Pawn(PieceColor.Black);
            
            // Place pawns on the board in advanced positions
            var whiteSquare = board.GetSquare(4, 3); // d4
            var blackSquare = board.GetSquare(3, 3); // d5
            whiteSquare.Piece = whitePawn;
            blackSquare.Piece = blackPawn;
            
            // Act & Assert
            // White pawn trying to move backward
            Assert.False(whitePawn.IsValidMove(board, whiteSquare, board.GetSquare(5, 3)));
            
            // Black pawn trying to move backward
            Assert.False(blackPawn.IsValidMove(board, blackSquare, board.GetSquare(2, 3)));
        }
        
        [Fact]
        public void Pawn_MoveSideways_Invalid()
        {
            // Arrange
            var board = new Board();
            var whitePawn = new Pawn(PieceColor.White);
            
            // Place pawn on the board
            var whiteSquare = board.GetSquare(6, 3); // d2
            whiteSquare.Piece = whitePawn;
            
            // Act & Assert
            // White pawn trying to move sideways
            Assert.False(whitePawn.IsValidMove(board, whiteSquare, board.GetSquare(6, 4)));
            Assert.False(whitePawn.IsValidMove(board, whiteSquare, board.GetSquare(6, 2)));
        }
        
        [Fact]
        public void Pawn_MoveDiagonally_NoCapture_Invalid()
        {
            // Arrange
            var board = new Board();
            var whitePawn = new Pawn(PieceColor.White);
            
            // Place pawn on the board
            var whiteSquare = board.GetSquare(6, 3); // d2
            whiteSquare.Piece = whitePawn;
            
            // Act & Assert
            // White pawn trying to move diagonally with no piece to capture
            Assert.False(whitePawn.IsValidMove(board, whiteSquare, board.GetSquare(5, 2)));
            Assert.False(whitePawn.IsValidMove(board, whiteSquare, board.GetSquare(5, 4)));
        }
        
        [Fact]
        public void Pawn_MoveDiagonally_Capture_Valid()
        {
            // Arrange
            var board = new Board();
            var whitePawn = new Pawn(PieceColor.White);
            var blackPawn1 = new Pawn(PieceColor.Black);
            var blackPawn2 = new Pawn(PieceColor.Black);
            
            // Place pawns on the board
            var whiteSquare = board.GetSquare(6, 3); // d2
            var blackSquare1 = board.GetSquare(5, 2); // c3
            var blackSquare2 = board.GetSquare(5, 4); // e3
            whiteSquare.Piece = whitePawn;
            blackSquare1.Piece = blackPawn1;
            blackSquare2.Piece = blackPawn2;
            
            // Act & Assert
            // White pawn capturing diagonally
            Assert.True(whitePawn.IsValidMove(board, whiteSquare, blackSquare1));
            Assert.True(whitePawn.IsValidMove(board, whiteSquare, blackSquare2));
        }
        
        [Fact]
        public void Pawn_MoveDiagonally_CaptureSameColor_Invalid()
        {
            // Arrange
            var board = new Board();
            var whitePawn1 = new Pawn(PieceColor.White);
            var whitePawn2 = new Pawn(PieceColor.White);
            var whitePawn3 = new Pawn(PieceColor.White);
            
            // Place pawns on the board
            var whiteSquare1 = board.GetSquare(6, 3); // d2
            var whiteSquare2 = board.GetSquare(5, 2); // c3
            var whiteSquare3 = board.GetSquare(5, 4); // e3
            whiteSquare1.Piece = whitePawn1;
            whiteSquare2.Piece = whitePawn2;
            whiteSquare3.Piece = whitePawn3;
            
            // Act & Assert
            // White pawn trying to capture its own color
            Assert.False(whitePawn1.IsValidMove(board, whiteSquare1, whiteSquare2));
            Assert.False(whitePawn1.IsValidMove(board, whiteSquare1, whiteSquare3));
        }
        
        [Fact]
        public void Pawn_EnPassant_Capture_Valid()
        {
            // Arrange
            var board = new Board();
            var game = new Game(board);
            var whitePawn = new Pawn(PieceColor.White);
            var blackPawn = new Pawn(PieceColor.Black);
            
            // Place pawns on the board to simulate an en passant scenario
            // White pawn has moved to the 5th rank
            var whiteSquare = board.GetSquare(3, 4); // e5
            // Black pawn is adjacent and just moved two squares in the previous turn
            var blackSquare = board.GetSquare(3, 5); // f5
            whiteSquare.Piece = whitePawn;
            blackSquare.Piece = blackPawn;
            
            // Simulate that the black pawn just moved two squares
            // Set the en passant capture square
            var captureSquare = board.GetSquare(2, 5); // f6
            game.EnPassantCaptureSquare = captureSquare;
            
            // Act & Assert
            // White pawn should be able to capture the black pawn en passant
            Assert.True(whitePawn.IsValidMove(board, whiteSquare, captureSquare));
            
            // Now the en passant capture should not be valid if reset
            game.EnPassantCaptureSquare = null;
            Assert.False(whitePawn.IsValidMove(board, whiteSquare, captureSquare));
        }
        
        [Fact]
        public void Pawn_EnPassant_WrongRank_Invalid()
        {
            // Arrange
            var board = new Board();
            var whitePawn = new Pawn(PieceColor.White);
            var blackPawn = new Pawn(PieceColor.Black);
            
            // Place pawns on the board in a position that's NOT valid for en passant
            // White pawn is not on the 5th rank (for white)
            var whiteSquare = board.GetSquare(4, 4); // e4
            // Black pawn is adjacent
            var blackSquare = board.GetSquare(4, 5); // f4
            whiteSquare.Piece = whitePawn;
            blackSquare.Piece = blackPawn;
            
            // Simulate that the black pawn just moved two squares
            // blackPawn.LastMoveWasTwoSquareAdvance = true;
            
            // The attempted en passant capture destination
            var captureSquare = board.GetSquare(3, 5); // f5
            
            // Act & Assert
            // White pawn should not be able to capture en passant if not on the correct rank
            Assert.False(whitePawn.IsValidMove(board, whiteSquare, captureSquare));
        }
        
        [Fact]
        public void Pawn_EnPassant_BlackPawn_Valid()
        {
            // Arrange
            var board = new Board();
            var game = new Game(board);
            var whitePawn = new Pawn(PieceColor.White);
            var blackPawn = new Pawn(PieceColor.Black);
            
            // Place pawns on the board to simulate an en passant scenario for black
            // Black pawn has moved to the 4th rank
            var blackSquare = board.GetSquare(4, 4); // e4
            // White pawn is adjacent and just moved two squares in the previous turn
            var whiteSquare = board.GetSquare(4, 3); // d4
            blackSquare.Piece = blackPawn;
            whiteSquare.Piece = whitePawn;
            
            // Simulate that the white pawn just moved two squares
            // Set the en passant capture square
            var captureSquare = board.GetSquare(5, 3); // d3
            game.EnPassantCaptureSquare = captureSquare;
            
            // Act & Assert
            // Black pawn should be able to capture the white pawn en passant
            Assert.True(blackPawn.IsValidMove(board, blackSquare, captureSquare));
        }
        
        [Fact]
        public void Pawn_EnPassant_PawnNotAdjacent_Invalid()
        {
            // Arrange
            var board = new Board();
            var whitePawn = new Pawn(PieceColor.White);
            var blackPawn = new Pawn(PieceColor.Black);
            
            // Place pawns on the board where they are not adjacent
            var whiteSquare = board.GetSquare(3, 2); // c5
            var blackSquare = board.GetSquare(3, 5); // f5
            whiteSquare.Piece = whitePawn;
            blackSquare.Piece = blackPawn;
            
            // Simulate that the black pawn just moved two squares
            // blackPawn.LastMoveWasTwoSquareAdvance = true;
            
            // The attempted en passant capture destination
            var captureSquare = board.GetSquare(2, 5); // f6
            
            // Act & Assert
            // White pawn should not be able to capture en passant if not adjacent
            Assert.False(whitePawn.IsValidMove(board, whiteSquare, captureSquare));
        }
        
        [Fact]
        public void Pawn_EnPassant_NoPawnToCapture_Invalid()
        {
            // Arrange
            var board = new Board();
            var whitePawn = new Pawn(PieceColor.White);
            
            // Place white pawn on the board with no adjacent black pawn
            var whiteSquare = board.GetSquare(3, 4); // e5
            whiteSquare.Piece = whitePawn;
            
            // The attempted en passant capture destination with no pawn to capture
            var captureSquare = board.GetSquare(2, 5); // f6
            
            // Act & Assert
            // White pawn should not be able to capture en passant if there's no pawn to capture
            Assert.False(whitePawn.IsValidMove(board, whiteSquare, captureSquare));
        }
    }
}

using System;
using System.Collections.Generic;
using Xunit;
using chesslib;

namespace chesslib_test
{
    public class BoardTests
    {
        [Fact]
        public void Board_DefaultConstructor_CreatesEmptyBoard()
        {
            // Arrange & Act
            var board = new Board();
            
            // Assert
            Assert.NotNull(board);
            Assert.Equal(Board.BoardSize, board.Squares.Count);
            
            for (int row = 0; row < Board.BoardSize; row++)
            {
                Assert.Equal(Board.BoardSize, board.Squares[row].Count);
                
                for (int col = 0; col < Board.BoardSize; col++)
                {
                    var square = board.GetSquare(row, col);
                    Assert.Equal(row, square.Row);
                    Assert.Equal(col, square.Column);
                    Assert.Null(square.Piece);
                    Assert.Equal((row + col) % 2 == 0 ? SquareColor.Light : SquareColor.Dark, square.Color);
                }
            }
        }
        
        [Fact]
        public void Board_CustomConstructor_CreatesSpecificBoard()
        {
            // Arrange
            var squares = new List<List<Square>>();
            
            for (int row = 0; row < Board.BoardSize; row++)
            {
                var rowList = new List<Square>();
                
                for (int col = 0; col < Board.BoardSize; col++)
                {
                    // Create a new square with its position and color (using opposite coloring for testing)
                    bool isLight = (row + col) % 2 != 0; // Opposite of standard coloring
                    rowList.Add(new Square(row, col, isLight ? SquareColor.Light : SquareColor.Dark));
                }
                
                squares.Add(rowList);
            }
            
            // Act
            var board = new Board(squares);
            
            // Assert
            Assert.NotNull(board);
            Assert.Equal(Board.BoardSize, board.Squares.Count);
            
            for (int row = 0; row < Board.BoardSize; row++)
            {
                Assert.Equal(Board.BoardSize, board.Squares[row].Count);
                
                for (int col = 0; col < Board.BoardSize; col++)
                {
                    var square = board.GetSquare(row, col);
                    Assert.Equal(row, square.Row);
                    Assert.Equal(col, square.Column);
                    Assert.Null(square.Piece);
                    // Verify our custom coloring pattern (opposite of standard)
                    Assert.Equal((row + col) % 2 != 0 ? SquareColor.Light : SquareColor.Dark, square.Color);
                }
            }
        }
        
        [Fact]
        public void GetSquare_ValidPosition_ReturnsCorrectSquare()
        {
            // Arrange
            var board = new Board();
            
            // Act & Assert
            var square = board.GetSquare(0, 0);
            Assert.Equal(0, square.Row);
            Assert.Equal(0, square.Column);
            Assert.Equal(SquareColor.Light, square.Color);
            Assert.Equal("a8", square.AlgebraicPosition);
            
            square = board.GetSquare(7, 7);
            Assert.Equal(7, square.Row);
            Assert.Equal(7, square.Column);
            Assert.Equal(SquareColor.Light, square.Color);
            Assert.Equal("h1", square.AlgebraicPosition);
        }
        
        [Fact]
        public void GetSquare_ValidAlgebraicPosition_ReturnsCorrectSquare()
        {
            // Arrange
            var board = new Board();
            
            // Act & Assert
            var square = board.GetSquare("a8");
            Assert.Equal(0, square.Row);
            Assert.Equal(0, square.Column);
            Assert.Equal(SquareColor.Light, square.Color);
            
            square = board.GetSquare("h1");
            Assert.Equal(7, square.Row);
            Assert.Equal(7, square.Column);
            Assert.Equal(SquareColor.Light, square.Color);
        }
        
        [Fact]
        public void GetSquare_InvalidPosition_ThrowsException()
        {
            // Arrange
            var board = new Board();
            
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => board.GetSquare(-1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => board.GetSquare(0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => board.GetSquare(8, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => board.GetSquare(0, 8));
        }
        
        [Fact]
        public void GetSquare_InvalidAlgebraicPosition_ThrowsException()
        {
            // Arrange
            var board = new Board();
            
            // Act & Assert
            Assert.Throws<ArgumentException>(() => board.GetSquare(""));
            Assert.Throws<ArgumentException>(() => board.GetSquare("a"));
            Assert.Throws<ArgumentException>(() => board.GetSquare("a0"));
            Assert.Throws<ArgumentException>(() => board.GetSquare("a9"));
            Assert.Throws<ArgumentException>(() => board.GetSquare("i1"));
        }
        
        [Fact]
        public void Board_CustomConstructor_WithInvalidDimensions_ThrowsException()
        {
            // Arrange
            var tooFewRows = new List<List<Square>>();
            for (int i = 0; i < Board.BoardSize - 1; i++)
            {
                tooFewRows.Add(new List<Square>());
                for (int j = 0; j < Board.BoardSize; j++)
                {
                    tooFewRows[i].Add(new Square(i, j, SquareColor.Light));
                }
            }
            
            var wrongColumnCount = new List<List<Square>>();
            for (int i = 0; i < Board.BoardSize; i++)
            {
                wrongColumnCount.Add(new List<Square>());
                for (int j = 0; j < (i == Board.BoardSize - 1 ? Board.BoardSize - 1 : Board.BoardSize); j++)
                {
                    wrongColumnCount[i].Add(new Square(i, j, SquareColor.Light));
                }
            }
            
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Board(null));
            Assert.Throws<ArgumentException>(() => new Board(tooFewRows));
            Assert.Throws<ArgumentException>(() => new Board(wrongColumnCount));
        }
    }
}

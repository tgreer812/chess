using System;
using System.Collections.Generic;
using chesslib;
using chesslib.Pieces;

namespace ChessConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // To display chess piece symbols properly
            
            Console.WriteLine("Chess Library Demo");
            Console.WriteLine("-----------------");
            
            DemoStandardGame();
            
            DemoCustomGame();
            
            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();
        }
        
        static void DemoStandardGame()
        {
            Console.WriteLine("\n=== STANDARD CHESS GAME ===");
            
            // Create a standard game with all pieces in starting position
            var game = new Game();
            Console.WriteLine("\nInitial game state (standard setup):");
            Console.WriteLine(game.ToString());
            
            // Try some legal pawn moves
            Console.WriteLine("\nTrying a legal pawn move (e2-e4):");
            bool moveResult = game.TryMove("e2", "e4");
            Console.WriteLine($"Move success: {moveResult}");
            Console.WriteLine(game.ToString());
            
            // Try another legal pawn move for black
            Console.WriteLine("\nTrying a legal pawn move for black (e7-e5):");
            moveResult = game.TryMove("e7", "e5");
            Console.WriteLine($"Move success: {moveResult}");
            Console.WriteLine(game.ToString());
            
            // Try a knight move
            Console.WriteLine("\nTrying a knight move (g1-f3):");
            moveResult = game.TryMove("g1", "f3");
            Console.WriteLine($"Move success: {moveResult}");
            Console.WriteLine(game.ToString());
        }
        
        static void DemoCustomGame()
        {
            Console.WriteLine("\n=== CUSTOM CHESS GAME ===");
            
            // Create a custom board with various pieces
            var customSquares = CreateCustomBoard();
            var customBoard = new Board(customSquares);
            Console.WriteLine("\nCustom board setup:");
            Console.WriteLine(customBoard.ToString());
            
            // Create a game with the custom board
            var game = new Game(customBoard);
            
            // Demonstrate a rook move
            Console.WriteLine("\nTrying a legal rook move (a1-a5):");
            bool moveResult = game.TryMove("a1", "a5");
            Console.WriteLine($"Move success: {moveResult}");
            Console.WriteLine(game.ToString());
            
            // Demonstrate a bishop move
            Console.WriteLine("\nTrying a legal bishop move (c1-a3):");
            moveResult = game.TryMove("c1", "a3");
            Console.WriteLine($"Move success: {moveResult}");
            Console.WriteLine(game.ToString());
            
            // Demonstrate a queen move
            Console.WriteLine("\nTrying a legal queen move (d8-h4):");
            moveResult = game.TryMove("d8", "h4");
            Console.WriteLine($"Move success: {moveResult}");
            Console.WriteLine(game.ToString());
            
            // Check if white is in check
            Console.WriteLine($"\nIs white in check? {game.IsInCheck(PieceColor.White)}");
        }
        
        static List<List<Square>> CreateCustomBoard()
        {
            var squares = new List<List<Square>>();
            
            for (int row = 0; row < Board.BoardSize; row++)
            {
                var rowList = new List<Square>();
                
                for (int col = 0; col < Board.BoardSize; col++)
                {
                    bool isLight = (row + col) % 2 == 0;
                    var square = new Square(row, col, isLight ? SquareColor.Light : SquareColor.Dark);
                    
                    // Add pieces to specific positions for demonstration
                    if (row == 7 && col == 0) // a1
                    {
                        square.Piece = PieceFactory.CreatePiece(PieceFactory.PieceType.Rook, PieceColor.White);
                    }
                    else if (row == 7 && col == 2) // c1
                    {
                        square.Piece = PieceFactory.CreatePiece(PieceFactory.PieceType.Bishop, PieceColor.White);
                    }
                    else if (row == 7 && col == 4) // e1
                    {
                        square.Piece = PieceFactory.CreatePiece(PieceFactory.PieceType.King, PieceColor.White);
                    }
                    else if (row == 0 && col == 3) // d8
                    {
                        square.Piece = PieceFactory.CreatePiece(PieceFactory.PieceType.Queen, PieceColor.Black);
                    }
                    
                    rowList.Add(square);
                }
                
                squares.Add(rowList);
            }
            
            return squares;
        }
    }
}

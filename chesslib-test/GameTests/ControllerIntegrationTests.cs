using System;
using System.Threading.Tasks;
using chesslib;
using chesslib.Controllers;
using Xunit;

namespace chesslib_test.GameTests
{
    public class ControllerIntegrationTests
    {
        /// <summary>
        /// Tests a complete game played through controllers.
        /// </summary>
        [Fact]
        public async Task Game_PlaysCompleteGameWithControllers()
        {
            // Arrange - Create controllers with predetermined moves (Scholar's Mate)
            var whiteMoves = new[]
            {
                ("e2", "e4"),  // 1. e4
                ("d1", "h5"),  // 2. Qh5
                ("f1", "c4"),  // 3. Bc4
                ("h5", "f7")   // 4. Qxf7# (Checkmate)
            };
            
            var blackMoves = new[]
            {
                ("e7", "e5"),  // 1. ... e5
                ("b8", "c6"),  // 2. ... Nc6
                ("g8", "f6")   // 3. ... Nf6
            };
            
            var whiteController = new TestHelpers.TestController(whiteMoves);
            var blackController = new TestHelpers.TestController(blackMoves);
            
            var game = new Game(whiteController, blackController);
            
            // Act & Assert - Play through the game
            for (int i = 0; i < 7; i++)
            {
                if (i < 6) // All moves except the last should succeed
                {
                    bool moveResult = await game.RequestAndExecuteNextMoveAsync();
                    Assert.True(moveResult, $"Move {i + 1} should succeed");
                }
                else // The last move (Qxf7#) should end the game
                {
                    bool moveResult = await game.RequestAndExecuteNextMoveAsync();
                    Assert.True(moveResult, "The final checkmate move should succeed");
                    // Here we would check for checkmate, once that's implemented
                    // Assert.True(game.IsGameOver);
                }
            }
            
            // Verify the final board state
            var f7Square = game.Board.GetSquare("f7");
            Assert.NotNull(f7Square.Piece);
            Assert.Equal(PieceColor.White, f7Square.Piece.Color);
            Assert.Equal("Queen", f7Square.Piece.GetType().Name);
        }
        
        /// <summary>
        /// Tests that a HumanController and RandomController work together properly.
        /// </summary>
        [Fact]
        public async Task Game_HumanAndRandomControllerInteraction()
        {
            // Arrange
            var humanController = new HumanController();
            var randomController = new RandomController();
            
            var game = new Game(humanController, randomController);
            
            // Act - Human makes a move
            Task<bool> moveTask = game.RequestAndExecuteNextMoveAsync();
            
            // The move task shouldn't complete yet (waiting for human input)
            Assert.False(moveTask.IsCompleted);
            
            // Provide the human move
            humanController.SetNextMove("e2", "e4");
            
            // Wait for the move to complete
            bool humanMoveResult = await moveTask;
            
            // Assert - Human move succeeded
            Assert.True(humanMoveResult);
            Assert.Equal(PieceColor.Black, game.CurrentTurn);
            
            // Act - Random controller makes a move
            bool randomMoveResult = await game.RequestAndExecuteNextMoveAsync();
            
            // Assert - Random move succeeded
            Assert.True(randomMoveResult);
            Assert.Equal(PieceColor.White, game.CurrentTurn);
            Assert.True(game.MoveHistory.Count >= 2);
        }
    }
}

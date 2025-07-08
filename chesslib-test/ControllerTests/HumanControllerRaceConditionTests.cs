using System;
using System.Threading.Tasks;
using Xunit;
using chesslib;
using chesslib.Controllers;

namespace chesslib_test
{
    public class HumanControllerRaceConditionTests
    {
        /// <summary>
        /// Tests the race condition that occurs when SetNextMove is called before GetMoveAsync.
        /// This reproduces the issue where users need to click multiple times on the Test page.
        /// </summary>
        [Fact]
        public async Task HumanController_SetNextMoveBeforeGetMoveAsync_ShouldNotIgnoreMove()
        {
            // Arrange
            var humanController = new HumanController();
            var game = new Game();
            
            // Simulate the race condition: SetNextMove called before GetMoveAsync
            // This is what happens in the current ChessBoard.SquareClicked method
            humanController.SetNextMove("e2", "e4");
            
            // Act - Now call GetMoveAsync (this is what RequestAndExecuteNextMoveAsync does)
            var move = await humanController.GetMoveAsync(game);
            
            // Assert - The move should not be null (currently fails due to race condition)
            Assert.NotNull(move);
            Assert.Equal("e2", move.Value.from);
            Assert.Equal("e4", move.Value.to);
        }
        
        /// <summary>
        /// Tests the correct flow where GetMoveAsync is called first, then SetNextMove.
        /// This should work properly.
        /// </summary>
        [Fact]
        public async Task HumanController_GetMoveAsyncThenSetNextMove_ShouldWork()
        {
            // Arrange
            var humanController = new HumanController();
            var game = new Game();
            
            // Act - Start waiting for move first
            var moveTask = humanController.GetMoveAsync(game);
            
            // Then provide the move
            humanController.SetNextMove("e2", "e4");
            
            // Wait for the move to be processed
            var move = await moveTask;
            
            // Assert
            Assert.NotNull(move);
            Assert.Equal("e2", move.Value.from);
            Assert.Equal("e4", move.Value.to);
        }
        
        /// <summary>
        /// Tests multiple SetNextMove calls before GetMoveAsync to simulate rapid clicking.
        /// </summary>
        [Fact]
        public async Task HumanController_MultipleSetNextMoveBeforeGetMoveAsync_ShouldHandleGracefully()
        {
            // Arrange
            var humanController = new HumanController();
            var game = new Game();
            
            // Simulate multiple rapid clicks (SetNextMove calls)
            humanController.SetNextMove("e2", "e4");
            humanController.SetNextMove("d2", "d4");
            humanController.SetNextMove("c2", "c4");
            
            // Act - Now call GetMoveAsync
            var move = await humanController.GetMoveAsync(game);
            
            // Assert - Should get at least one of the moves, not null
            Assert.NotNull(move);
            // In current implementation, this will likely be null, demonstrating the bug
        }
    }
}
using System;
using System.Threading.Tasks;
using chesslib;
using chesslib.Controllers;
using Xunit;
using Moq;

namespace chesslib_test
{
    public class ControllerTests
    {
        /// <summary>
        /// Tests that the game correctly requests moves from controllers.
        /// </summary>
        [Fact]
        public async Task Game_RequestAndExecuteNextMoveAsync_UsesController()
        {
            // Arrange
            var mockController = new Mock<IController>();
            mockController
                .Setup(c => c.GetMoveAsync(It.IsAny<Game>()))
                .ReturnsAsync(("e2", "e4"));  // Standard pawn opening move

            var game = new Game(mockController.Object, mockController.Object);

            // Act
            bool moveResult = await game.RequestAndExecuteNextMoveAsync();

            // Assert
            Assert.True(moveResult);
            mockController.Verify(c => c.GetMoveAsync(game), Times.Once);
            
            // Verify the move was actually executed on the board
            var destinationSquare = game.Board.GetSquare("e4");
            Assert.NotNull(destinationSquare.Piece);
            Assert.Equal(PieceColor.White, destinationSquare.Piece.Color);
        }

        /// <summary>
        /// Tests that the game handles controllers returning invalid moves.
        /// </summary>
        [Fact]
        public async Task Game_RequestAndExecuteNextMoveAsync_HandlesInvalidMove()
        {
            // Arrange
            var mockController = new Mock<IController>();
            mockController
                .Setup(c => c.GetMoveAsync(It.IsAny<Game>()))
                .ReturnsAsync(("e2", "e5"));  // Invalid pawn move (can't move 3 squares)

            var game = new Game(mockController.Object, mockController.Object);

            // Act
            bool moveResult = await game.RequestAndExecuteNextMoveAsync();

            // Assert
            Assert.False(moveResult);
            mockController.Verify(c => c.GetMoveAsync(game), Times.Once);
            
            // Verify the move was NOT executed on the board
            var sourceSquare = game.Board.GetSquare("e2");
            var destinationSquare = game.Board.GetSquare("e5");
            
            Assert.NotNull(sourceSquare.Piece); // Pawn should still be at e2
            Assert.Null(destinationSquare.Piece); // e5 should be empty
        }

        /// <summary>
        /// Tests that the game switches controllers based on the current turn.
        /// </summary>
        [Fact]
        public async Task Game_RequestAndExecuteNextMoveAsync_SwitchesControllers()
        {
            // Arrange
            var mockWhiteController = new Mock<IController>();
            mockWhiteController
                .Setup(c => c.GetMoveAsync(It.IsAny<Game>()))
                .ReturnsAsync(("e2", "e4"));

            var mockBlackController = new Mock<IController>();
            mockBlackController
                .Setup(c => c.GetMoveAsync(It.IsAny<Game>()))
                .ReturnsAsync(("e7", "e5"));

            var game = new Game(mockWhiteController.Object, mockBlackController.Object);

            // Act - First move (White)
            bool firstMoveResult = await game.RequestAndExecuteNextMoveAsync();
            
            // Assert - First move
            Assert.True(firstMoveResult);
            mockWhiteController.Verify(c => c.GetMoveAsync(game), Times.Once);
            mockBlackController.Verify(c => c.GetMoveAsync(game), Times.Never);
            Assert.Equal(PieceColor.Black, game.CurrentTurn);

            // Act - Second move (Black)
            bool secondMoveResult = await game.RequestAndExecuteNextMoveAsync();
            
            // Assert - Second move
            Assert.True(secondMoveResult);
            mockWhiteController.Verify(c => c.GetMoveAsync(game), Times.Once);
            mockBlackController.Verify(c => c.GetMoveAsync(game), Times.Once);
            Assert.Equal(PieceColor.White, game.CurrentTurn);
        }

        /// <summary>
        /// Tests that the HumanController properly accepts and returns a move.
        /// </summary>
        [Fact]
        public async Task HumanController_GetMoveAsync_ReturnsSetMove()
        {
            // Arrange
            var humanController = new HumanController();
            var game = new Game();
            
            // Start a task to get the move
            var moveTask = humanController.GetMoveAsync(game);
            
            // The task should not be completed yet since no move has been set
            Assert.False(moveTask.IsCompleted);
            
            // Act - Set a move
            humanController.SetNextMove("e2", "e4");
            
            // Wait for the task to complete
            var move = await moveTask;
            
            // Assert
            Assert.NotNull(move);
            Assert.Equal("e2", move.Value.from);
            Assert.Equal("e4", move.Value.to);
        }

        /// <summary>
        /// Tests that the HumanController handles move cancellation correctly.
        /// </summary>
        [Fact]
        public async Task HumanController_CancelMoveRequest_ReturnsNull()
        {
            // Arrange
            var humanController = new HumanController();
            var game = new Game();
            
            // Start a task to get the move
            var moveTask = humanController.GetMoveAsync(game);
            
            // Act - Cancel the move request
            humanController.CancelMoveRequest();
            
            // Wait for the task to complete
            var move = await moveTask;
            
            // Assert
            Assert.Null(move);
        }

        /// <summary>
        /// Tests that the HumanController's IsWaitingForMove property works correctly.
        /// </summary>
        [Fact]
        public async void HumanController_IsWaitingForMove_ReflectsState()
        {
            // Arrange
            var humanController = new HumanController();
            var game = new Game();
            
            // Initially not waiting for a move
            Assert.False(humanController.IsWaitingForMove);
            
            // Act - Start waiting for a move
            var moveTask = humanController.GetMoveAsync(game);
            
            // Assert - Now waiting for a move
            Assert.True(humanController.IsWaitingForMove);
            
            // Act - Provide a move
            humanController.SetNextMove("e2", "e4");

            // Complete the task
            await moveTask;
            
            // Assert - No longer waiting for a move
            Assert.False(humanController.IsWaitingForMove);
        }

        /// <summary>
        /// Tests that the HumanController ignores moves with non-matching IDs.
        /// </summary>
        [Fact]
        public async Task HumanController_SetNextMove_IgnoresNonMatchingIds()
        {
            // Arrange
            var humanController = new HumanController();
            var game = new Game();
            
            // Start a task to get the move
            var moveTask = humanController.GetMoveAsync(game);
            
            // The task should not be completed yet
            Assert.False(moveTask.IsCompleted);
            
            // Act - Try to set a move with an incorrect ID
            humanController.SetNextMove("e2", "e4", 999); // Using an incorrect move ID
            
            // The task should still not be completed
            Assert.False(moveTask.IsCompleted);
            
            // Now set the move without an ID (which should work)
            humanController.SetNextMove("d2", "d4");
            
            // Wait for the task to complete
            var move = await moveTask;
            
            // Assert - Should get the correct move
            Assert.NotNull(move);
            Assert.Equal("d2", move.Value.from);
            Assert.Equal("d4", move.Value.to);
        }

        /// <summary>
        /// Tests that the RandomController returns a valid move.
        /// </summary>
        [Fact]
        public async Task RandomController_GetMoveAsync_ReturnsValidMove()
        {
            // Arrange
            var randomController = new RandomController();
            var game = new Game(); // Standard game setup
            
            // Act
            var move = await randomController.GetMoveAsync(game);
            
            // Assert
            Assert.NotNull(move);
            
            // Check that the source square contains a piece of the current turn's color
            var sourceSquare = game.Board.GetSquare(move.Value.from);
            Assert.NotNull(sourceSquare.Piece);
            Assert.Equal(game.CurrentTurn, sourceSquare.Piece.Color);
            
            // Verify the move is actually valid by attempting it
            bool moveResult = game.TryMove(move.Value.from, move.Value.to);
            Assert.True(moveResult);
        }

        /// <summary>
        /// Tests that the game correctly requests moves from controllers.
        /// </summary>
        [Fact]
        public async Task Game_RequestAndExecuteNextMoveAsync_CallsController()
        {
            // Arrange
            var game = new Game();
            var mockController = new TestHelpers.TestController(("e2", "e4"));
            game.SetController(mockController, PieceColor.White);
            
            // Act
            bool result = await game.RequestAndExecuteNextMoveAsync();
            
            // Assert
            Assert.True(result);
            Assert.Equal(PieceColor.Black, game.CurrentTurn); // Turn should have changed
            Assert.Single(game.MoveHistory);
            Assert.Equal("e2e4", game.MoveHistory[0].AlgebraicNotation);
        }
    }
}

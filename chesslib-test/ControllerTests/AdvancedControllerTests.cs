using System;
using System.Threading.Tasks;
using chesslib;
using chesslib.Controllers;
using chesslib.Pieces;
using Xunit;
using Moq;

namespace chesslib_test
{
    public class AdvancedControllerTests
    {
        /// <summary>
        /// Tests that a controller can handle multiple consecutive move requests correctly.
        /// </summary>
        [Fact]
        public async Task HumanController_MultipleMoveRequests_HandlesCorrectly()
        {
            // Arrange
            var humanController = new HumanController();
            var game = new Game();
            
            // Start a task to get the move but don't await it yet
            var moveTask1 = humanController.GetMoveAsync(game);
            
            // Set the first move
            humanController.SetNextMove("e2", "e4");
            
            // Get the result
            var result1 = await moveTask1;
            
            // Request a second move
            var moveTask2 = humanController.GetMoveAsync(game);
            
            // Set the second move
            humanController.SetNextMove("d2", "d4");
            
            // Get the second result
            var result2 = await moveTask2;
            
            // Assert
            Assert.Equal("e2", result1!.Value.from);
            Assert.Equal("e4", result1!.Value.to);
            Assert.Equal("d2", result2!.Value.from);
            Assert.Equal("d4", result2!.Value.to);
        }

        /// <summary>
        /// Tests that a controller can be canceled and then used again.
        /// </summary>
        [Fact]
        public async Task HumanController_CancelAndReuse_WorksCorrectly()
        {
            // Arrange
            var humanController = new HumanController();
            var game = new Game();
            
            // Start a task to get the move
            var moveTask1 = humanController.GetMoveAsync(game);
            
            // Cancel the move request
            humanController.CancelMoveRequest();
            
            // Complete the task
            var result1 = await moveTask1;
            
            // Request a new move after cancellation
            var moveTask2 = humanController.GetMoveAsync(game);
            
            // Set the move
            humanController.SetNextMove("e2", "e4");
            
            // Complete the task
            var result2 = await moveTask2;
            
            // Assert
            Assert.Null(result1);
            Assert.NotNull(result2);
            Assert.Equal("e2", result2!.Value.from);
            Assert.Equal("e4", result2!.Value.to);
        }

        /// <summary>
        /// Tests that sequential move requests work correctly.
        /// </summary>
        [Fact]
        public async Task HumanController_SequentialMoveRequests_WorksCorrectly()
        {
            // Arrange
            var humanController = new HumanController();
            var game = new Game();
            
            // Start a task to get the first move
            var moveTask1 = humanController.GetMoveAsync(game);
            
            // Set the first move
            humanController.SetNextMove("e2", "e4");
            
            // Complete the task
            var result1 = await moveTask1;
            
            // Request a second move
            var moveTask2 = humanController.GetMoveAsync(game);
            
            // Set the second move
            humanController.SetNextMove("d2", "d4");
            
            // Complete the task
            var result2 = await moveTask2;
            
            // Assert
            Assert.Equal("e2", result1!.Value.from);
            Assert.Equal("e4", result1!.Value.to);
            Assert.Equal("d2", result2!.Value.from);
            Assert.Equal("d4", result2!.Value.to);
        }

        /// <summary>
        /// Tests that RandomController consistently returns valid moves in a variety of board positions.
        /// </summary>
        [Fact]
        public async Task RandomController_VariousBoardPositions_ReturnsValidMoves()
        {
            // Test with standard opening position
            await TestRandomControllerWithPosition("standard");
            
            // Test with a complex middlegame position
            await TestRandomControllerWithPosition("r1bqkbnr/pp1ppppp/2n5/2p5/4P3/5N2/PPPP1PPP/RNBQKB1R");
            
            // Test with a position where few moves are available
            await TestRandomControllerWithPosition("8/8/8/4k3/8/8/4K3/8");
        }
        
        private async Task TestRandomControllerWithPosition(string position)
        {
            // Arrange
            var controller = new RandomController();
            Board board;
            Game game;
            
            if (position == "standard")
            {
                game = new Game();
            }
            else
            {
                board = new Board();
                TestHelpers.SetupBoardFromFen(board, position);
                game = new Game(board);
            }
            
            // Act
            var move = await controller.GetMoveAsync(game);
            
            // Assert
            Assert.NotNull(move);
            
            // Verify the move is valid
            var sourceSquare = game.Board.GetSquare(move!.Value.from);
            var destSquare = game.Board.GetSquare(move!.Value.to);
            
            Assert.NotNull(sourceSquare.Piece);
            Assert.Equal(game.CurrentTurn, sourceSquare.Piece.Color);
            Assert.True(sourceSquare.Piece.IsValidMove(game.Board, sourceSquare, destSquare));
        }

        /// <summary>
        /// Tests switching controllers during a game.
        /// </summary>
        [Fact]
        public async Task Game_SwitchControllersDuringGame_WorksCorrectly()
        {
            // Arrange
            var initialWhiteController = new Mock<IController>();
            initialWhiteController
                .Setup(c => c.GetMoveAsync(It.IsAny<Game>()))
                .ReturnsAsync(("e2", "e4"));
                
            var initialBlackController = new Mock<IController>();
            initialBlackController
                .Setup(c => c.GetMoveAsync(It.IsAny<Game>()))
                .ReturnsAsync(("e7", "e5"));
                
            var newWhiteController = new Mock<IController>();
            newWhiteController
                .Setup(c => c.GetMoveAsync(It.IsAny<Game>()))
                .ReturnsAsync(("d2", "d4"));
                
            var game = new Game(initialWhiteController.Object, initialBlackController.Object);
            
            // Act - White's first move
            bool firstMoveResult = await game.RequestAndExecuteNextMoveAsync();
            Assert.True(firstMoveResult);
            initialWhiteController.Verify(c => c.GetMoveAsync(game), Times.Once);
            
            // Black's first move
            bool secondMoveResult = await game.RequestAndExecuteNextMoveAsync();
            Assert.True(secondMoveResult);
            initialBlackController.Verify(c => c.GetMoveAsync(game), Times.Once);
            
            // Switch White's controller
            game.SetController(newWhiteController.Object, PieceColor.White);
            
            // White's second move with new controller
            bool thirdMoveResult = await game.RequestAndExecuteNextMoveAsync();
            Assert.True(thirdMoveResult);
            newWhiteController.Verify(c => c.GetMoveAsync(game), Times.Once);
            
            // Assert
            Assert.Equal(3, game.MoveHistory.Count);
        }
        
        /// <summary>
        /// Tests that the RandomController doesn't return moves that would leave the king in check.
        /// </summary>
        [Fact]
        public async Task RandomController_KingInCheck_OnlyReturnsLegalMoves()
        {
            // Arrange - Set up a board with the king in check
            var board = new Board();
            
            // Set up a simple position with white king in check
            TestHelpers.SetupBoardFromFen(board, "4q3/8/8/8/8/8/8/4K3");
            
            var randomController = new RandomController();
            var game = new Game(board, PieceColor.White);
            
            // Act
            var move = await randomController.GetMoveAsync(game);
            
            // Assert
            Assert.NotNull(move);
            
            // Verify the move gets the king out of check
            bool moveResult = game.TryMove(move!.Value.from, move!.Value.to);
            Assert.True(moveResult);
            Assert.False(game.IsInCheck(PieceColor.White));
        }

        /// <summary>
        /// Tests that a custom controller implementing IController can be used.
        /// </summary>
        [Fact]
        public async Task Game_CustomControllerImplementation_WorksCorrectly()
        {
            // Create a custom controller that always plays e4
            var customWhiteController = new E4OpeningController();
            var humanBlackController = new HumanController();
            
            var game = new Game(customWhiteController, humanBlackController);
            
            // Act - Execute the white move
            bool moveResult = await game.RequestAndExecuteNextMoveAsync();
            
            // Assert
            Assert.True(moveResult);
            Assert.Equal(PieceColor.Black, game.CurrentTurn);
            
            // Verify e4 was played
            var e4Square = game.Board.GetSquare("e4");
            Assert.NotNull(e4Square.Piece);
            Assert.Equal(PieceColor.White, e4Square.Piece.Color);
            Assert.IsType<Pawn>(e4Square.Piece);
        }
    }

    /// <summary>
    /// A custom controller that always plays e4 as the opening move.
    /// </summary>
    public class E4OpeningController : IController
    {
        private bool _openingMovePlayed = false;
        
        public Task<(string from, string to)?> GetMoveAsync(Game game)
        {
            if (!_openingMovePlayed)
            {
                _openingMovePlayed = true;
                return Task.FromResult<(string from, string to)?>(("e2", "e4"));
            }
            
            // Use a random controller for subsequent moves
            var randomController = new RandomController();
            return randomController.GetMoveAsync(game);
        }
    }
}

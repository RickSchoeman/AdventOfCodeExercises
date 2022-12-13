using FluentAssertions;
using Moq;
using RockPaperScissors.Interfaces;
using RockPaperScissors.Models;
using RockPaperScissors.Repositories;
using RockPaperScissors.Services;
using Xunit;

namespace RockPaperScissors.Tests
{
    public class RockPaperScissorServiceTests
    {
        private readonly RockPaperScissorService _rockPaperScissorService;
        private readonly Mock<ITurnService> _turnServiceMock = new Mock<ITurnService>();

        private readonly Player player = new() { Name = "Player" };
        private readonly Player opponent = new() { Name = "Opponent" };

        public RockPaperScissorServiceTests()
        {
            _rockPaperScissorService = new RockPaperScissorService(_turnServiceMock.Object);
        }

        [Theory]
        [InlineData(false, 2500)]
        [InlineData(true, 2500)]
        public void CalculateScoreFromGame_GivenMethod_ReturnsScore(bool newMethod, int expectedScore)
        {
            //Arrange
            var moves = new List<Move>
            {
                new Move
                {
                    Player = player,
                    Shape = new Shape
                    {
                        Type = Enums.Shape.Rock
                    }
                },
                new Move
                {
                    Player = opponent,
                    Shape = new Shape
                    {
                        Type= Enums.Shape.Rock
                    }
                }
            };

            _turnServiceMock.Setup(x => x.DetermineMovesPlayedInTurn(opponent, player, It.IsAny<string>(), newMethod)).Returns(moves);

            _turnServiceMock.Setup(x => x.DetermineTurnScoreForPlayer(player, moves)).Returns(1);

            //Act
            var score = _rockPaperScissorService.CalculateScoreFromGame(player, opponent, newMethod);

            //Assert
            score.Should().Be(expectedScore);
        }
    }
}
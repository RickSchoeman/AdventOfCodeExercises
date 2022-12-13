using FluentAssertions;
using Moq;
using RockPaperScissors.Interfaces;
using RockPaperScissors.Models;
using RockPaperScissors.Services;
using Xunit;

namespace RockPaperScissors.Tests
{
    public class TurnServiceTests
    {
        private readonly TurnService _turnService;
        private readonly Mock<IShapeService> _shapeServiceMock = new Mock<IShapeService>();

        public TurnServiceTests()
        {
            _turnService = new TurnService(_shapeServiceMock.Object);
        }

        private readonly Player player = new() { Name = "Player" };
        private readonly Player opponent = new() { Name = "Opponent" };

        [Theory]
        [InlineData(Enums.Shape.Rock, Enums.Shape.Rock, Enums.Outcome.Draw, 4)]
        [InlineData(Enums.Shape.Rock, Enums.Shape.Paper, Enums.Outcome.Lose, 1)]
        [InlineData(Enums.Shape.Rock, Enums.Shape.Scissor, Enums.Outcome.Win, 7)]
        [InlineData(Enums.Shape.Paper, Enums.Shape.Rock, Enums.Outcome.Win, 8)]
        [InlineData(Enums.Shape.Paper, Enums.Shape.Paper, Enums.Outcome.Draw, 5)]
        [InlineData(Enums.Shape.Paper, Enums.Shape.Scissor, Enums.Outcome.Lose, 2)]
        [InlineData(Enums.Shape.Scissor, Enums.Shape.Rock, Enums.Outcome.Lose, 3)]
        [InlineData(Enums.Shape.Scissor, Enums.Shape.Paper, Enums.Outcome.Win, 9)]
        [InlineData(Enums.Shape.Scissor, Enums.Shape.Scissor, Enums.Outcome.Draw, 6)]
        public void DetermineTurnScoreForPlayer_GivenMoves_ReturnsScore(Enums.Shape playerShape, Enums.Shape opponentShape, Enums.Outcome matchOutcome, int expectedScore)
        {
            //Arrange
            var moves = new List<Move>
            {
                new Move
                {
                    Player = player,
                    Shape = new Shape
                    {
                        Type = playerShape,
                    }
                },
                new Move
                {
                    Player = opponent,
                    Shape = new Shape
                    {
                        Type = opponentShape,
                    }
                }
            };

            _shapeServiceMock.Setup(x => x.DetermineOutcomeMatchup(playerShape, opponentShape)).Returns(matchOutcome);

            //Act
            var score = _turnService.DetermineTurnScoreForPlayer(player, moves);

            //Assert
            score.Should().Be(expectedScore);
        }

        [Theory]
        [InlineData("A X", Enums.Shape.Rock, Enums.Shape.Rock, false)]
        [InlineData("B X", Enums.Shape.Paper, Enums.Shape.Rock, false)]
        [InlineData("C X", Enums.Shape.Scissor, Enums.Shape.Rock, false)]
        [InlineData("A Y", Enums.Shape.Rock, Enums.Shape.Paper, false)]
        [InlineData("B Y", Enums.Shape.Paper, Enums.Shape.Paper, false)]
        [InlineData("C Y", Enums.Shape.Scissor, Enums.Shape.Paper, false)]
        [InlineData("A Z", Enums.Shape.Rock, Enums.Shape.Scissor, false)]
        [InlineData("B Z", Enums.Shape.Paper, Enums.Shape.Scissor, false)]
        [InlineData("C Z", Enums.Shape.Scissor, Enums.Shape.Scissor, false)]
        [InlineData("A X", Enums.Shape.Rock, Enums.Shape.Scissor, true)]
        [InlineData("B X", Enums.Shape.Paper, Enums.Shape.Rock, true)]
        [InlineData("C X", Enums.Shape.Scissor, Enums.Shape.Paper, true)]
        [InlineData("A Y", Enums.Shape.Rock, Enums.Shape.Rock, true)]
        [InlineData("B Y", Enums.Shape.Paper, Enums.Shape.Paper, true)]
        [InlineData("C Y", Enums.Shape.Scissor, Enums.Shape.Scissor, true)]
        [InlineData("A Z", Enums.Shape.Rock, Enums.Shape.Paper, true)]
        [InlineData("B Z", Enums.Shape.Paper, Enums.Shape.Scissor, true)]
        [InlineData("C Z", Enums.Shape.Scissor, Enums.Shape.Rock, true)]
        public void DetermineMovesPlayedInTurn_GivenMoveAndMethod_ReturnsPlayedMoves(string move, Enums.Shape opponentShapeType, Enums.Shape playerShapeType, bool newMethod)
        {
            //Arrange
            var playerShape = new Shape { Type = playerShapeType };
            var opponentShape = new Shape { Type = opponentShapeType };
            var chars = GetCharsFromTurn(move);

            _shapeServiceMock.Setup(x => x.DetermineShape(chars.Item1)).Returns(opponentShape);
            _shapeServiceMock.Setup(x => x.DetermineShapeNewMethod(It.IsAny<char>(), It.IsAny<Enums.Shape>())).Returns(playerShape);
            if (!newMethod)
            {
                _shapeServiceMock.Setup(x => x.DetermineShape(chars.Item2)).Returns(playerShape);
            }

            //Act
            var moves = _turnService.DetermineMovesPlayedInTurn(opponent, player, move, newMethod);

            //Assert
            moves.Single(x => x.Player == player).Shape!.Type.Should().Be(playerShapeType);
            moves.Single(x => x.Player == opponent).Shape!.Type.Should().Be(opponentShapeType);
        }

        private static (char, char) GetCharsFromTurn(string turn)
        {
            var shapesInTurn = turn.Split(' ');

            _ = char.TryParse(shapesInTurn[0], out char opponentChar);
            _ = char.TryParse(shapesInTurn[1], out char yourChar);

            return (opponentChar, yourChar);
        }
    }
}
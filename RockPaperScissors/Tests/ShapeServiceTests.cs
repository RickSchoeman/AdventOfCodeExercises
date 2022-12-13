using FluentAssertions;
using RockPaperScissors.Models;
using RockPaperScissors.Services;
using Xunit;

namespace RockPaperScissors.Tests
{
    public class ShapeServiceTests
    {
        private readonly ShapeService _shapeService = new();

        [Theory]
        [InlineData('A', Enums.Shape.Rock)]
        [InlineData('B', Enums.Shape.Paper)]
        [InlineData('C', Enums.Shape.Scissor)]
        [InlineData('X', Enums.Shape.Rock)]
        [InlineData('Y', Enums.Shape.Paper)]
        [InlineData('Z', Enums.Shape.Scissor)]
        public void DetermineShape_GivenChar_ReturnsShape(char charFromGuide, Enums.Shape expectedShape)
        {
            //Act
            var shape = _shapeService.DetermineShape(charFromGuide);

            //Assert
            shape.Type.Should().Be(expectedShape);
        }

        [Theory]
        [InlineData('X', Enums.Shape.Rock, Enums.Shape.Scissor)]
        [InlineData('Y', Enums.Shape.Rock, Enums.Shape.Rock)]
        [InlineData('Z', Enums.Shape.Rock, Enums.Shape.Paper)]
        [InlineData('X', Enums.Shape.Paper, Enums.Shape.Rock)]
        [InlineData('Y', Enums.Shape.Paper, Enums.Shape.Paper)]
        [InlineData('Z', Enums.Shape.Paper, Enums.Shape.Scissor)]
        [InlineData('X', Enums.Shape.Scissor, Enums.Shape.Paper)]
        [InlineData('Y', Enums.Shape.Scissor, Enums.Shape.Scissor)]
        [InlineData('Z', Enums.Shape.Scissor, Enums.Shape.Rock)]
        public void DetermineShapeNewMethod_GivenCharAndOpponentShape_ReturnsShape(char charFromGuide, Enums.Shape opponentShape, Enums.Shape expectedShape)
        {
            //Act
            var shape = _shapeService.DetermineShapeNewMethod(charFromGuide, opponentShape);

            //Assert
            shape.Type.Should().Be(expectedShape);
        }

        [Theory]
        [InlineData(Enums.Shape.Rock, Enums.Shape.Rock, Enums.Outcome.Draw)]
        [InlineData(Enums.Shape.Rock, Enums.Shape.Paper, Enums.Outcome.Lose)]
        [InlineData(Enums.Shape.Rock, Enums.Shape.Scissor, Enums.Outcome.Win)]
        [InlineData(Enums.Shape.Paper, Enums.Shape.Rock, Enums.Outcome.Win)]
        [InlineData(Enums.Shape.Paper, Enums.Shape.Paper, Enums.Outcome.Draw)]
        [InlineData(Enums.Shape.Paper, Enums.Shape.Scissor, Enums.Outcome.Lose)]
        [InlineData(Enums.Shape.Scissor, Enums.Shape.Rock, Enums.Outcome.Lose)]
        [InlineData(Enums.Shape.Scissor, Enums.Shape.Paper, Enums.Outcome.Win)]
        [InlineData(Enums.Shape.Scissor, Enums.Shape.Scissor, Enums.Outcome.Draw)]
        public void DetermineOutcomeMatchup_GivenTwoShapeTypes_ReturnsOutcome(Enums.Shape playerShapeType, Enums.Shape opponentShapeType, Enums.Outcome expectedOutcome)
        {
            //Act
            var outcome = _shapeService.DetermineOutcomeMatchup(playerShapeType, opponentShapeType);

            //Assert
            outcome.Should().Be(expectedOutcome);
        }
    }
}
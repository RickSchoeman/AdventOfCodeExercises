using RockPaperScissors.Interfaces;
using RockPaperScissors.Models;

namespace RockPaperScissors.Services
{
    public class ShapeService : IShapeService
    {
        public Shape DetermineShape(char input) => input switch
        {
            'A' or 'X' => new Shape
            {
                Type = Enums.Shape.Rock,
            },
            'B' or 'Y' => new Shape
            {
                Type = Enums.Shape.Paper,
            },
            'C' or 'Z' => new Shape
            {
                Type = Enums.Shape.Scissor,
            },
            _ => new Shape
            {
                Type = Enums.Shape.Invalid,
            },
        };

        public Shape DetermineShapeNewMethod(char input, Enums.Shape opponentShapeType) => input switch
        {
            'X' => opponentShapeType switch
            {
                Enums.Shape.Rock => new Shape
                {
                    Type = Enums.Shape.Scissor,
                },
                Enums.Shape.Paper => new Shape
                {
                    Type = Enums.Shape.Rock,
                },
                Enums.Shape.Scissor => new Shape
                {
                    Type = Enums.Shape.Paper,
                },
                _ => new Shape
                {
                    Type = Enums.Shape.Invalid,
                },
            },
            'Y' => opponentShapeType switch
            {
                Enums.Shape.Rock => new Shape
                {
                    Type = Enums.Shape.Rock,
                },
                Enums.Shape.Paper => new Shape
                {
                    Type = Enums.Shape.Paper,
                },
                Enums.Shape.Scissor => new Shape
                {
                    Type = Enums.Shape.Scissor,
                },
                _ => new Shape
                {
                    Type = Enums.Shape.Invalid,
                },
            },
            'Z' => opponentShapeType switch
            {
                Enums.Shape.Rock => new Shape
                {
                    Type = Enums.Shape.Paper,
                },
                Enums.Shape.Paper => new Shape
                {
                    Type = Enums.Shape.Scissor,
                },
                Enums.Shape.Scissor => new Shape
                {
                    Type = Enums.Shape.Rock,
                },
                _ => new Shape
                {
                    Type = Enums.Shape.Invalid,
                },
            },
            _ => new Shape
            {
                Type = Enums.Shape.Invalid,
            }
        };

        public Enums.Outcome DetermineOutcomeMatchup(Enums.Shape playerShapeType, Enums.Shape opponentShapeType) => playerShapeType switch
        {
            Enums.Shape.Rock => opponentShapeType switch
            {
                Enums.Shape.Rock => Enums.Outcome.Draw,
                Enums.Shape.Paper => Enums.Outcome.Lose,
                Enums.Shape.Scissor => Enums.Outcome.Win,
                _ => Enums.Outcome.Lose,
            },
            Enums.Shape.Paper => opponentShapeType switch
            {
                Enums.Shape.Rock => Enums.Outcome.Win,
                Enums.Shape.Paper => Enums.Outcome.Draw,
                Enums.Shape.Scissor => Enums.Outcome.Lose,
                _ => Enums.Outcome.Lose,
            },
            Enums.Shape.Scissor => opponentShapeType switch
            {
                Enums.Shape.Rock => Enums.Outcome.Lose,
                Enums.Shape.Paper => Enums.Outcome.Win,
                Enums.Shape.Scissor => Enums.Outcome.Draw,
                _ => Enums.Outcome.Lose,
            },
            _ => Enums.Outcome.Lose,
        };
    }
}
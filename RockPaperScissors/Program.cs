using RockPaperScissors.Interfaces;
using RockPaperScissors.Models;
using RockPaperScissors.Repositories;
using RockPaperScissors.Services;

namespace RockPaperScissors
{
    public static class Program
    {
        private static void Main()
        {
            IShapeService _shapeService = new ShapeService();
            ITurnService _turnService = new TurnService(_shapeService);
            IRockPaperScissorService _rockPaperScissorService = new RockPaperScissorService(_turnService);

            var player = new Player
            {
                Name = "Rick",
            };

            var opponent = new Player
            {
                Name = "Elf",
            };

            var scorePartOne = _rockPaperScissorService.CalculateScoreFromGame(player, opponent, false);

            Console.WriteLine("Part 1: The score calculation if X=Rock,Y=Paper,Z=Scissors:");
            Console.WriteLine(scorePartOne);

            Console.WriteLine();

            var scorePartTwo = _rockPaperScissorService.CalculateScoreFromGame(player, opponent, true);

            Console.WriteLine("Part 1: The score calculation if X=Lose,Y=Draw,Z=Win:");
            Console.WriteLine(scorePartTwo);

            Console.ReadLine();
        }
    }
}
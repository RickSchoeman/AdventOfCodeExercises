using SeaCucumberShuffle.Interfaces;
using SeaCucumberShuffle.Repositories;
using SeaCucumberShuffle.Services;
using System.Runtime.CompilerServices;

namespace SeaCucumberShuffle
{
    internal class Program
    {
        static void Main()
        {
            ISeaCucumberService _seaCucumberService = new SeaCucumberService();
            IMovementService _movementService = new MovementService(_seaCucumberService);

            var turns = _movementService.DetermineWhenMovementStops();
            Console.WriteLine(turns);
/*            foreach(var turn in turns)
            {
                Console.WriteLine($"X={turn.XCoordinate}Y={turn.YCoordinate}Type={turn.Type}");
            }*/
        }
    }
}
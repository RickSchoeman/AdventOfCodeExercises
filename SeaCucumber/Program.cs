using SeaCucumberShuffle.Interfaces;
using SeaCucumberShuffle.Services;

namespace SeaCucumberShuffle
{
    internal class Program
    {
        private static void Main()
        {
            ISeaCucumberService _seaCucumberService = new SeaCucumberService();
            IMovementService _movementService = new MovementService(_seaCucumberService);

            //var turns = _movementService.DetermineWhenMovementStops();
            var x = _movementService.DifferentCalculation();

            /*            foreach(var turn in turns)
                        {
                            Console.WriteLine($"X={turn.XCoordinate}Y={turn.YCoordinate}Type={turn.Type}");
                        }*/
        }
    }
}
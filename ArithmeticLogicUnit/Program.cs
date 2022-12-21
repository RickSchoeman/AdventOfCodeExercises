using ArithmeticLogicUnit.Interfaces;
using ArithmeticLogicUnit.Services;

namespace ArithmeticLogicUnit
{
    public static class Program
    {
        private static void Main()
        {
            IInstructionService _instructionService = new InstructionService();
            IProcessingService _processingService = new ProcessingService(_instructionService);
            IMonadService _monadService = new MonadService(_processingService);

            //Part 1: 39924989499969
            //Part 2: 16811412161117

            var largestModelNumber = _monadService.DetermineLargestModelNumber(39924989499969); //Reduced the amount to save calculating space

            Console.WriteLine($"After running the new ALU we determined the largest model number possible is: {largestModelNumber}");
        }
    }
}
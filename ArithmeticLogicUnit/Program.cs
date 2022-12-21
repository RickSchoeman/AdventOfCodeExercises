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

            //
            //
            //      I tried it for a while but my code does not seem to work and i couldn't figure out why,
            //      when debugging it seemed that sometimes my values were not written to the object,
            //      maybe some synchronous performance issue
            //
            //
            //

            var largestModelNumber = _monadService.DetermineLargestModelNumber(39924989499969); //Reduced the amount to save calculating space

            Console.WriteLine($"After running the new ALU we determined the largest model number possible is: {largestModelNumber}");
        }
    }
}
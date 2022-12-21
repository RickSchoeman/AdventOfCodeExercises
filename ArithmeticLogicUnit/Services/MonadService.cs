using ArithmeticLogicUnit.Interfaces;
using ArithmeticLogicUnit.Models;

namespace ArithmeticLogicUnit.Services
{
    public class MonadService : IMonadService
    {
        private readonly IProcessingService _processingService;

        public MonadService(IProcessingService processingService)
        {
            _processingService = processingService;
        }

        public long DetermineLargestModelNumber(long input)
        {
            long highestNumber = 11111111111111; //11111196249761

            for (long modelNumber = input; modelNumber <= 99999999999999; modelNumber++)
            {
                var modelNumberString = modelNumber.ToString();

                if (modelNumberString.Contains('0'))
                {
                    continue;
                }

                var processingVariables = new ProcessingVariables();
                var result = _processingService.ProcessCommands(processingVariables, modelNumberString);

                Console.WriteLine(result.Z);

                if (result.Z == 0 && modelNumber > highestNumber)
                {
                    highestNumber = modelNumber;
                }
            }
            return highestNumber;
        }
    }
}
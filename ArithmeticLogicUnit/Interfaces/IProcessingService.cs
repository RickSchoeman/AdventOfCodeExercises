using ArithmeticLogicUnit.Models;

namespace ArithmeticLogicUnit.Interfaces
{
    public interface IProcessingService
    {
        ProcessingVariables ProcessCommands(ProcessingVariables processingVariables, string monadInput);
    }
}
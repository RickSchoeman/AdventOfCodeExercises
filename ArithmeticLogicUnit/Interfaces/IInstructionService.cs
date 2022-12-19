using ArithmeticLogicUnit.Models;

namespace ArithmeticLogicUnit.Interfaces
{
    public interface IInstructionService
    {
        Enums.InstructionMethod DetermineInstructionMethod(string methodFromCommand);
        int ExecuteInstruction(int aValue, int bValue, Enums.InstructionMethod method);
    }
}
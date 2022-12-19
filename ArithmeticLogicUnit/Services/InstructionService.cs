using ArithmeticLogicUnit.Interfaces;
using ArithmeticLogicUnit.Models;

namespace ArithmeticLogicUnit.Services
{
    public class InstructionService : IInstructionService
    {
        public Enums.InstructionMethod DetermineInstructionMethod(string methodFromCommand) => methodFromCommand switch
        {
            "inp" => Enums.InstructionMethod.Inp,
            "add" => Enums.InstructionMethod.Add,
            "mul" => Enums.InstructionMethod.Mul,
            "div" => Enums.InstructionMethod.Div,
            "mod" => Enums.InstructionMethod.Mod,
            "eql" => Enums.InstructionMethod.Eql,
            _ => throw new NotImplementedException(),
        };

        public int ExecuteInstruction(int aValue, int bValue, Enums.InstructionMethod method) => method switch
        {
            Enums.InstructionMethod.Inp => InputInstruction(aValue),
            Enums.InstructionMethod.Add => AddInstruction(aValue, bValue),
            Enums.InstructionMethod.Mul => MultiplyInstruction(aValue, bValue),
            Enums.InstructionMethod.Div => DivideInstruction(aValue, bValue),
            Enums.InstructionMethod.Mod => ModuloInstruction(aValue, bValue),
            Enums.InstructionMethod.Eql => EqualInstruction(aValue, bValue),
            _ => throw new NotImplementedException()
        };

        private int InputInstruction(int aValue) => aValue;

        private int AddInstruction(int aValue, int bValue) => aValue + bValue;

        private int MultiplyInstruction(int aValue, int bValue) => aValue * bValue;

        private int DivideInstruction(int aValue, int bValue) => bValue == 0 ? 0 : (int)Math.Truncate((double)aValue / bValue);

        private int ModuloInstruction(int aValue, int bValue) => aValue < 0 || bValue <= 0 ? 0 : aValue % bValue;

        private int EqualInstruction(int aValue, int bValue) => aValue == bValue ? 1 : 0;
    }
}
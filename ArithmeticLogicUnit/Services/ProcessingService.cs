using ArithmeticLogicUnit.Interfaces;
using ArithmeticLogicUnit.Models;
using ArithmeticLogicUnit.Repositories;

namespace ArithmeticLogicUnit.Services
{
    public class ProcessingService
    {
        private readonly IInstructionService _instructionService;
        private readonly CommandsRepository _commandsRepository = new();

        public ProcessingService(IInstructionService instructionService)
        {
            _instructionService = instructionService;
        }

        public ProcessingVariables ProcessCommands(ProcessingVariables processingVariables)
        {
            var commands = _commandsRepository.GetCommands();

            foreach (var command in commands)
            {
                var valuesFromCommand = GetValuesFromCommand(command);
                var method = _instructionService.DetermineInstructionMethod(valuesFromCommand.Item1);
                var variableToChange = DetermineVariable(valuesFromCommand.Item2);
                var aValue = DetermineInputValue(valuesFromCommand.Item2, processingVariables);
                var bValue = DetermineInputValue(valuesFromCommand.Item3, processingVariables);

                var newValue = _instructionService.ExecuteInstruction(aValue, bValue, method);

                processingVariables = UpdateVariable(variableToChange, newValue, processingVariables);
            }

            return processingVariables;
        }

        private static ProcessingVariables UpdateVariable(Enums.Variable variableToChange, int newValue, ProcessingVariables processingVariables)
        {
            switch (variableToChange)
            {
                case (Enums.Variable.W):
                    processingVariables.W = newValue;
                    return processingVariables;

                case (Enums.Variable.X):
                    processingVariables.X = newValue;
                    return processingVariables;

                case (Enums.Variable.Y):
                    processingVariables.Y = newValue;
                    return processingVariables;

                case (Enums.Variable.Z):
                    processingVariables.Z = newValue;
                    return processingVariables;

                default:
                    return processingVariables;
            }
        }

        private static Enums.Variable DetermineVariable(char input) => input switch
        {
            'w' => Enums.Variable.W,
            'x' => Enums.Variable.X,
            'y' => Enums.Variable.Y,
            'z' => Enums.Variable.Z,
            _ => Enums.Variable.None,
        };

        private static int DetermineInputValue(char input, ProcessingVariables processingVariables) => input switch
        {
            'w' => processingVariables.W,
            'x' => processingVariables.X,
            'y' => processingVariables.Y,
            'z' => processingVariables.Z,
            _ => (int)char.GetNumericValue(input),
        };

        private static (string, char, char) GetValuesFromCommand(string commandLine)
        {
            var valuesFromCommand = commandLine.Split(' ');

            var command = valuesFromCommand[0];
            _ = char.TryParse(valuesFromCommand[1], out char aValue);
            _ = char.TryParse(valuesFromCommand[2], out char bValue);

            return (command, aValue, bValue);
        }
    }
}
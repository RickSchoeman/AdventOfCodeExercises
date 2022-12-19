using ArithmeticLogicUnit.Interfaces;
using ArithmeticLogicUnit.Models;
using ArithmeticLogicUnit.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void ProcessCommands(out ProcessingVariables processingVariables)
        {
            var commands = _commandsRepository.GetCommands();

            foreach(var command in commands)
            {
                var valuesFromCommand = GetValuesFromCommand(command);
                var method = _instructionService.de
            }

            processingVariables = new ProcessingVariables();
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

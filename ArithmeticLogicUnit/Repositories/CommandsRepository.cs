namespace ArithmeticLogicUnit.Repositories
{
    public class CommandsRepository
    {
        public IEnumerable<string> GetCommands() => File.ReadLines("Data/Commands.txt");
    }
}
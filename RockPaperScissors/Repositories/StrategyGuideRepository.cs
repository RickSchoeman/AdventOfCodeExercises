namespace RockPaperScissors.Repositories
{
    public class StrategyGuideRepository
    {
        public IEnumerable<string> GetTurnsFromStrategyGuide() => File.ReadLines("Data/StrategyGuide.txt");
    }
}
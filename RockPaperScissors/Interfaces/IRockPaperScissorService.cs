using RockPaperScissors.Models;

namespace RockPaperScissors.Interfaces
{
    public interface IRockPaperScissorService
    {
        int CalculateScoreFromGame(Player you, Player opponent, bool newMethod);
    }
}
using RockPaperScissors.Models;

namespace RockPaperScissors.Interfaces
{
    public interface ITurnService
    {
        int DetermineTurnScoreForPlayer(Player player, IList<Move> moves);

        IList<Move> DetermineMovesPlayedInTurn(Player opponent, Player you, string move, bool newMethod);
    }
}
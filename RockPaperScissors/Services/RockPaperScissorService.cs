using RockPaperScissors.Interfaces;
using RockPaperScissors.Models;
using RockPaperScissors.Repositories;

namespace RockPaperScissors.Services
{
    public class RockPaperScissorService : IRockPaperScissorService
    {
        private readonly ITurnService _turnService;
        private readonly StrategyGuideRepository _strategyGuideRepository = new();

        public RockPaperScissorService(ITurnService turnService)
        {
            _turnService = turnService;
        }

        public int CalculateScoreFromGame(Player you, Player opponent, bool newMethod)
        {
            var gameScore = 0;
            var moves = _strategyGuideRepository.GetTurnsFromStrategyGuide();

            foreach (var move in moves)
            {
                var playedMoves = _turnService.DetermineMovesPlayedInTurn(opponent, you, move, newMethod).ToList();

                gameScore += _turnService.DetermineTurnScoreForPlayer(you, playedMoves);
            }

            return gameScore;
        }
    }
}
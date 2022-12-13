using RockPaperScissors.Interfaces;
using RockPaperScissors.Models;

namespace RockPaperScissors.Services
{
    public class TurnService : ITurnService
    {
        private readonly IShapeService _shapeService;

        public TurnService(IShapeService shapeService)
        {
            _shapeService = shapeService;
        }

        public int DetermineTurnScoreForPlayer(Player player, IList<Move> moves)
        {
            var turnScore = 0;
            var playerMove = moves.Single(x => x.Player == player);
            var opponentMove = moves.Single(x => x.Player != player);

            if (playerMove.Shape == null)
            {
                return turnScore;
            }

            if (opponentMove.Shape == null)
            {
                return turnScore;
            }

            turnScore += playerMove.Shape.Value;

            var matchupOutcome = _shapeService.DetermineOutcomeMatchup(playerMove.Shape.Type, opponentMove.Shape.Type);

            turnScore += (int)matchupOutcome;

            return turnScore;
        }

        public IList<Move> DetermineMovesPlayedInTurn(Player opponent, Player you, string move, bool newMethod)
        {
            var moves = new List<Move>();

            var chars = GetCharsFromTurn(move);
            var opponentShape = _shapeService.DetermineShape(chars.Item1);
            Shape yourShape;
            if (newMethod)
            {
                yourShape = _shapeService.DetermineShapeNewMethod(chars.Item2, opponentShape.Type);
            }
            else
            {
                yourShape = _shapeService.DetermineShape(chars.Item2);
            }

            moves.Add(new Move { Player = opponent, Shape = opponentShape });
            moves.Add(new Move { Player = you, Shape = yourShape });

            return moves;
        }

        private static (char, char) GetCharsFromTurn(string turn)
        {
            var shapesInTurn = turn.Split(' ');

            _ = char.TryParse(shapesInTurn[0], out char opponentChar);
            _ = char.TryParse(shapesInTurn[1], out char yourChar);

            return (opponentChar, yourChar);
        }
    }
}
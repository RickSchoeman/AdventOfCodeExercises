using RockPaperScissors.Models;

namespace RockPaperScissors.Interfaces
{
    public interface IShapeService
    {
        Shape DetermineShape(char input);

        Shape DetermineShapeNewMethod(char input, Enums.Shape opponentShapeType);

        Enums.Outcome DetermineOutcomeMatchup(Enums.Shape playerShapeType, Enums.Shape opponentShapeType);
    }
}
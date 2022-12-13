namespace RockPaperScissors.Models
{
    public class Enums
    {
        public enum Shape
        {
            Rock = 1,
            Paper = 2,
            Scissor = 3,
            Invalid = 0,
        }

        public enum Outcome
        {
            Win = 6,
            Draw = 3,
            Lose = 0,
        }
    }
}
namespace RockPaperScissors.Models
{
    public class Shape
    {
        public Enums.Shape Type { get; init; }
        public int Value => (int)Type;
    }
}
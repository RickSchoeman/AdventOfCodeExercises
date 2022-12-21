namespace SeaCucumberShuffle.Models
{
    public class SeaCucumber
    {
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public Enums.SeaCucumberType Type { get; set; }

        public bool HasMoved { get; set; }
    }
}
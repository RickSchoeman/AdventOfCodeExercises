using SeaCucumberShuffle.Models;

namespace SeaCucumberShuffle.Repositories
{
    public class SeaFloorRepository
    {
        public SeaFloor GetSeaFloor()
        {
            var allLines = File.ReadLines("Data/SeaFloor.txt");

            var height = allLines.Count();
            var width = allLines.First().Length;

            return new SeaFloor { Layout = allLines, Height = height, Width = width };
        }
    }
}
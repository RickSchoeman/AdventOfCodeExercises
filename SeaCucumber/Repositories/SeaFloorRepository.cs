using SeaCucumberShuffle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

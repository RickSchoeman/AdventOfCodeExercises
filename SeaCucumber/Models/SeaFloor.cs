using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaCucumberShuffle.Models
{
    public class SeaFloor
    {
        public IEnumerable<string>? Layout { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}

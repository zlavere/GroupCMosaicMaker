using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace ImageSandbox.Model
{
    public class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public List<Color> Colors { get; set; }
        public List<int> PixelIndexes { get; set; }
        public List<int> RedValues { get; set; }
        public List<int> GreenValues { get; set; }
        public List<int> BlueValues { get; set; }

        public Cell()
        {
            this.Colors = new List<Color>();
            this.PixelIndexes = new List<int>();
            this.RedValues = new List<int>();
            this.GreenValues = new List<int>();
            this.BlueValues = new List<int>();
        }

    }
}

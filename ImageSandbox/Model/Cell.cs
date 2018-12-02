using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Notifications;

namespace ImageSandbox.Model
{
    public class Cell
    {
        public Color averageColor;

        public int X { get; set; }
        public int Y { get; set; }
        public List<Color> Colors { get; set; }
        public List<int> PixelIndexes { get; set; }
        public List<byte> RedValues { get; set; }
        public List<byte> GreenValues { get; set; }
        public List<byte> BlueValues { get; set; }

        public Color AverageColor
        {
            get
            {
                var averageRed = (int)this.RedValues.Average(value => value);
                var averageGreen = (int)this.GreenValues.Average(value => value);
                var averageBlue = (int)this.BlueValues.Average(value => value);
                this.averageColor = Color.FromArgb(0, (byte)averageRed, (byte)averageGreen, (byte)averageBlue);
                return this.averageColor;
            } 
        }


        public Cell()
        {
            this.Colors = new List<Color>();
            this.PixelIndexes = new List<int>();
            this.RedValues = new List<byte>();
            this.GreenValues = new List<byte>();
            this.BlueValues = new List<byte>();
        }

        public void SetRbgLists()
        {
            foreach (var current in this.Colors)
            {
                this.BlueValues.Add(current.B);
                this.RedValues.Add(current.R);
                this.GreenValues.Add(current.G);
            }
        }



    }
}

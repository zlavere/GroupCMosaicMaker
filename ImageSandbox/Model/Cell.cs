using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Notifications;

namespace ImageSandbox.Model
{
    public class Cell
    {
        private Color averageColor;
        private List<byte> redValues;
        private List<byte> greenValues;
        private List<byte> blueValues;

        public int X { get; set; }
        public int Y { get; set; }
        public List<Color> Colors { get; set; }
        public List<int> PixelIndexes { get; set; }

        public List<byte> RedValues
        {
            get
            {
                this.redValues = this.Colors.Select(color => color.R).ToList();
                return this.redValues;
            }
            set => this.redValues = value;
        }

        public List<byte> GreenValues
        {
            get
            {
                this.greenValues = this.Colors.Select(color => color.G).ToList();
                return this.greenValues;
            }
            set => this.greenValues = value;
        }

        public List<byte> BlueValues
        {
            get
            {
                this.blueValues = this.Colors.Select(color => color.B).ToList();
                return this.blueValues;
            }
            set => this.blueValues = value;
        }

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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace ImageSandbox.Model
{
    public class SolidMosaic:Mosaic
    {
        private int numberOfColumns;
        private int numberOfRows;
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public BitmapImage Image { get; set; }

        public int NumberOfColumns
        {
            get
            {
                this.numberOfColumns = this.ImageWidth / (int) this.CellSide;
                return this.numberOfColumns;
            }
            set => this.numberOfColumns = value;
        }

        public int NumberOfRows
        {
            get
            {
                this.numberOfRows = this.ImageHeight / (int)this.CellSide;
                return this.numberOfRows;
            }
            set => this.numberOfRows = value;
        }

        public SolidMosaic(uint cellSideLength, BitmapImage image) : base(cellSideLength)
        {
            this.Image = image;
            this.ImageHeight = this.Image.PixelHeight;
            this.ImageWidth = this.Image.PixelWidth;
        }

        //private Color getAverageColorOfCell() //TODO switch to this signature probably
        private void getAverageColorOfCell()
        {
            //TODO Get Average Color
            //BitmapDecoder decoder for imported image
            //this.GetCellOfImage(decoder, 0, 0);
           
        }
    }
}

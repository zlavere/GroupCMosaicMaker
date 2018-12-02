using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using ImageSandbox.Extensions;
using ImageSandbox.Utility;

namespace ImageSandbox.Model
{
    public class SolidMosaic : Mosaic
    {
        #region Properties

        public List<Cell> Cells { get; set; }
        public List<Color> Colors { get; set; }

        #endregion

        #region Constructors

        public SolidMosaic(WriteableBitmap sourceImage, WriteableBitmap mosaicImage, int cellSideLength,
            GridFactory gridFactory)
            : base(sourceImage, mosaicImage, cellSideLength, gridFactory)
        {
        }

        #endregion

        #region Methods

        public void SetCellData()
        {
            this.Colors = this.SourceImage.GetPixelColors();

            this.Cells = new List<Cell>();
            var countCells = this.GridFactory.NumberOfColumns * this.GridFactory.NumberOfRows;

            this.createCell(0);

        }

        private IList<int> createCell(int index)
        {
            var newCell = new Cell();
            var pixelIndexes = new List<int>();
            var cellSideLength = this.GridFactory.CellSideLength;
            var pixelsPerCell = cellSideLength * cellSideLength;
            Debug.WriteLine($"Pixel Width = {this.SourceImage.PixelWidth}");
            Debug.WriteLine($"Cell Side Length = {cellSideLength}");
            for (var i = 0; i < this.GridFactory.CellSideLength; i++)
            {
                for (var j = 0; j < this.GridFactory.CellSideLength; j++)
                {
                    var pixelIndex = j + ((index + i) * this.SourceImage.PixelWidth);
                        Debug.WriteLine($"Pixel Index = {pixelIndex}{Environment.NewLine}" +
                                        $"   Pixel In Cell = ({j}, {i}){Environment.NewLine}" +
                                        $"      of Cell = {index}{Environment.NewLine}" +
                                        $"---");
                    pixelIndexes.Add(pixelIndex);
                }
            }

            return pixelIndexes;
        }
        #endregion
    }
}

//        private byte[] byteArraySetup()
//        {
//            var cells = this.SetCellData();
//            using (var stream = MosaicImage.PixelBuffer.AsStream())
//            {
//                var buffer = new byte[SourceImage.PixelWidth * SourceImage.PixelHeight * 4];
//                Debug.WriteLine($"Count Cells: {cells.Count}");
//                foreach (var current in cells)
//                {
//                    current.SetRbgLists();
//                    foreach (var pixel in current.PixelIndexes)
//                    {
//                        buffer[pixel] = current.AverageColor.B;
//                        buffer[pixel + 1] = current.AverageColor.G;
//                        buffer[pixel + 2] = current.AverageColor.R;
//                        buffer[pixel + 3] = 0;
//                    }
//                }
//                stream.Read(buffer, 0, MosaicImage.PixelWidth * MosaicImage.PixelHeight);
//                return buffer;
//            }
//        }

//        public async Task<WriteableBitmap> ConstructMosaic()
//        {
//            var bitmap = new WriteableBitmap(SourceImage.PixelWidth, SourceImage.PixelHeight);
//            var pixelData = this.byteArraySetup();
//            using (var stream = bitmap.PixelBuffer.AsStream())
//            {
//                await stream.WriteAsync(pixelData, 0, SourceImage.PixelHeight * SourceImage.PixelWidth);
//                MosaicImage = bitmap;
//                return MosaicImage;
//            }
//        }


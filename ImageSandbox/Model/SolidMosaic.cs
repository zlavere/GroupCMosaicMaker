using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using ImageSandbox.Extensions;

namespace ImageSandbox.Model
{
    public class SolidMosaic : Mosaic
    {
        public List<Cell> Cells { get; set; }

        #region Constructors

        public SolidMosaic(WriteableBitmap sourceImage, WriteableBitmap mosaicImage, int cellSideLength,
            GridFactory gridFactory)
            : base(sourceImage, mosaicImage, cellSideLength, gridFactory)
        {
        }

        #endregion

        #region Methods

        public List<Cell> SetCellData()
        {
            var colors = SourceImage.GetPixelColors();
            this.Cells = new List<Cell>();
            var pixelIndex = 0;
            var cellY = 0;
            var cellX = 0;
            var currentCell = new Cell();

            for (var i = 0; i < SourceImage.PixelHeight; i++)
            {
                if ((i + 1) % GridFactory.CellSideLength == 0 || i + 1 >=
                    GridFactory.CellSideLength * (GridFactory.NumberOfRows - 1))
                {
                    cellX = 0;
                    cellY++;
                    currentCell = new Cell {
                        Y = cellY
                    };
                    this.Cells.Add(currentCell);
                }

                for (var j = 0; j < SourceImage.PixelWidth; j++)
                {
                    if ((j + 1) % GridFactory.CellSideLength == 0 || j + 1 >=
                        GridFactory.CellSideLength * (GridFactory.NumberOfColumns - 1))
                    {
                        if (i + 1 % GridFactory.CellSideLength != 0 || !(i + 1 >=
                                                                         GridFactory.CellSideLength *
                                                                         (GridFactory.NumberOfRows - 1)))
                        {
                            currentCell = new Cell {
                                X = cellX
                            };
                            this.Cells.Add(currentCell);
                        }
                        else
                        {
                            currentCell.X = cellX;
                        }
                    }

                    currentCell.PixelIndexes.Add(pixelIndex);
                    currentCell.Colors.Add(colors[pixelIndex]);
                    pixelIndex++;
                }
            }

            return this.Cells;
        }

        private byte[] byteArraySetup()
        {
            using (var stream = this.MosaicImage.PixelBuffer.AsStream())
            {
                var buffer = new byte[(this.SourceImage.PixelWidth * this.SourceImage.PixelHeight) * 4];
                stream.Read(buffer, 0, this.MosaicImage.PixelWidth * this.MosaicImage.PixelHeight);
;               foreach (var current in this.SetCellData())
                {
                    current.SetRbgLists();
                    foreach (var pixel in current.PixelIndexes)
                    {
                        buffer[pixel] = current.AverageColor.B;
                        buffer[pixel+ 1] = current.AverageColor.G;
                        buffer[pixel+ 2] = current.AverageColor.R;
                        buffer[pixel + 3] = 0;

                    }
                }

                return buffer;

            }
        }

        public async Task<WriteableBitmap> ConstructMosaic()
        {
            var bitmap = new WriteableBitmap(this.SourceImage.PixelWidth, this.SourceImage.PixelHeight);
            var pixelData = this.byteArraySetup();
            using (var stream = bitmap.PixelBuffer.AsStream())
            {
              await stream.WriteAsync(pixelData, 0, this.SourceImage.PixelHeight * this.SourceImage.PixelWidth);
                this.MosaicImage = bitmap;  
              return this.MosaicImage;
            }

        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using ImageSandbox.Extensions;

namespace ImageSandbox.Model
{
    public class SolidMosaic:Mosaic
    {
        public SolidMosaic(WriteableBitmap sourceImage, WriteableBitmap mosaicImage, int cellSideLength,
            GridFactory gridFactory)
            : base(sourceImage, mosaicImage, cellSideLength, gridFactory){}

        public async void SetCellData()
        {
            var colors = await this.SourceImage.GetPixelColors();
            var cells = new List<Cell>();
            var pixelIndex = 0;
            var cellY = 0;
            var cellX = 0;
            var currentCell = new Cell();

            for (var i = 0; i < this.SourceImage.PixelHeight; i++)
            {
                if ((i + 1) % this.GridFactory.CellSideLength == 0 || (i + 1) >=
                    this.GridFactory.CellSideLength * (this.GridFactory.NumberOfRows - 1))
                {
                    cellX = 0;
                    cellY++;
                    currentCell = new Cell {
                        Y = cellY
                    };
                    cells.Add(currentCell);
                }
                for (var j = 0; j < this.SourceImage.PixelWidth; j++)
                {
                    if ((j + 1) % this.GridFactory.CellSideLength == 0 || (j + 1) >=
                        this.GridFactory.CellSideLength * (this.GridFactory.NumberOfColumns - 1))
                    {
                        if (i + 1 % this.GridFactory.CellSideLength != 0 || !((i + 1) >=
                            this.GridFactory.CellSideLength * (this.GridFactory.NumberOfRows- 1)))
                        {
                            currentCell = new Cell() {
                                X = cellX
                            };
                            cells.Add(currentCell);
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
        }
    }
  }

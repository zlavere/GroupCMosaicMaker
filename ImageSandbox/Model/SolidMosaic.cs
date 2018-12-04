using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using ImageSandbox.Extensions;

namespace ImageSandbox.Model
{
    public class SolidMosaic : Mosaic
    {
        #region Properties

        private int PixelIndex { get; set; }
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
            this.Colors = SourceImage.GetPixelColors();
            this.calculateCellAttributes();
            this.writeToBitmapAverageColor();
        }

        private void calculateCellAttributes()
        {
            this.Cells = new List<Cell>();
            this.PixelIndex = 0;
            for (var rowIndex = 0; rowIndex <= GridFactory.NumberOfRows; rowIndex++)
            {
                this.Cells.AddRange(this.createRow(rowIndex));
            }
        }

        private List<Cell> createRow(int rowIndex)
        {
            var rowOfCells = new List<Cell>();
            for (var columnIndex = 0; columnIndex <= GridFactory.NumberOfColumns; columnIndex++)
            {
                var cell = new Cell();
                var currentHeight = 0;
                var currentWidth = 0;

                if (columnIndex == GridFactory.NumberOfColumns)
                {
                    currentWidth = GridFactory.LastColumnWidth;
                }
                else
                {
                    currentWidth = GridFactory.CellSideLength;
                }

                if (rowIndex == GridFactory.NumberOfRows)
                {
                    currentHeight = GridFactory.LastRowHeight;
                }
                else
                {
                    currentHeight = GridFactory.CellSideLength;
                }

                cell = this.createCell(rowIndex, currentHeight, currentWidth, cell, columnIndex);
                rowOfCells.Add(cell);
            }

            return rowOfCells;
        }

        private Cell createCell(int rowIndex, int currentHeight, int currentWidth, Cell cell, int columnIndex)
        {
            for (var pixelY = 0; pixelY < currentHeight; pixelY++)
            {
                for (var pixelX = 0; pixelX < currentWidth; pixelX++)
                {
                    cell.X = columnIndex;
                    cell.Y = rowIndex;
                    cell.Colors.Add(this.Colors[this.PixelIndex]);
                    cell.PixelIndexes.Add(this.PixelIndex);
                    var byteOffset = (pixelY * SourceImage.PixelWidth + pixelX +
                                      columnIndex * GridFactory.CellSideLength +
                                      rowIndex * SourceImage.PixelWidth * GridFactory.CellSideLength) * 4;
                    cell.PixelOffsetsInByteArray.Add(byteOffset);
                    this.PixelIndex++;
                }
            }
            
            return cell;
        }

        private async void writeToBitmapAverageColor()
        {
            var sourcePixels = SourceImage.PixelWidth * SourceImage.PixelHeight;
            var mosaic = new WriteableBitmap(SourceImage.PixelWidth, SourceImage.PixelHeight);
            using (var stream = this.MosaicImage.PixelBuffer.AsStream())
            {
                var buffer = this.setUpSolidMosaicPixelData();

                await stream.WriteAsync(buffer, 0, sourcePixels * 4);
                await stream.FlushAsync();
            }
        }

        private byte[] setUpSolidMosaicPixelData()
        {
            var buffer = new byte[SourceImage.PixelWidth * SourceImage.PixelHeight * 4];

            foreach (var current in this.Cells)
            {
                foreach (var currentPixelStart in current.PixelOffsetsInByteArray)
                {
                    buffer[currentPixelStart] = current.AverageColor.B;
                    buffer[currentPixelStart + 1] = current.AverageColor.G;
                    buffer[currentPixelStart + 2] = current.AverageColor.R;
                    buffer[currentPixelStart + 3] = 0;
                }
            }

            var countWhere0 = buffer.Count(value => value == 0);
            Debug.WriteLine(countWhere0);
            return buffer;
        }

        #endregion
    }
}
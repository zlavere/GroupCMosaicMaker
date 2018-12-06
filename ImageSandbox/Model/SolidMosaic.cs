using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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

        /// <summary>
        ///     Gets or sets the cells.
        /// </summary>
        /// <value>
        ///     The cells.
        /// </value>
        public List<Cell> Cells { get; set; }

        /// <summary>
        ///     Gets or sets the colors.
        /// </summary>
        /// <value>
        ///     The colors.
        /// </value>
        public List<Color> Colors { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SolidMosaic" /> class.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        /// <param name="mosaicImage">The mosaic image.</param>
        /// <param name="cellSideLength">Length of the cell side.</param>
        /// <param name="gridFactory">The grid factory.</param>
        public SolidMosaic(WriteableBitmap sourceImage, WriteableBitmap mosaicImage, int cellSideLength,
            GridFactory gridFactory)
            : base(sourceImage, mosaicImage, cellSideLength, gridFactory)
        {
        }

        #endregion

        #region Methods

        public async Task<WriteableBitmap> SetCellData()
        {
            this.Colors = await this.SourceImage.GetPixelColors();
            this.calculateCellAttributes();
            return await this.writeToBitmapAverageColor();
        }

        private void calculateCellAttributes()
        {
            this.Cells = new List<Cell>();
            this.PixelIndex = 0;
            for (var rowIndex = 0; rowIndex < GridFactory.NumberOfRows; rowIndex++)
            {
                this.Cells.AddRange(this.createRow(rowIndex));
            }
        }

        private List<Cell> createRow(int rowIndex)
        {
            var rowOfCells = new List<Cell>();
            for (var columnIndex = 0; columnIndex < GridFactory.NumberOfColumns; columnIndex++)
            {
                var cell = new Cell();
                int currentHeight;
                int currentWidth;

                if (columnIndex == GridFactory.NumberOfColumns -1)
                {
                    currentWidth = GridFactory.LastColumnWidth;
                }
                else
                {
                    currentWidth = GridFactory.CellSideLength;
                }

                if (rowIndex == GridFactory.NumberOfRows -1)
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
                    var byteOffset = (((pixelY * SourceImage.PixelWidth) + pixelX) +
                                      (columnIndex * GridFactory.CellSideLength) +
                                      rowIndex * SourceImage.PixelWidth * GridFactory.CellSideLength) * 4;
                    var colorIndex = Convert.ToInt32(byteOffset / 4);

                    try
                    {
                        
                        cell.Colors.Add(this.Colors[colorIndex]);
                        cell.PixelOffsetsInByteArray.Add(byteOffset);
                        this.PixelIndex++;
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine(Convert.ToInt32(byteOffset / 4));
                    }
                    
                }
            }
            
            return cell;
        }

        private async Task<WriteableBitmap> writeToBitmapAverageColor()
        {
            var sourcePixels = SourceImage.PixelWidth * SourceImage.PixelHeight;
            var mosaic = new WriteableBitmap(SourceImage.PixelWidth, SourceImage.PixelHeight);
            using (var stream = mosaic.PixelBuffer.AsStream())
            {
                var buffer = this.setUpSolidMosaicPixelData();

                await stream.WriteAsync(buffer, 0, sourcePixels * 4);
                await stream.FlushAsync();
            }

            return mosaic;
        }

        private byte[] setUpSolidMosaicPixelData()
        {
            var buffer = new byte[SourceImage.PixelWidth * SourceImage.PixelHeight * 4];

                Parallel.ForEach(this.Cells, cell =>
                {
                    Parallel.ForEach(cell.PixelOffsetsInByteArray, offset =>
                    {
                        buffer[offset] = cell.AverageColor.B;
                        buffer[offset + 1] = cell.AverageColor.G;
                        buffer[offset + 2] = cell.AverageColor.R;
                        buffer[offset + 3] = 0;
                    }); 
                });

            return buffer;
        }

        #endregion
    }
}
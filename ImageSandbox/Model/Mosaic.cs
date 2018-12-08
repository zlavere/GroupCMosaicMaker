using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace ImageSandbox.Model
{
    public abstract class Mosaic
    {
        #region Properties


        /// <summary>
        ///     Gets or sets the source image.
        /// </summary>
        /// <value>
        ///     The source image.
        /// </value>
        public WriteableBitmap SourceImage { get; set; }


        /// <summary>
        ///     Gets or sets the grid factory.
        /// </summary>
        /// <value>
        ///     The grid factory.
        /// </value>
        public GridFactory GridFactory { get; set; }

        /// <summary>
        /// Gets or sets the index of the pixel.
        /// </summary>
        /// <value>
        /// The index of the pixel.
        /// </value>
        public int PixelIndex { get; set; }
        
        /// <summary>
        /// Gets or sets the colors.
        /// </summary>
        /// <value>
        /// The colors.
        /// </value>
        public List<Color> Colors { get; set; }

        /// <summary>
        ///     Gets or sets the cells.
        /// </summary>
        /// <value>
        ///     The cells.
        /// </value>
        public List<Cell> Cells { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ImageSandbox.Model.Mosaic" /> class.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        /// <param name="gridFactory">The grid factory.</param>
        protected Mosaic(WriteableBitmap sourceImage,
            GridFactory gridFactory)
        {
            this.SourceImage = sourceImage;
            this.GridFactory = gridFactory;
        }

        public abstract Task<WriteableBitmap> SetCellData();

        protected IEnumerable<Cell> CreateRow(int rowIndex)
        {
            var rowOfCells = new List<Cell>();
            for (var columnIndex = 0; columnIndex < this.GridFactory.NumberOfColumns; columnIndex++)
            {
                var cell = new Cell();
                int currentHeight;
                int currentWidth;

                if (columnIndex == this.GridFactory.NumberOfColumns - 1)
                {
                    currentWidth = this.GridFactory.LastColumnWidth;
                }
                else
                {
                    currentWidth = this.GridFactory.CellSideLength;
                }

                if (rowIndex == this.GridFactory.NumberOfRows - 1)
                {
                    currentHeight = this.GridFactory.LastRowHeight;
                }
                else
                {
                    currentHeight = this.GridFactory.CellSideLength;
                }

                cell = this.CreateCell(rowIndex, currentHeight, currentWidth, cell, columnIndex);
                rowOfCells.Add(cell);
            }

            return rowOfCells;
        }

        protected Cell CreateCell(int rowIndex, int currentHeight, int currentWidth, Cell cell, int columnIndex)
        {
            for (var pixelY = 0; pixelY < currentHeight; pixelY++)
            {
                for (var pixelX = 0; pixelX < currentWidth; pixelX++)
                {
                    cell.X = columnIndex;
                    cell.Y = rowIndex;
                    var byteOffset = (((pixelY * this.SourceImage.PixelWidth) + pixelX) +
                                      (columnIndex * this.GridFactory.CellSideLength) +
                                      rowIndex * this.SourceImage.PixelWidth * this.GridFactory.CellSideLength) * 4;
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

        #endregion
    }
}
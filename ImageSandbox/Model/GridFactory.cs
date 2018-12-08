using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace ImageSandbox.Model
{
    public class GridFactory
    {
        #region Data members

        private int numberOfRows;
        private int numberOfColumns;
        private int lastRowHeight;
        private int lastColumnWidth;
        private int gridHeight;
        private int gridWidth;

        #endregion

        #region Properties

        public Mosaic Mosaic { get; set; }

        /// <summary>
        ///     Gets or sets the width of the grid.
        /// </summary>
        /// <value>
        ///     The width of the grid.
        /// </value>
        public int GridWidth
        {
            get
            {
                if (this.Mosaic != null)
                {
                    this.gridWidth = this.Mosaic.SourceImage.PixelWidth;
                }

                return this.gridWidth;
            }
            set => this.gridWidth = value;
        }

        /// <summary>
        ///     Gets or sets the cells.
        /// </summary>
        /// <value>
        ///     The cells.
        /// </value>
        public List<Cell> Cells { get; set; }

        /// <summary>
        ///     Gets or sets the index of the pixel.
        /// </summary>
        /// <value>
        ///     The index of the pixel.
        /// </value>
        public int PixelIndex { get; set; }

        /// <summary>
        ///     Gets or sets the height of the grid.
        /// </summary>
        /// <value>
        ///     The height of the grid.
        /// </value>
        public int GridHeight
        {
            get
            {
                if (this.Mosaic != null)
                {
                    this.gridHeight = this.Mosaic.SourceImage.PixelHeight;
                }

                return this.gridHeight;
            }
            set => this.gridHeight = value;
        }

        /// <summary>
        ///     Gets or sets the length of the cell side.
        /// </summary>
        /// <value>
        ///     The length of the cell side.
        /// </value>
        public int CellSideLength { get; set; }

        /// <summary>
        ///     Gets the last height of the row.
        /// </summary>
        /// <value>
        ///     The last height of the row.
        /// </value>
        public int LastRowHeight
        {
            get
            {
                if (this.GridHeight % this.CellSideLength != 0)
                {
                    this.lastRowHeight = this.GridHeight % this.CellSideLength;
                }
                else
                {
                    this.lastRowHeight = this.CellSideLength;
                }

                return this.lastRowHeight;
            }
        }

        /// <summary>
        ///     Gets the last width of the column.
        /// </summary>
        /// <value>
        ///     The last width of the column.
        /// </value>
        public int LastColumnWidth
        {
            get
            {
                if (this.GridWidth % this.CellSideLength != 0)
                {
                    this.lastColumnWidth = this.GridWidth % this.CellSideLength;
                }
                else
                {
                    this.lastColumnWidth = this.CellSideLength;
                }

                return this.lastColumnWidth;
            }
        }

        /// <summary>
        ///     Gets the number of rows.
        /// </summary>
        /// <value>
        ///     The number of rows.
        /// </value>
        public int NumberOfRows
        {
            get
            {
                this.numberOfRows = this.GridHeight / this.CellSideLength;
                if (this.GridHeight % this.CellSideLength != 0)
                {
                    this.numberOfRows++;
                }

                return this.numberOfRows;
            }
        }

        /// <summary>
        ///     Gets the number of columns.
        /// </summary>
        /// <value>
        ///     The number of columns.
        /// </value>
        public int NumberOfColumns
        {
            get
            {
                this.numberOfColumns = this.GridWidth / this.CellSideLength;
                if (this.GridWidth % this.CellSideLength != 0)
                {
                    this.numberOfColumns++;
                }

                return this.numberOfColumns;
            }
        }

        #endregion

        #region Methods

        private Rectangle createCell(int width, int height)
        {
            var cell = new Rectangle {
                Width = width,
                Height = height,
                Stroke = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255)),
                StrokeThickness = 1,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            return cell;
        }

        /// <summary>
        ///     Draws the grid.
        /// </summary>
        /// <returns>
        ///     The grid.
        /// </returns>
        public Grid DrawGrid()
        {
            var grid = new Grid();

            for (var i = 0; i <= this.NumberOfRows; i++)
            {
                grid.RowDefinitions.Add(
                    new RowDefinition {
                        Height = GridLength.Auto
                    });
            }

            for (var i = 0; i < this.NumberOfColumns; i++)
            {
                grid.ColumnDefinitions.Add(
                    new ColumnDefinition {
                        Width = GridLength.Auto
                    });
            }

            for (var rowIndex = 0; rowIndex < this.NumberOfRows; rowIndex++)
            {
                this.addRowToGrid(grid, rowIndex);
            }

            return grid;
        }

        private void addRowToGrid(Grid grid, int rowNumber)
        {
            for (var columnIndex = 0; columnIndex < this.NumberOfColumns; columnIndex++)
            {
                var currentWidth = this.getCurrentColumnWidth(columnIndex);

                var currentHeight = this.getCurrentRowHeight(rowNumber);

                var cell = this.createCell(currentWidth, currentHeight);
                Grid.SetColumn(cell, columnIndex);
                Grid.SetRow(cell, rowNumber);
                grid.Name = "overlay";
                grid.Children.Add(cell);
            }
        }

        public void CalculateCellAttributes()
        {
            this.Cells = new List<Cell>();
            this.PixelIndex = 0;
            for (var rowIndex = 0; rowIndex < this.NumberOfRows; rowIndex++)
            {
                this.Cells.AddRange(this.CreateRow(rowIndex));
            }
        }

        protected IEnumerable<Cell> CreateRow(int rowIndex)
        {
            var rowOfCells = new List<Cell>();
            for (var columnIndex = 0; columnIndex < this.NumberOfColumns; columnIndex++)
            {
                var cell = new Cell();
                var currentWidth = this.getCurrentColumnWidth(columnIndex);
                var currentHeight = this.getCurrentRowHeight(rowIndex);

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
                    var byteOffset = (pixelY * this.GridWidth + pixelX +
                                      columnIndex * this.CellSideLength +
                                      rowIndex * this.GridWidth * this.CellSideLength) * 4;
                    var colorIndex = Convert.ToInt32(byteOffset / 4);

                    try
                    {
                        cell.Colors.Add(this.Mosaic.Colors[colorIndex]);
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

        private int getCurrentRowHeight(int rowNumber)
        {
            var currentHeight = this.CellSideLength;
            if (rowNumber == this.NumberOfRows - 1)
            {
                currentHeight = this.LastRowHeight;
            }

            return currentHeight;
        }

        private int getCurrentColumnWidth(int columnIndex)
        {
            var currentWidth = this.CellSideLength;
            if (columnIndex == this.NumberOfColumns - 1)
            {
                currentWidth = this.LastColumnWidth;
            }

            return currentWidth;
        }

        #endregion
    }
}
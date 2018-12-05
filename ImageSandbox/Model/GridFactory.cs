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

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the width of the grid.
        /// </summary>
        /// <value>
        ///     The width of the grid.
        /// </value>
        public int GridWidth { get; set; }

        /// <summary>
        ///     Gets or sets the height of the grid.
        /// </summary>
        /// <value>
        ///     The height of the grid.
        /// </value>
        public int GridHeight { get; set; }

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
                    this.numberOfRows++;
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
                Stroke = new SolidColorBrush(Color.FromArgb(100, 0, 0, 0)),
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
            
            for (var i = 0; i < this.NumberOfRows; i++)

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
                var currentWidth = this.CellSideLength;
                var currentHeight = this.CellSideLength;

                if (rowNumber == this.NumberOfRows - 1)
                {
                    currentHeight = this.LastRowHeight;
                }

                if (columnIndex == this.NumberOfColumns - 1)
                {
                    currentWidth = this.LastColumnWidth;
                }

                var cell = this.createCell(currentWidth, currentHeight);
                Grid.SetColumn(cell, columnIndex);
                Grid.SetRow(cell, rowNumber);
                grid.Name = "overlay";
                grid.Children.Add(cell);
            }
        }

        #endregion
    }
}
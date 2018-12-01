using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace ImageSandbox.Model
{
    public class GridFactory
    {
        private int numberOfRows;
        private int numberOfColumns;
        private int lastRowHeight;
        private int lastColumnWidth;

        public int GridWidth { get; set; }
        public int GridHeight { get; set; }
        public int CellSideLength { get; set; }

        public int LastRowHeight
        {
            get
            {
                this.lastRowHeight = this.GridHeight % this.CellSideLength;
                return this.lastRowHeight;
            }
        }

        public int LastColumnWidth
        {
            get
            {
                this.lastColumnWidth = this.GridWidth % this.CellSideLength;
                return this.lastColumnWidth;
            }
        }

        public int NumberOfRows
        {
            get
            {
                this.numberOfRows = this.GridHeight / this.CellSideLength;
                return this.numberOfRows;
            }
        }

        public int NumberOfColumns {
            get
            {
                this.numberOfColumns = this.GridWidth / this.CellSideLength;
                return this.numberOfColumns;
            }
        }

        private Rectangle createCell(int width, int height)
        {
            var cell = new Rectangle {
                Width = width,
                Height = height,
                Stroke = new SolidColorBrush(Color.FromArgb(100, 0, 0, 0)),
                StrokeThickness = 1,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
            };
            return cell;
        }

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

            for (var i = 0; i <= this.NumberOfColumns; i++)
            {
                grid.ColumnDefinitions.Add(
                    new ColumnDefinition {
                        Width = GridLength.Auto
                });
            }

            for (var rowIndex = 0; rowIndex <= this.NumberOfRows; rowIndex++)
            {
                this.addRowToGrid(grid, rowIndex);
            }
            
            return grid;
        }

        private void addRowToGrid(Grid grid, int rowNumber)
        {
            for (var columnIndex = 0; columnIndex <= this.NumberOfColumns; columnIndex++)
            {
                var currentWidth = this.CellSideLength;
                var currentHeight = this.CellSideLength;

                if (rowNumber == this.NumberOfRows)
                {
                    currentHeight = this.LastRowHeight;
                }

                if (columnIndex == this.NumberOfColumns)
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
    }
}

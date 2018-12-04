using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        private int PixelIndex{ get; set; }
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

        public async Task<WriteableBitmap> SetCellData()
        {
            this.Colors = this.SourceImage.GetPixelColors();
            this.Cells =  this.calculateCellAttributes();
            await this.writeToBitmapAverageColor();
            return this.MosaicImage;
        }

        private List<Cell> calculateCellAttributes()
        {
            List<Cell> cells = new List<Cell>();
            this.PixelIndex = 0;

            for (var rowIndex = 0; rowIndex <= this.GridFactory.NumberOfRows; rowIndex++)
            {
                cells.AddRange(this.getRowOfCells(rowIndex));
            }

            return cells;
        }

        private List<Cell> getRowOfCells(int rowIndex)
        {
            var cells = new List<Cell>();
            for (var columnIndex = 0; columnIndex <= this.GridFactory.NumberOfColumns; columnIndex++)
            {
                var currentHeight = 0;
                var currentWidth = 0;
                var cell = new Cell();
                
                if (columnIndex == this.GridFactory.NumberOfColumns)
                {
                    currentWidth = this.GridFactory.LastColumnWidth;
                }
                else
                {
                    currentWidth = this.GridFactory.CellSideLength;
                }

                if (rowIndex == this.GridFactory.NumberOfRows)
                {
                    currentHeight = this.GridFactory.LastRowHeight;
                }
                else
                {
                    currentHeight = this.GridFactory.CellSideLength;
                }

                for (var pixelRow = 0; pixelRow < currentHeight; pixelRow++)
                {
                    for (var pixelColumn = 0; pixelColumn < currentWidth; pixelColumn++)
                    {
                        cell.X = columnIndex;
                        cell.Y = rowIndex;
                        cell.Colors.Add(this.Colors[this.PixelIndex]);
                        cell.PixelIndexes.Add(this.PixelIndex);
                        this.PixelIndex++;
                    }
                }
                cells.Add(cell);
            }
            return cells;
        }
        private async Task<WriteableBitmap> writeToBitmapAverageColor()
        {
            var sourcePixels = this.SourceImage.PixelWidth * this.SourceImage.PixelHeight;
            var mosaic = new WriteableBitmap(this.SourceImage.PixelWidth, this.SourceImage.PixelHeight);
            using (var stream = mosaic.PixelBuffer.AsStream())
            {
                var buffer = this.setUpSolidMosaicPixelData();
               
                await stream.WriteAsync(buffer, 0, sourcePixels);
                return mosaic;
            }
        }

        private byte[] setUpSolidMosaicPixelData()
        {
            var buffer = new byte[(this.SourceImage.PixelWidth * this.SourceImage.PixelHeight) * 4];
            foreach (var current in this.Cells)
            {
                Debug.WriteLine($"Pixels In Cell = {current.PixelIndexes.Count}");
                foreach (var currentPixel in current.PixelIndexes)
                {
                    buffer[currentPixel] = current.AverageColor.B;
                    buffer[currentPixel + 1] = current.AverageColor.G;
                    buffer[currentPixel + 2] = current.AverageColor.R;
                    buffer[currentPixel + 3] = 0;

                    Debug.WriteLine($"Index = {currentPixel}----{Environment.NewLine}" +
                                    $"RGB = {current.AverageColor.R}, {current.AverageColor.G}, {current.AverageColor.B}{Environment.NewLine}" +
                                    $"Current Cell = ({current.X},{current.Y})");
                }
            }
            return buffer;
        }
    }




//            var pixelIndex = 0;
//            var newCell = new Cell();
//            var pixelIndexes = new List<int>();
//            var cellSideLength = this.GridFactory.CellSideLength;
//            var pixelsPerCell = cellSideLength * cellSideLength;
//            Debug.WriteLine($"Pixel Width = {this.SourceImage.PixelWidth}");
//            Debug.WriteLine($"Cell Side Length = {cellSideLength}");
//            for (var i = 0; i < this.CellSideLength; i++)
//            {
//                for (var j = 0; j < this.CellSideLength; j++)
//                {
//                        Debug.WriteLine($"Pixel Index = {pixelIndex}{Environment.NewLine}" +
//                                        $"   Pixel In Cell = ({j}, {i}){Environment.NewLine}" +
//                                        $"      of Cell = {index}{Environment.NewLine}" +
//                                        $"---");
//                    pixelIndexes.Add(pixelIndex);
//                    this.Cells.Add(newCell);
//                   
//                }
//            }

//            return pixelIndexes;
        }
        #endregion
    


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


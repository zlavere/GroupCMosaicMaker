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





        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SolidMosaic" /> class.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        /// <param name="mosaicImage">The mosaic image.</param>
        /// <param name="cellSideLength">Length of the cell side.</param>
        /// <param name="gridFactory">The grid factory.</param>
        public SolidMosaic(WriteableBitmap sourceImage,
            GridFactory gridFactory)
            : base(sourceImage, gridFactory)
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
                this.Cells.AddRange(this.CreateRow(rowIndex));
            }
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
                        buffer[offset + 3] = 255;
                    }); 
                });

            return buffer;
        }

        #endregion
    }
}
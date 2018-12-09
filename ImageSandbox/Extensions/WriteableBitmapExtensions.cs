using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace ImageSandbox.Extensions
{
    public static class WriteableBitmapExtensions
    {
        #region Methods

        /// <summary>
        ///     Gets the pixel colors of a WriteableBitmap.
        /// </summary>
        /// <param name="wb">The wb.</param>
        /// <returns>
        ///     A List of colors in a WriteableBitmap.
        /// </returns>
        public static async Task<List<Color>> GetPixelColors(this WriteableBitmap wb)
        {
            using (var stream = wb.PixelBuffer.AsStream())
            {
                var pixelsCount = (wb.PixelWidth * wb.PixelHeight);
                var bytesCount = pixelsCount * 4;
                var pixelBuffer = new byte[bytesCount];
                await stream.ReadAsync(pixelBuffer, 0, bytesCount);
                var colors = new List<Color>();
                for (var i = 0; i < pixelsCount; i++)
                {
                    var offsetPosition = i * 4;
                    var pixelBlue = pixelBuffer[offsetPosition];
                    var pixelGreen = pixelBuffer[offsetPosition + 1];
                    var pixelRed = pixelBuffer[offsetPosition + 2];

                    colors.Add(Color.FromArgb(255, pixelRed, pixelGreen, pixelBlue));  
                }
                return colors;
            }
        }

        public static WriteableBitmap ResizeImage(this WriteableBitmap sourceImage, int blockSize, int origWidth, int origHeight)
        {
            var ratioX = blockSize / (float)origWidth;
            var ratioY = blockSize / (float)origHeight;
            var ratio = Math.Min(ratioX, ratioY);
            var newHeight = (int)(origHeight * ratio);
            var newWidth = (int)(origWidth * ratio);

            var newBitmap = new WriteableBitmap(newWidth, newHeight);

            var source = sourceImage.PixelBuffer.AsStream().AsRandomAccessStream();
            newBitmap.SetSource(source);

            return newBitmap;
        }

        #endregion
    }
}
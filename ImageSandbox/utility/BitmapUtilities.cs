using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace ImageSandbox.Utility
{
    public static class BitmapUtilities
    {
        /// <summary>
        /// Copies the bitmap from file.
        /// </summary>
        /// <param name="imageFile">The image file.</param>
        /// <returns></returns>
        public static async Task<BitmapImage> CopyBitmapFromFile(StorageFile imageFile)
        {
            IRandomAccessStream inputStream = await imageFile.OpenReadAsync();
            var newImage = new BitmapImage();
            newImage.SetSource(inputStream);
            return newImage;
        }



        /// <summary>
        /// Gets the pixel color.
        /// </summary>
        /// <param name="pixels">The pixels.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>
        /// The pixels color.
        /// </returns>
        public static Color GetPixelBgra8(byte[] pixels, int x, int y, uint width, uint height)
        {
            var offset = (x * (int)width + y) * 4;
            var r = pixels[offset + 2];
            var g = pixels[offset + 1];
            var b = pixels[offset + 0];
            return Color.FromArgb(0, r, g, b);
        }

        /// <summary>
        /// Gets the average color of the pixels in an image.
        /// </summary>
        /// <param name="pixels">The pixels.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public static Color GetAveragePixelColor(byte[] pixels, uint width, uint height)
        {
            var colorCollection = new List<Color>();
            var totalPixels = width * height;
            var currentColor = 0;
            var totalR = 0;
            var totalG = 0;
            var totalB = 0;
            for (int x = 0; x < totalPixels; x++)
            {
                if (currentColor == 0)
                {
                    totalB += (int)pixels[x];
                }
                if (currentColor == 1)
                {
                    totalG += (int)pixels[x];
                }
                if (currentColor == 2)
                {
                    totalR += (int) pixels[x];
                }
                if (currentColor != 2)
                {
                    currentColor++;
                }
                else
                {
                    currentColor = 0;
                }
                
            }

            var averageR = totalR / totalPixels;
            var averageG = totalG / totalPixels;
            var averageB = totalB / totalPixels;
            Color averageColor = Color.FromArgb(0, (byte)averageR, (byte)averageG, (byte)averageB);
            return averageColor;
        }

        /// <summary>
        /// Sets the pixel color bgra8.
        /// </summary>
        /// <param name="pixels">The pixels.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="color">The color.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public static void SetPixelBgra8(byte[] pixels, int x, int y, Color color, uint width, uint height)
        {
            var offset = (x * (int)width + y) * 4;
            pixels[offset + 2] = color.R;
            pixels[offset + 1] = color.G;
            pixels[offset + 0] = color.B;
        }

        public static async Task<WriteableBitmap> DecodeStorageFileToBitmap(StorageFile file)
        {
            var copyBitmapImage = await BitmapUtilities.CopyBitmapFromFile(file);

            using (var fileStream = await file.OpenAsync(FileAccessMode.Read))
            {
                var decoder = await BitmapDecoder.CreateAsync(fileStream);
                var transform = new BitmapTransform
                {
                    ScaledWidth = Convert.ToUInt32(copyBitmapImage.PixelWidth),
                    ScaledHeight = Convert.ToUInt32(copyBitmapImage.PixelHeight)
                };

                //this.DpiX = decoder.DpiX;
                //this.DpiY = decoder.DpiY;

                var pixelData = await decoder.GetPixelDataAsync(
                    BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Straight,
                    transform,
                    ExifOrientationMode.IgnoreExifOrientation,
                    ColorManagementMode.DoNotColorManage
                );

                var sourcePixels = pixelData.DetachPixelData();

                var returnImage = new WriteableBitmap((int)decoder.PixelWidth, (int)decoder.PixelHeight);
                using (var writeStream = returnImage.PixelBuffer.AsStream())
                {
                    await writeStream.WriteAsync(sourcePixels, 0, sourcePixels.Length);
                    return returnImage;
                }
            }
        }

    }
}

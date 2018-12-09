using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace ImageSandbox.Utility
{
    /// <summary>
    ///     This class contains utility methods for working with bitmap images.
    /// </summary>
    public static class BitmapUtilities
    {
        #region Methods

        /// <summary>
        ///     Copies the bitmap from file.
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
        ///     Gets the pixel color.
        /// </summary>
        /// <param name="pixels">The pixels.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>
        ///     The pixels color.
        /// </returns>
        public static Color GetPixelBgra8(byte[] pixels, int x, int y, uint width, uint height)
        {
            var offset = (y * width + x) * 4;
            var r = pixels[offset + 2];
            var g = pixels[offset + 1];
            var b = pixels[offset + 0];
            return Color.FromArgb(0, r, g, b);
        }

        /// <summary>
        ///     Sets the pixel color bgra8.
        /// </summary>
        /// <param name="pixels">The pixels.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="color">The color.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public static void SetPixelBgra8(byte[] pixels, int x, int y, Color color, uint width, uint height)
        {
            var offset = (y * width + x) * 4;
            pixels[offset + 2] = color.R;
            pixels[offset + 1] = color.G;
            pixels[offset + 0] = color.B;
        }

        /// <summary>
        ///     Converts to bitmap.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public static async Task<WriteableBitmap> DecodeStorageFileToBitmap(StorageFile file)
        {
            var copyBitmapImage = await CopyBitmapFromFile(file);

            using (var fileStream = await file.OpenAsync(FileAccessMode.Read))
            {
                var decoder = await BitmapDecoder.CreateAsync(fileStream);
                var transform = new BitmapTransform {
                    ScaledWidth = Convert.ToUInt32(copyBitmapImage.PixelWidth),
                    ScaledHeight = Convert.ToUInt32(copyBitmapImage.PixelHeight)
                };

                var pixelData = await decoder.GetPixelDataAsync(
                    BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Straight,
                    transform,
                    ExifOrientationMode.IgnoreExifOrientation,
                    ColorManagementMode.DoNotColorManage
                );

                var sourcePixels = pixelData.DetachPixelData();

                var returnImage = new WriteableBitmap((int) decoder.PixelWidth, (int) decoder.PixelHeight);
                using (var writeStream = returnImage.PixelBuffer.AsStream())
                {
                    await writeStream.WriteAsync(sourcePixels, 0, sourcePixels.Length);
                    return returnImage;
                }
            }
        }

        /// <summary>
        ///     Converts to a bitmap to black and white.
        /// </summary>
        /// <param name="sourceBitmap">The source bitmap.</param>
        /// <returns>
        ///     A black and white WriteableBitmap.
        /// </returns>
        public static WriteableBitmap ConvertToBlackAndWhite(WriteableBitmap sourceBitmap)
        {
            var height = (uint) sourceBitmap.PixelHeight;
            var width = (uint) sourceBitmap.PixelWidth;
            var imageAsArray = sourceBitmap.PixelBuffer.ToArray();

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var currentColor = GetPixelBgra8(imageAsArray, x, y, width, height);
                    var changedColor = toBlackOrWhite(currentColor);
                    SetPixelBgra8(imageAsArray, x, y, changedColor, width, height);
                }
            }

            var returnBitmap = new WriteableBitmap(sourceBitmap.PixelWidth, sourceBitmap.PixelHeight);
            var pixelBufferStream = returnBitmap.PixelBuffer.AsStream();
            foreach (var currentByte in imageAsArray)
            {
                pixelBufferStream.WriteByte(currentByte);
            }

            return returnBitmap;
        }

        private static Color toBlackOrWhite(Color baseColor)
        {
            var white = 255;
            var black = 0;
            int blue = baseColor.B;
            int green = baseColor.G;
            int red = baseColor.R;
            var total = blue + green + red;
            var returnColor = new Color();
            if (total > 384)
            {
                returnColor.B = (byte) white;
                returnColor.G = (byte) white;
                returnColor.R = (byte) white;
                returnColor.A = 255;
            }
            else
            {
                returnColor.B = (byte) black;
                returnColor.G = (byte) black;
                returnColor.R = (byte) black;
                returnColor.A = 255;
            }

            return returnColor;
        }

        #endregion
    }
}
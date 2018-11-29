using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using ImageSandbox.Utility;

namespace ImageSandbox.IO
{
    /// <summary>
    ///     Opens an image and returns
    /// </summary>
    public class ImageReader
    {
        #region Properties

        /// <summary>
        ///     Gets the dpi for the x axis..
        /// </summary>
        /// <value>
        ///     The dpi of the x axis.
        /// </value>
        public double DpiX { get; private set; }

        /// <summary>
        ///     Gets the dpi for the y axis.
        /// </summary>
        /// <value>
        ///     The dpi of the y axis.
        /// </value>
        public double DpiY { get; private set; }

        #endregion

        #region Methods

        public async Task<WriteableBitmap> OpenImage()
        {
            var sourceImageFile = await this.selectSourceImageFile();
            var copyBitmapImage = await BitmapUtilities.CopyBitmapFromFile(sourceImageFile);

            using (var fileStream = await sourceImageFile.OpenAsync(FileAccessMode.Read))
            {
                var decoder = await BitmapDecoder.CreateAsync(fileStream);
                var transform = new BitmapTransform {
                    ScaledWidth = Convert.ToUInt32(copyBitmapImage.PixelWidth),
                    ScaledHeight = Convert.ToUInt32(copyBitmapImage.PixelHeight)
                };

                this.DpiX = decoder.DpiX;
                this.DpiY = decoder.DpiY;

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

        private async Task<StorageFile> selectSourceImageFile()
        {
            var openPicker = new FileOpenPicker {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".bmp");

            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
            }

            return file;
        }

        #endregion
    }
}
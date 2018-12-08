using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media.Imaging;

namespace ImageSandbox.IO
{
    /// <summary>
    ///     Saves an image as varying file types.
    /// </summary>
    public class ImageWriter
    {
        #region Methods

        /// <summary>
        ///     Saves the image.
        /// </summary>
        /// <param name="imageToSave">The image to save.</param>
        /// <param name="dpiX">The dpi x.</param>
        /// <param name="dpiY">The dpi y.</param>
        public async void SaveImage(WriteableBitmap imageToSave, double dpiX, double dpiY)
        {
            try
            {
                var saveFile = await this.selectSaveFile();
                if (saveFile.FileType == ".bmp")
                {
                    await this.saveAsBmp(saveFile, imageToSave, dpiX, dpiY);
                }

                if (saveFile.FileType == ".jpg")
                {
                    await this.saveAsJpg(saveFile, imageToSave, dpiX, dpiY);
                }

                if (saveFile.FileType == ".png")
                {
                    await this.saveAsPng(saveFile, imageToSave, dpiX, dpiY);
                }
            }
            catch (NullReferenceException)
            {
                //TODO Nothing happens.
            }


        }

        private async Task saveAsBmp(StorageFile saveFile, WriteableBitmap imageToSave, double dpiX, double dpiY)
        {
            if (saveFile != null)
            {
                var stream = await saveFile.OpenAsync(FileAccessMode.ReadWrite);
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);

                var pixelStream = imageToSave.PixelBuffer.AsStream();
                var pixels = new byte[pixelStream.Length];
                await pixelStream.ReadAsync(pixels, 0, pixels.Length);

                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore,
                    (uint) imageToSave.PixelWidth,
                    (uint) imageToSave.PixelHeight, dpiX, dpiY, pixels);
                await encoder.FlushAsync();

                stream.Dispose();
            }
        }

        private async Task saveAsJpg(StorageFile saveFile, WriteableBitmap imageToSave, double dpiX, double dpiY)
        {
            if (saveFile != null)
            {
                var stream = await saveFile.OpenAsync(FileAccessMode.ReadWrite);
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);

                var pixelStream = imageToSave.PixelBuffer.AsStream();
                var pixels = new byte[pixelStream.Length];
                await pixelStream.ReadAsync(pixels, 0, pixels.Length);

                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore,
                    (uint) imageToSave.PixelWidth,
                    (uint) imageToSave.PixelHeight, dpiX, dpiY, pixels);
                await encoder.FlushAsync();

                stream.Dispose();
            }
        }

        private async Task saveAsPng(StorageFile saveFile, WriteableBitmap imageToSave, double dpiX, double dpiY)
        {
            if (saveFile != null)
            {
                var stream = await saveFile.OpenAsync(FileAccessMode.ReadWrite);
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);

                var pixelStream = imageToSave.PixelBuffer.AsStream();
                var pixels = new byte[pixelStream.Length];
                await pixelStream.ReadAsync(pixels, 0, pixels.Length);

                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore,
                    (uint) imageToSave.PixelWidth,
                    (uint) imageToSave.PixelHeight, dpiX, dpiY, pixels);
                await encoder.FlushAsync();

                stream.Dispose();
            }
        }

        private async Task<StorageFile> selectSaveFile()
        {
            var savePicker = new FileSavePicker {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            savePicker.FileTypeChoices.Add("JPG", new List<string> {".jpg"});
            savePicker.FileTypeChoices.Add("PNG", new List<string> {".png"});
            savePicker.FileTypeChoices.Add("BMP", new List<string> {".bmp"});
            savePicker.FileTypeChoices.Add("All Types", new List<string> {".jpg", ".png", ".bmp"});

            var file = await savePicker.PickSaveFileAsync();

            return file;
        }

        #endregion
    }
}
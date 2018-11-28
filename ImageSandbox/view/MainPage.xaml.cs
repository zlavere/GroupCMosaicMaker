﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using ImageSandbox.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ImageSandbox

{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Data members

        private double dpiX;
        private double dpiY;
        private WriteableBitmap modifiedImage;

        public MainPageViewModel ViewModel { get; set; }

        #endregion

        #region Constructors

        public MainPage()
        {
            this.InitializeComponent();
            this.ViewModel = new MainPageViewModel();
            DataContext = this.ViewModel;

            ApplicationView.PreferredLaunchViewSize = new Size(1080, 720);

            this.modifiedImage = null;
            this.dpiX = 0;
            this.dpiY = 0;
        }

        #endregion

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            this.saveWritableBitmap();
        }

        private async void saveWritableBitmap()
        {
            var fileSavePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                SuggestedFileName = "image"
            };
            fileSavePicker.FileTypeChoices.Add("PNG files", new List<string> { ".png" });
            var savefile = await fileSavePicker.PickSaveFileAsync();

            if (savefile != null)
            {
                var stream = await savefile.OpenAsync(FileAccessMode.ReadWrite);
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);

                var pixelStream = this.modifiedImage.PixelBuffer.AsStream();
                var pixels = new byte[pixelStream.Length];
                await pixelStream.ReadAsync(pixels, 0, pixels.Length);

                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore,
                    (uint)this.modifiedImage.PixelWidth,
                    (uint)this.modifiedImage.PixelHeight, this.dpiX, this.dpiY, pixels);
                await encoder.FlushAsync();

                stream.Dispose();
            }
        }


        private async void loadButton_Click(object sender, RoutedEventArgs e)
        {
            //var sourceImageFile = await this.selectSourceImageFile();
            //var copyBitmapImage = await this.MakeACopyOfTheFileToWorkOn(sourceImageFile);

            //using (var fileStream = await sourceImageFile.OpenAsync(FileAccessMode.Read))
            //{
            //    var decoder = await BitmapDecoder.CreateAsync(fileStream);
            //    var transform = new BitmapTransform
            //    {
            //        ScaledWidth = Convert.ToUInt32(copyBitmapImage.PixelWidth),
            //        ScaledHeight = Convert.ToUInt32(copyBitmapImage.PixelHeight)
            //    };

            //    this.dpiX = decoder.DpiX;
            //    this.dpiY = decoder.DpiY;

            //    var pixelData = await decoder.GetPixelDataAsync(
            //        BitmapPixelFormat.Bgra8,
            //        BitmapAlphaMode.Straight,
            //        transform,
            //        ExifOrientationMode.IgnoreExifOrientation,
            //        ColorManagementMode.DoNotColorManage
            //        );

            //    var sourcePixels = pixelData.DetachPixelData();

            //    this.modifiedImage = new WriteableBitmap((int)decoder.PixelWidth, (int)decoder.PixelHeight);
            //    using (var writeStream = this.modifiedImage.PixelBuffer.AsStream())
            //    {
            //        await writeStream.WriteAsync(sourcePixels, 0, sourcePixels.Length);
            //        this.outputImage.Source = this.modifiedImage;
            //    }
            //}

        }

        private void giveImageRedTint(byte[] sourcePixels, uint imageWidth, uint imageHeight)
        {
            for (var i = 0; i < imageHeight; i++)
            {
                for (var j = 0; j < imageWidth; j++)
                {
                    var pixelColor = this.getPixelBgra8(sourcePixels, i, j, imageWidth, imageHeight);
                    pixelColor.R = 255;
                    this.setPixelBgra8(sourcePixels, i, j, pixelColor, imageWidth, imageHeight);
                }
            }
        }

        private async Task<StorageFile> selectSourceImageFile()
        {
            var openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".bmp");

            var file = await openPicker.PickSingleFileAsync();

            return file;
        }

        private async Task<BitmapImage> MakeACopyOfTheFileToWorkOn(StorageFile imageFile)
        {
            IRandomAccessStream inputstream = await imageFile.OpenReadAsync();
            var newImage = new BitmapImage();
            newImage.SetSource(inputstream);
            return newImage;
        }

        private Color getPixelBgra8(byte[] pixels, int x, int y, uint width, uint height)
        {
            var offset = (x * (int)width + y) * 4;
            var r = pixels[offset + 2];
            var g = pixels[offset + 1];
            var b = pixels[offset + 0];
            return Color.FromArgb(0, r, g, b);
        }

        private void setPixelBgra8(byte[] pixels, int x, int y, Color color, uint width, uint height)
        {
            var offset = (x * (int)width + y) * 4;
            pixels[offset + 2] = color.R;
            pixels[offset + 1] = color.G;
            pixels[offset + 0] = color.B;
        }
    }
}

using System;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using ImageSandbox.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ImageSandbox.View

{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Properties

        public MainPageViewModel ViewModel { get; set; }

        #endregion

        #region Constructors

        public MainPage()
        {
            this.InitializeComponent();
            this.ViewModel = new MainPageViewModel();
            DataContext = this.ViewModel;

            ApplicationView.PreferredLaunchViewSize = new Size(1080, 720);
        }

        #endregion

        #region Methods

        private Color getPixelBgra8(byte[] pixels, int x, int y, uint width, uint height)
        {
            var offset = (x * (int) width + y) * 4;
            var r = pixels[offset + 2];
            var g = pixels[offset + 1];
            var b = pixels[offset + 0];
            return Color.FromArgb(0, r, g, b);
        }

        private void setPixelBgra8(byte[] pixels, int x, int y, Color color, uint width, uint height)
        {
            var offset = (x * (int) width + y) * 4;
            pixels[offset + 2] = color.R;
            pixels[offset + 1] = color.G;
            pixels[offset + 0] = color.B;
        }



        #endregion

        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
           

                var picker = new Windows.Storage.Pickers.FileOpenPicker();

                picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
                picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
                picker.FileTypeFilter.Add(".jpg");

                Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
                if (file != null)
                {
                    var fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);


                    // create a new stream and encoder for the new image
                    InMemoryRandomAccessStream ras = new InMemoryRandomAccessStream();
                    BitmapEncoder enc = await BitmapEncoder.CreateForTranscodingAsync(ras, decoder);

                    // convert the entire bitmap to a 100px by 100px bitmap
                    enc.BitmapTransform.ScaledHeight = 100;
                    enc.BitmapTransform.ScaledWidth = 100;


                    BitmapBounds bounds = new BitmapBounds();
                    bounds.Height = 50;
                    bounds.Width = 50;
                    bounds.X = 50;
                    bounds.Y = 50;
                    enc.BitmapTransform.Bounds = bounds;

                    // write out to the stream
                    try
                    {
                        await enc.FlushAsync();
                    }
                    catch (Exception ex)
                    {
                        string s = ex.ToString();
                    }

                    // render the stream to the screen
                    BitmapImage bImg = new BitmapImage();
                    bImg.SetSource(ras);
                    this.testImage.Source = bImg; // image element in xaml

                
            }
        }
    }
}
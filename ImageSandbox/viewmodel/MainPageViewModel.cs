using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using ImageSandbox.IO;
using ImageSandbox.Utility;

namespace ImageSandbox.ViewModel
{
    /// <summary>
    ///     Handles communications between the view and model in a testable fashion.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class MainPageViewModel : INotifyPropertyChanged
    {

        private WriteableBitmap currentlyDisplayedImage;
        private WriteableBitmap currentlyDisplayedGridLines;
        private WriteableBitmap currentlyDisplayedMosaic;

        private int mosaicPixelSize;

        private double currentDpiX;
        private double currentDpiY;

        private bool mosaicType;

        /// <summary>
        /// Gets or sets the load image command.
        /// </summary>
        /// <value>
        /// The load image command.
        /// </value>
        public RelayCommand LoadImageCommand { get; set; }

        /// <summary>
        /// Gets or sets the save image command.
        /// </summary>
        /// <value>
        /// The save image command.
        /// </value>
        public RelayCommand SaveImageCommand { get; set; }

        /// <summary>
        /// Gets or sets the create mosaic command.
        /// </summary>
        /// <value>
        /// The create mosaic command.
        /// </value>
        public RelayCommand CreateMosaicCommand { get; set; }

        /// <summary>
        /// Gets or sets the currently displayed image.
        /// </summary>
        /// <value>
        /// The currently displayed image.
        /// </value>
        /// <exception cref="ArgumentNullException"></exception>
        public WriteableBitmap CurrentlyDisplayedImage
        {
            get => this.currentlyDisplayedImage;
            set
            {
                this.currentlyDisplayedImage = value ?? throw new ArgumentNullException();
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the currently displayed grid lines.
        /// </summary>
        /// <value>
        /// The currently displayed grid lines.
        /// </value>
        /// <exception cref="ArgumentNullException"></exception>
        public WriteableBitmap CurrentlyDisplayedGridLines
        {
            get => this.currentlyDisplayedGridLines;
            set
            {
                this.currentlyDisplayedGridLines = value ?? throw new ArgumentNullException();
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the currently displayed mosaic.
        /// </summary>
        /// <value>
        /// The currently displayed mosaic.
        /// </value>
        /// <exception cref="ArgumentNullException"></exception>
        public WriteableBitmap CurrentlyDisplayedMosaic
        {
            get => this.currentlyDisplayedMosaic;
            set
            {
                this.currentlyDisplayedMosaic = value ?? throw new ArgumentNullException();
                this.canSaveImage(true);
                this.canCreateMosaic(true);
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the size of the mosaic pixel.
        /// </summary>
        /// <value>
        /// The size of the mosaic pixel.
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public int MosaicPixelSize
        {
            get => this.mosaicPixelSize;
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException();
                }

                this.mosaicPixelSize = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [mosaic type].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [mosaic type]; otherwise, <c>false</c>.
        /// </value>
        public bool MosaicType
        {
            get => this.mosaicType;
            set
            {
                this.mosaicType = value;
                this.OnPropertyChanged();
            }
        }


        #region Constructors

        public MainPageViewModel()
        {
            this.loadCommands();
            this.currentDpiX = 0;
            this.currentDpiY = 0;
            this.currentlyDisplayedImage = null;
            this.currentlyDisplayedGridLines = null;
            this.currentlyDisplayedMosaic = null;
        }

        #endregion

        #region Methods

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void loadCommands()
        {
            this.LoadImageCommand = new RelayCommand(this.loadImage, this.canLoadImage);
            this.SaveImageCommand = new RelayCommand(this.saveImage, this.canSaveImage);
            this.canLoadImage(true);
            this.CreateMosaicCommand = new RelayCommand(this.createMosaic, this.canCreateMosaic);
        }

        private async void loadImage(object obj)
        {
            //TODO
            ImageReader readImage = new ImageReader();
            var results = await readImage.OpenImage();
            this.CurrentlyDisplayedImage = results;
            this.currentDpiX = readImage.DpiX;
            this.currentDpiY = readImage.DpiY;

        }

        private bool canLoadImage(object obj)
        {
            return true;
        }

        private void saveImage(object obj)
        {

        }

        private bool canSaveImage(object obj)
        {
            return this.currentlyDisplayedMosaic != null;
        }

        private void createMosaic(object obj)
        {

        }

        private bool canCreateMosaic(object obj)
        {
            return this.currentlyDisplayedImage != null;
        }

        #endregion
    }
}
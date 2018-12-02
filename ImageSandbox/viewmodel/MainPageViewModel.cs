using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using ImageSandbox.Extensions;
using ImageSandbox.IO;
using ImageSandbox.Model;
using ImageSandbox.Utility;

namespace ImageSandbox.ViewModel
{
    /// <summary>
    ///     Handles communications between the view and model in a testable fashion.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class MainPageViewModel : INotifyPropertyChanged
    {
        #region Data members

        private WriteableBitmap sourceImage;
        private WriteableBitmap currentlyDisplayedGridLines;
        private WriteableBitmap mosaicImage;
        private int cellSideLength;

        private double currentDpiX;
        private double currentDpiY;

        private bool mosaicType;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the load image command.
        /// </summary>
        /// <value>
        ///     The load image command.
        /// </value>
        public RelayCommand LoadImageCommand { get; set; }

        /// <summary>
        ///     Gets or sets the save image command.
        /// </summary>
        /// <value>
        ///     The save image command.
        /// </value>
        public RelayCommand SaveImageCommand { get; set; }

        /// <summary>
        ///     Gets or sets the create mosaic command.
        /// </summary>
        /// <value>
        ///     The create mosaic command.
        /// </value>
        public RelayCommand CreateMosaicCommand { get; set; }

        /// <summary>
        /// Toggles the grid.
        /// </summary>
        /// <value>
        /// The toggle grid command.
        /// </value>
        public RelayCommand ToggleGridCommand { get; set; }

        public RelayCommand ShowGridCommand { get; set; }
        
        /// <summary>
        ///     Gets or sets the currently displayed image.
        /// </summary>
        /// <value>
        ///     The currently displayed image.
        /// </value>
        /// <exception cref="ArgumentNullException"></exception>
        public WriteableBitmap SourceImage
        {
            get => this.sourceImage;
            set
            {
                this.sourceImage = value ?? throw new ArgumentNullException();
                ActiveImage.Image = this.sourceImage;
                this.canCreateMosaic(true);
                this.OnPropertyChanged();
            }
        }

        public Mosaic Mosaic { get; set; }

        /// <summary>
        ///     Gets or sets the currently displayed grid lines.
        /// </summary>
        /// <value>
        ///     The currently displayed grid lines.
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
        ///     Gets or sets the currently displayed mosaic.
        /// </summary>
        /// <value>
        ///     The currently displayed mosaic.
        /// </value>
        /// <exception cref="ArgumentNullException"></exception>
        public WriteableBitmap MosaicImage
        {
            get => this.mosaicImage;
            set
            {
                mosaicImage = value ?? throw new ArgumentNullException();
                this.CreateMosaicCommand.OnCanExecuteChanged();
                this.SaveImageCommand.OnCanExecuteChanged();
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the size of the mosaic pixel.
        /// </summary>
        /// <value>
        ///     The size of the mosaic pixel.
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public int CellSideLength
        {
            get => this.cellSideLength;
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException();
                }

                this.cellSideLength = value;
                this.CreateMosaicCommand.OnCanExecuteChanged();
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether [mosaic type].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [mosaic type]; otherwise, <c>false</c>.
        /// </value>
        public bool MosaicType
        {
            get => this.mosaicType;
            set
            {
                this.mosaicType = value;
                this.CreateMosaicCommand.OnCanExecuteChanged();
                this.OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the grid factory.</summary>
        /// <value>The grid factory.</value>
        public GridFactory GridFactory { get; set; }

        #endregion

        #region Constructors

        public MainPageViewModel()
        {
            this.loadCommands();
            this.GridFactory = new GridFactory();
            this.currentDpiX = 0;
            this.currentDpiY = 0;
            this.sourceImage = null;
            this.currentlyDisplayedGridLines = null;
            this.mosaicImage = null;
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
            this.LoadImageCommand = new RelayCommand(this.loadImage, canAlwaysExecute);
            this.SaveImageCommand = new RelayCommand(this.saveImage, this.canSaveImage);
            this.CreateMosaicCommand = new RelayCommand(this.createMosaic, this.canCreateMosaic);
        }

        private async void loadImage(object obj)
        {
            var readImage = new ImageReader();
            var results = await readImage.OpenImage();
            this.SourceImage = results;
            this.MosaicImage = results;
            this.currentDpiX = readImage.DpiX;
            this.currentDpiY = readImage.DpiY;
        }

        private static bool canAlwaysExecute(object obj)
        {
            return true;
        }

        public void SaveImage()
        {
            var imageWriter = new ImageWriter();
            imageWriter.SaveImage(this.MosaicImage, this.currentDpiX, this.currentDpiY);
        }

        private void saveImage(object obj)
        {
            var imageWriter = new ImageWriter();
            imageWriter.SaveImage(this.MosaicImage, this.currentDpiX, this.currentDpiY);
        }

        private bool canSaveImage(object obj)
        {
            return this.MosaicImage != null;
        }

        private async void createMosaic(object obj)
        {
            var colors = await this.MosaicImage.GetPixelColors();
            var cells = new Color[this.GridFactory.NumberOfRows, this.GridFactory.NumberOfColumns];
            var cellX = 0;
            
            for (var xPoint = 0; xPoint < colors.Count; xPoint++)
            {
                if (xPoint == this.CellSideLength - 1)
                {
                    cellX++;
                }

                var cellY = 0;
                for (var yPoint = 0; yPoint < this.MosaicImage.PixelWidth; yPoint++)
                {
                    if (yPoint == this.CellSideLength - 1)
                    {
                        cellY++;
                    }

                    if (yPoint == this.MosaicImage.PixelWidth - 1)
                    {
                        cellY = 0;
                    }
                }
            }
        }

        private bool canCreateMosaic(object obj)
        {
            return this.sourceImage != null && this.CellSideLength > 0;
        }

        private bool canOverlayGrid(object obj)
        {
            return this.GridFactory.CellSideLength > 0 && this.SourceImage != null;
        }

        #endregion
    }
}
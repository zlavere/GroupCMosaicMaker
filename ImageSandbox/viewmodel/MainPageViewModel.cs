using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
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

        private WriteableBitmap currentlyDisplayedImage;
        private WriteableBitmap currentlyDisplayedGridLines;
        private WriteableBitmap currentlyDisplayedMosaic;
        private ObservableCollection<Image> mosaicPalette;


        private int cellSideLength;
        private GridFactory gridFactory;

        private double currentDpiX;
        private double currentDpiY;

        private bool mosaicType;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current palette.
        /// </summary>
        /// <value>
        /// The current palette.
        /// </value>
        public PaletteReader CurrentPalette { get; set; }

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

        /// <summary>
        /// Gets or sets the load palette command.
        /// </summary>
        /// <value>
        /// The load palette command.
        /// </value>
        public RelayCommand LoadPaletteCommand { get; set; }

        public RelayCommand ShowGridCommand { get; set; }

        /// <summary>
        ///     Gets or sets the currently displayed image.
        /// </summary>
        /// <value>
        ///     The currently displayed image.
        /// </value>
        /// <exception cref="ArgumentNullException"></exception>
        public WriteableBitmap CurrentlyDisplayedImage
        {
            get => this.currentlyDisplayedImage;
            set
            {
                this.currentlyDisplayedImage = value ?? throw new ArgumentNullException();
                ActiveImage.Image = value;
                this.canCreateMosaic(true);
                this.OnPropertyChanged();
            }
        }

        public Mosaic Mosaic { get; set; }

        public GridFactory GridFactory
        {
            get
            {
                if (this.gridFactory == null)
                {
                    this.gridFactory = new GridFactory();
                }
                if (this.CurrentlyDisplayedImage != null)
                {
                    this.gridFactory.GridWidth = this.currentlyDisplayedImage.PixelWidth;
                    this.gridFactory.GridHeight = this.currentlyDisplayedImage.PixelHeight;
                    this.gridFactory.CellSideLength = this.CellSideLength;
                }
                return this.gridFactory;
            }
            set
            {
                this.gridFactory = value;
                this.OnPropertyChanged();
            }
        }

        public SolidMosaic SolidMosaic
        {
            get
            {
                if (this.CurrentlyDisplayedImage != null)
                {
                    this.solidMosaic = new SolidMosaic(this.CurrentlyDisplayedImage,  this.CurrentlyDisplayedMosaic, this.CellSideLength,
                        this.GridFactory);
                }
                return this.solidMosaic;
            }
            set
            {
                this.solidMosaic = value;
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
        public WriteableBitmap CurrentlyDisplayedMosaic
        {
            get => this.currentlyDisplayedMosaic;
            set
            {
                this.currentlyDisplayedMosaic = value ?? throw new ArgumentNullException();
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

        public ObservableCollection<Image> MosaicPalette
        {
            get => this.mosaicPalette;
            set
            {
                this.mosaicPalette = value ?? throw new ArgumentNullException();
                this.OnPropertyChanged();
            }
        }

        #endregion

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
            this.LoadImageCommand = new RelayCommand(this.loadImage, canAlwaysExecute);
            this.SaveImageCommand = new RelayCommand(this.saveImage, this.canSaveImage);
            this.CreateMosaicCommand = new RelayCommand(this.createMosaic, this.canCreateMosaic);
            this.LoadPaletteCommand = new RelayCommand(this.loadPalette, canAlwaysExecute);
        }

        private async void loadImage(object obj)
        {
            var readImage = new ImageReader();
            var results = await readImage.OpenImage();
            this.CurrentlyDisplayedImage = results;
            this.CurrentlyDisplayedMosaic = results;
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
            imageWriter.SaveImage(this.CurrentlyDisplayedMosaic, this.currentDpiX, this.currentDpiY);
        }

        private void saveImage(object obj)
        {
            var imageWriter = new ImageWriter();
            imageWriter.SaveImage(this.CurrentlyDisplayedMosaic, this.currentDpiX, this.currentDpiY);
        }

        private bool canSaveImage(object obj)
        {
            return this.CurrentlyDisplayedMosaic != null;
        }

        private void createMosaic(object obj)
        {
            this.SolidMosaic.SetCellData();
        }

        private bool canCreateMosaic(object obj)
        {
            return true;
        }

        private async void loadPalette(object obj)
        {
            await this.CurrentPalette.LoadPalette();

            this.MosaicPalette = new ObservableCollection<Image>(this.CurrentPalette.DisplayablePalette);
        }

        #endregion
    }
}
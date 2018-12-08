﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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

        private WriteableBitmap currentlyDisplayedImage;
        private WriteableBitmap currentlyDisplayedMosaic;
        private ObservableCollection<WriteableBitmap> mosaicPalette;
        private SolidMosaic solidMosaic;
        private int paletteSize;
        private int maxHeight;
        private WriteableBitmap blackAndWhiteMosaic;
        private WriteableBitmap normalMosaic;
        private bool isBlackAndWhite;
        private string originalImageFileType;

        private int cellSideLength;
        private GridFactory gridFactory;

        private double currentDpiX;
        private double currentDpiY;

        private bool mosaicType;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the current palette.
        /// </summary>
        /// <value>
        ///     The current palette.
        /// </value>
        public PaletteReader PaletteReader { get; set; }

        public int PaletteSize
        {
            get => this.paletteSize;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                this.paletteSize = value;
                this.CreateMosaicCommand.OnCanExecuteChanged();
                this.OnPropertyChanged();
            }
        }

        public Palette Palette { get; set; }

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
        ///     Toggles the grid.
        /// </summary>
        /// <value>
        ///     The toggle grid command.
        /// </value>
        public RelayCommand ToggleGridCommand { get; set; }

        /// <summary>
        ///     Gets or sets the load palette command.
        /// </summary>
        /// <value>
        ///     The load palette command.
        /// </value>
        public RelayCommand LoadPaletteCommand { get; set; }

        public RelayCommand ShowGridCommand { get; set; }

        public RelayCommand ToggleBlackAndWhiteCommand { get; set; }

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
                this.currentlyDisplayedImage = value;
                ActiveImage.Image = this.currentlyDisplayedImage;
                this.CreateMosaicCommand.OnCanExecuteChanged();
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
            get => this.solidMosaic;
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
                this.currentlyDisplayedMosaic = value;
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

        public ObservableCollection<WriteableBitmap> MosaicPalette
        {
            get => this.mosaicPalette;
            set
            {
                this.mosaicPalette = value ?? throw new ArgumentNullException();
                this.OnPropertyChanged();
            }
        }

        public int MaxImageHeight
        {
            get => this.maxHeight;
            set
            {
                if (value < 800 || this.CurrentlyDisplayedImage == null)
                {
                    this.maxHeight = 800;
                }
                else
                {
                    this.maxHeight = this.CurrentlyDisplayedImage.PixelHeight;
                }
            }
        }

        public bool IsBlackAndWhite
        {
            get => this.isBlackAndWhite;
            set
            {
                this.isBlackAndWhite = value;
                this.OnPropertyChanged();
                this.ToggleBlackAndWhiteCommand.OnCanExecuteChanged();
            }
        }

        public WriteableBitmap BlackAndWhiteMosaic
        {
            get => this.blackAndWhiteMosaic;
            set
            {
                this.blackAndWhiteMosaic = value;
                this.OnPropertyChanged();
                this.ToggleBlackAndWhiteCommand.OnCanExecuteChanged();
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
            this.currentlyDisplayedMosaic = null;
            this.PaletteReader = new PaletteReader();
            this.Palette = new Palette();
            this.MaxImageHeight = 800;
            this.IsBlackAndWhite = false;
        }

        #endregion

        #region Methods



        private void loadCommands()
        {
            this.LoadImageCommand = new RelayCommand(this.loadImage, canAlwaysExecute);
            this.SaveImageCommand = new RelayCommand(this.saveImage, this.canSaveImage);
            this.CreateMosaicCommand = new RelayCommand(this.createMosaic, this.canCreateMosaic);
            this.LoadPaletteCommand = new RelayCommand(this.loadPalette, canAlwaysExecute);
            this.ToggleBlackAndWhiteCommand = new RelayCommand(this.changeDisplayedMosaic, this.canChangeDisplayedMosaic);
        }


        private async void loadImage(object obj)
        {
            var readImage = new ImageReader();
            try
            {
                var results = await readImage.OpenImage();
                this.CurrentlyDisplayedImage = results;
                this.currentDpiX = readImage.DpiX;
                this.currentDpiY = readImage.DpiY;
                this.originalImageFileType = readImage.FileType;
            }
            catch (NullReferenceException)
            {
                this.CurrentlyDisplayedImage = null;
            }

            this.BlackAndWhiteMosaic = null;
            this.normalMosaic = null;
            this.CurrentlyDisplayedMosaic = null;
        }

        private static bool canAlwaysExecute(object obj)
        {
            return true;
        }

        private void saveImage(object obj)
        {
            var imageWriter = new ImageWriter();
            imageWriter.SaveImage(this.currentlyDisplayedMosaic, this.currentDpiX, this.currentDpiY, this.originalImageFileType);
        }

        private bool canSaveImage(object obj)
        {
            return this.CurrentlyDisplayedMosaic != null;
        }

        private async void createMosaic(object obj)
        {
            this.SolidMosaic = new SolidMosaic(this.CurrentlyDisplayedImage, this.GridFactory);
            var mosaic = await this.SolidMosaic.SetCellData();
            this.normalMosaic = mosaic;
            this.blackAndWhiteMosaic = null;
            this.CurrentlyDisplayedMosaic = mosaic;
            //Parallel.Invoke(() => this.changeToBlackAndWhite(mosaic));
            this.changeToBlackAndWhite(mosaic);
        }

        private bool canCreateMosaic(object obj)
        {
            return this.CurrentlyDisplayedImage != null;
        }

        private async void loadPalette(object obj)
        {
            try
            {
                await this.PaletteReader.LoadPalette();
                this.Palette.PaletteImages = this.PaletteReader.EditablePalette;
                this.MosaicPalette = this.Palette.PaletteImages.ToObservableCollection();
                this.PaletteSize = this.MosaicPalette.Count;
            }
            catch (NullReferenceException)
            {
                //TODO does nothing.
            }
        }

        private void changeDisplayedMosaic(object obj)
        {
            if (!this.isBlackAndWhite && this.blackAndWhiteMosaic != null)
            {
                this.CurrentlyDisplayedMosaic = this.blackAndWhiteMosaic;
                this.IsBlackAndWhite = true;
            }
            else
            {
                if (this.normalMosaic != null)
                {
                    this.CurrentlyDisplayedMosaic = this.normalMosaic;
                    this.IsBlackAndWhite = false;
                }
            }
        }

        private async void changeToBlackAndWhite(WriteableBitmap mosaic)
        {
            this.BlackAndWhiteMosaic = BitmapUtilities.ConvertToBlackAndWhite(mosaic);
        }

        private bool canChangeDisplayedMosaic(object obj)
        {
            return this.blackAndWhiteMosaic != null;
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
using System;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ImageSandbox.Model;
using ImageSandbox.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ImageSandbox.View

{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the show grid.
        /// </summary>
        /// <value>
        ///     The show grid.
        /// </value>
        public RadioButton ShowGrid { get; set; }

        /// <summary>
        ///     Gets or sets the hide grid.
        /// </summary>
        /// <value>
        ///     The hide grid.
        /// </value>
        public RadioButton HideGrid { get; set; }

        /// <summary>
        ///     Gets or sets the length of the cell side.
        /// </summary>
        /// <value>
        ///     The length of the cell side.
        /// </value>
        public int CellSideLength { get; set; }

        private Grid OverlayGrid { get; set; }
        private bool IsGridChangedOrHidden { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainPage" /> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            this.OverlayGrid = new Grid();
            this.ShowGrid = this.showGrid;
            this.HideGrid = this.hideGrid;
            ApplicationView.PreferredLaunchViewSize = new Size(1080, 720);
            this.IsGridChangedOrHidden = true;
        }

        #endregion

        #region Methods

        //TODO Move this to view model
        private void checked_DisplayGrid(object sender, RoutedEventArgs e)
        {
            this.setGrid();
            this.showGridOverlay();
        }

        private void showGridOverlay()
        {
            var showGridChecked = this.ShowGrid.IsChecked ?? false;
            if (showGridChecked && this.IsGridChangedOrHidden)
            {
                this.originalImageOverlay.Children.Add(this.OverlayGrid);
                this.IsGridChangedOrHidden = false;
            }
        }

        private Grid setGrid()
        {
            if (this.canUpdateGrid())
            {
                var scale = this.originalImage.ActualHeight / ActiveImage.Image.PixelHeight;
                var cellSizeScaled = this.CellSideLength * scale;

                var gridFactory = new GridFactory {
                    CellSideLength = (int) cellSizeScaled,
                    GridHeight = (int) this.originalImage.ActualHeight,
                    GridWidth = (int) this.originalImage.ActualWidth
                };
                if (this.CellSideLength > 0 && gridFactory.GridHeight > 0 && gridFactory.GridWidth > 0)
                {
                    this.OverlayGrid = gridFactory.DrawGrid();
                }
            }

            return this.OverlayGrid;
        }

        private bool canUpdateGrid()
        {
            var isImageSourceSet = this.originalImage.Source != null;
            var cellSizeParsed = Convert.ToInt32(this.gridSizeInput.Value);
            var isResultNew = cellSizeParsed != this.CellSideLength;
            var isResultValid = cellSizeParsed >= 5 && cellSizeParsed <= 50;

            var isReadyForUpdate = isImageSourceSet && isResultNew && isResultValid;

            if (isReadyForUpdate)
            {
                this.CellSideLength = cellSizeParsed;
                this.IsGridChangedOrHidden = true;
            }

            return isReadyForUpdate;
        }

        private void checked_HideGrid(object sender, RoutedEventArgs e)
        {
            this.hideGridOverlay();
        }

        private void hideGridOverlay()
        {
            for (var i = 1; i < this.originalImageOverlay.Children.Count; i++)
            {
                this.originalImageOverlay.Children.RemoveAt(i);
            }

            this.IsGridChangedOrHidden = true;
        }

        private void lostFocus_UpdateGrid(object sender, RoutedEventArgs e)
        {
            this.setGrid();
            this.showGridOverlay();
        }

        private void imageOpened_RemoveAndRecalculateGridOverlay(object sender, RoutedEventArgs e)
        {
            this.IsGridChangedOrHidden = true;
            this.setGrid();
            this.showGridOverlay();
        }

        #endregion
    }
}
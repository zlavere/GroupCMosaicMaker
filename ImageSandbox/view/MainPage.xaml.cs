using Windows.Foundation;
using Windows.UI.Notifications;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using ImageSandbox.Model;
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
        public RadioButton ShowGrid { get; set; }
        public RadioButton HideGrid { get; set; }
        public int CellSideLength { get; set; }
        private Grid overlayGrid { get; set; }
        private bool isGridChangedOrHidden { get; set; }
        #endregion

        #region Constructors

        public MainPage()
        {
            this.InitializeComponent();
            this.overlayGrid = new Grid();
            this.ViewModel = new MainPageViewModel();
            DataContext = this.ViewModel;
            this.ShowGrid = this.showGrid;
            this.HideGrid = this.hideGrid;
            ApplicationView.PreferredLaunchViewSize = new Size(1080, 720);
            this.isGridChangedOrHidden = true;
        }

        #endregion
        //TODO Move this to view model
        private void checked_DisplayGrid(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.setGrid();
            this.showGridOverlay();
        }

        private void showGridOverlay()
        {
            var showGridChecked = this.ShowGrid.IsChecked ?? false;
            if (showGridChecked && this.isGridChangedOrHidden)
            {
                this.originalImageOverlay.Children.Add(this.overlayGrid);
                this.isGridChangedOrHidden = false;
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
                    this.overlayGrid = gridFactory.DrawGrid();
                }
            }

            return this.overlayGrid;
        }

        private bool canUpdateGrid()
        {
            var isImageSourceSet = this.originalImage.Source != null;
            var cellSizeParsed = int.TryParse(this.gridSizeInput.Text, out var result);
            var isResultNew = result != this.CellSideLength;
            var isResultValid = result > 0;

            var isReadyForUpdate = isImageSourceSet && cellSizeParsed && isResultNew && isResultValid;

            if (isReadyForUpdate)
            {
                this.CellSideLength = result;
                this.isGridChangedOrHidden = true;
            }

            return isReadyForUpdate;
        }

        private void checked_HideGrid(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.hideGridOverlay();
        }

        private void hideGridOverlay()
        {
            for (var i = 1; i < this.originalImageOverlay.Children.Count; i++)
            {
                this.originalImageOverlay.Children.RemoveAt(i);  
            }
            this.isGridChangedOrHidden = true;
        }

        private void lostFocus_UpdateGrid(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.setGrid();
            this.showGridOverlay();
        }

        private void imageOpened_RemoveAndRecalculateGridOverlay(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.isGridChangedOrHidden = true;
            this.setGrid();
            this.showGridOverlay();
        }
    }
}
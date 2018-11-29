using Windows.Foundation;
using Windows.UI.Notifications;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
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
        #endregion

        #region Constructors

        public MainPage()
        {
            this.InitializeComponent();
            this.ViewModel = new MainPageViewModel();
            DataContext = this.ViewModel;
            this.ShowGrid = this.showGrid;
            this.HideGrid = this.hideGrid;
            ApplicationView.PreferredLaunchViewSize = new Size(1080, 720);
        }

        #endregion
        //TODO Move this to view model
        private void checked_DisplayGrid(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var cellSize = int.TryParse(this.gridSizeInput.Text, out var result);

            if (this.originalImage.Source != null)
            {
                var gridFactory = new GridFactory
                {
                    CellSideLength = result,
                    GridHeight = (int)this.originalImage.ActualHeight,
                    GridWidth = (int)this.originalImage.ActualWidth
                };
                if (result > 0 && gridFactory.GridHeight > 0 && gridFactory.GridWidth > 0)
                {
                    this.originalImageOverlay.Children.Add(gridFactory.DrawGrid());
                }
            }
        }

        private void checked_HideGrid(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            for (var i = 1; i < this.originalImageOverlay.Children.Count; i++)
            {
                this.originalImageOverlay.Children.RemoveAt(i);
            }
        }
    }
}
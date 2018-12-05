using Windows.UI.Xaml.Media.Imaging;

namespace ImageSandbox.ViewModel
{
    public static class ActiveImage
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the image.
        /// </summary>
        /// <value>
        ///     The image.
        /// </value>
        public static WriteableBitmap Image { get; set; }

        #endregion
    }
}
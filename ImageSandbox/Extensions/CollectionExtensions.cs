using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Imaging;

namespace ImageSandbox.Extensions
{
    /// <summary>
    ///     Provides new functionality to collections.
    /// </summary>
    public static class CollectionExtensions
    {
        #region Methods

        /// <summary>
        ///     Converts to a observable collection.
        /// </summary>
        /// <param name="writeableBitmaps">The writeable bitmaps.</param>
        /// <returns>
        ///     The converted collection.
        /// </returns>
        public static ObservableCollection<WriteableBitmap> ToObservableCollection(
            this IEnumerable<WriteableBitmap> writeableBitmaps)
        {
            return new ObservableCollection<WriteableBitmap>(writeableBitmaps);
        }

        #endregion
    }
}
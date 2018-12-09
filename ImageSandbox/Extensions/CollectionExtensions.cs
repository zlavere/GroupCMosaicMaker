using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Imaging;

namespace ImageSandbox.Extensions
{
    public static class CollectionExtensions
    {
        public static ObservableCollection<WriteableBitmap> ToObservableCollection(this IEnumerable<WriteableBitmap> writeableBitmaps)
        {
            return new ObservableCollection<WriteableBitmap>(writeableBitmaps);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

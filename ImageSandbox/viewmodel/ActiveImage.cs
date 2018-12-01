using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Media.Imaging;

namespace ImageSandbox.ViewModel
{
    public static class ActiveImage
    {
        private static WriteableBitmap image;
        public static WriteableBitmap Image
        {
            get => image ?? throw new ArgumentNullException();
            set => image = value;
        }
    }
}

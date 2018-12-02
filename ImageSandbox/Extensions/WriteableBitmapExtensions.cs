using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace ImageSandbox.Extensions
{
    public static class WriteableBitmapExtensions
    {
        public static List<Color> GetPixelColors(this WriteableBitmap wb)
        {
            using (var stream = wb.PixelBuffer.AsStream())
            {
                var pixelsCount = (wb.PixelWidth * wb.PixelHeight);
                var pixelBuffer = new byte[stream.Length];
                stream.Read(pixelBuffer, 0, (int)stream.Length);
                var colors = new List<Color>();

                for (var i = 0; i < pixelsCount; i++)
                {
                    var offsetPosition = i * 4;

                    var pixelBlue = pixelBuffer[offsetPosition];
                    var pixelGreen = pixelBuffer[offsetPosition + 1];
                    var pixelRed = pixelBuffer[offsetPosition + 2];

                    colors.Add(Color.FromArgb(0, pixelRed, pixelGreen, pixelBlue));
                }

                return colors;
            }
        } 
    }
}

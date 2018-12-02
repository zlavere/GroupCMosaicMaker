using System;
using System.Collections.Generic;
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
        public static async Task<IDictionary<int, Color>> GetPixelColors(this WriteableBitmap wb)
        {
            using (var stream = wb.PixelBuffer.AsStream())
            {
                var pixelsCount = (wb.PixelWidth * wb.PixelHeight) * 4;
                var pixelBuffer = new byte[pixelsCount];
                await stream.ReadAsync(pixelBuffer, 0, pixelsCount);
                var colors = new Dictionary<int, Color>();

                for (var i = 0; i < pixelsCount; i++)
                {
                    var offsetPosition = i * 4;
                    var pixelRed = pixelBuffer[offsetPosition + 1];
                    var pixelGreen = pixelBuffer[offsetPosition + 2];
                    var pixelBlue = pixelBuffer[offsetPosition + 3];

                    colors.Add(i, Color.FromArgb(0, pixelRed, pixelGreen, pixelBlue));
                }

                return colors;
            }
        } 
    }
}

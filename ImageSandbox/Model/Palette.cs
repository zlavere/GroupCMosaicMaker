using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using ImageSandbox.Utility;

namespace ImageSandbox.Model
{
    public class Palette
    {
        #region Properties

        public IList<WriteableBitmap> PaletteImages { get; set; }

        /// <summary>
        ///     Gets or sets the image average color dictionary.
        /// </summary>
        /// <key>
        ///     The index of PaletteImages.
        /// </key>
        /// <value>
        ///     An dictionary of indexes and average color.
        /// </value>
        public IDictionary<int, Color> ImageAverageColorDictionary { get; set; }

        #endregion

        #region Methods

        private IDictionary<int, Color> getAverageColorsOfPaletteImages()
        {
            for (var i = 0; i < this.PaletteImages.Count; i++)
            {
                var currentBitmapAsBytes = this.PaletteImages[i].PixelBuffer.ToArray();
                var imageWidth = (uint) this.PaletteImages[i].PixelWidth;
                var imageHeight = (uint) this.PaletteImages[i].PixelHeight;

                var averagePixelColor =
                    BitmapUtilities.GetAveragePixelColor(currentBitmapAsBytes, imageWidth, imageHeight);
                this.ImageAverageColorDictionary.Add(i, averagePixelColor);
            }

            return this.ImageAverageColorDictionary;
        }

        public WriteableBitmap findImageWithClosestColor(Color color)
        {
            var minimumDifference = this.ImageAverageColorDictionary.Values.Min(value =>
                Math.Abs(value.B - color.B + (value.B - color.B) + (value.R - color.R)));
            var bestIndexColorPair = this.ImageAverageColorDictionary.Where(value =>
                minimumDifference ==
                Math.Abs(value.Value.B - color.B + (value.Value.B - color.B) +
                         (value.Value.R - color.R)));
            var index = bestIndexColorPair.First().Key;

            return this.PaletteImages[index];
        }

        #endregion
    }
}
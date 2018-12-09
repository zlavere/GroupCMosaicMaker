using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using ImageSandbox.Extensions;

namespace ImageSandbox.Model
{
    public class Palette
    {
        private IList<WriteableBitmap> paletteImages;
        #region Properties

        public IList<WriteableBitmap> PaletteImages
        {
            get => this.paletteImages;
            set
            {
                this.paletteImages = value;
                this.ImageAverageColorDictionary = this.getAverageColorsOfPaletteImages().Result;
            }
        }

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

        public Palette()
        {
            this.PaletteImages = new List<WriteableBitmap>();
            this.ImageAverageColorDictionary = new Dictionary<int, Color>();
        }

        #region Methods

        private async Task<IDictionary<int, Color>> getAverageColorsOfPaletteImages()
        {
            for (var i = 0; i < this.PaletteImages.Count; i++) 
            {
                var colors = await this.PaletteImages[i].GetPixelColors();
                var averageRed = Convert.ToInt32(colors.Average(value => value.R));
                var averageGreen = Convert.ToInt32(colors.Average(value => value.G));
                var averageBlue = Convert.ToInt32(colors.Average(value => value.B)); 
                this.ImageAverageColorDictionary.Add(i, Color.FromArgb(255, (byte)averageRed, (byte)averageGreen, (byte)averageBlue));
            }

            return this.ImageAverageColorDictionary;
        }

        public WriteableBitmap FindImageWithClosestColor(Color color)
        {
            var minimumDifference = this.ImageAverageColorDictionary.Values.Min(value =>
                Math.Abs(value.B - color.B) + Math.Abs(value.B - color.B) + Math.Abs(value.R - color.R));

            var bestIndexColorPair = this.ImageAverageColorDictionary.Where(value =>
                minimumDifference ==
                Math.Abs(value.Value.B - color.B) + Math.Abs(value.Value.B - color.B) + Math.Abs(value.Value.R - color.R));

            var index = bestIndexColorPair.First().Key;

            return this.PaletteImages[index];
        }

        #endregion
    }
}
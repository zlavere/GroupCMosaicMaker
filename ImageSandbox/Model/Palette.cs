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
        #region Data members

        private IList<WriteableBitmap> paletteImages;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the palette images.
        /// </summary>
        /// <value>
        ///     The palette images.
        /// </value>
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

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Palette" /> class.
        /// </summary>
        public Palette()
        {
            this.PaletteImages = new List<WriteableBitmap>();
            this.ImageAverageColorDictionary = new Dictionary<int, Color>();
        }

        #endregion

        #region Methods

        private async Task<IDictionary<int, Color>> getAverageColorsOfPaletteImages()
        {
            for (var i = 0; i < this.PaletteImages.Count; i++)
            {
                var colors = await this.PaletteImages[i].GetPixelColors();
                var averageRed = Convert.ToInt32(colors.Average(value => value.R));
                var averageGreen = Convert.ToInt32(colors.Average(value => value.G));
                var averageBlue = Convert.ToInt32(colors.Average(value => value.B));
                this.ImageAverageColorDictionary.Add(i,
                    Color.FromArgb(255, (byte) averageRed, (byte) averageGreen, (byte) averageBlue));
            }

            return this.ImageAverageColorDictionary;
        }

        /// <summary>
        ///     Finds the color of the image with closest.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>
        ///     The image of the closest color.
        /// </returns>
        public WriteableBitmap FindImageWithClosestColor(Color color)
        {
            var colorSimilarity = this.ImageAverageColorDictionary.Values.OrderBy(value =>
                Math.Abs(value.B - color.B) + Math.Abs(value.B - color.B) +
                Math.Abs(value.R - color.R)).Take(5);

            var top5ClosestColors = colorSimilarity.ToList();

            var bestIndexColorPair =
                this.ImageAverageColorDictionary.Where(value => top5ClosestColors.Contains(value.Value));

            var rand = new Random();

            var randomKey = rand.Next(0, 5);

            var index = bestIndexColorPair.ToList()[randomKey].Key;

            return this.PaletteImages[index];
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using ImageSandbox.Extensions;

namespace ImageSandbox.Model
{
    public abstract class Mosaic
    {
        public WriteableBitmap sourceImage;
        #region Properties


        /// <summary>
        ///     Gets or sets the source image.
        /// </summary>
        /// <value>
        ///     The source image.
        /// </value>
        public WriteableBitmap SourceImage
        {
            get => this.sourceImage;
            set
            {
                this.sourceImage = value;
                this.getColors();
            }
        }

        /// <summary>
        ///     Gets or sets the grid factory.
        /// </summary>
        /// <value>
        ///     The grid factory.
        /// </value>
        public GridFactory GridFactory { get; set; }

        
        /// <summary>
        /// Gets or sets the colors.
        /// </summary>
        /// <value>
        /// The colors.
        /// </value>
        public List<Color> Colors { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ImageSandbox.Model.Mosaic" /> class.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        /// <param name="gridFactory">The grid factory.</param>
        protected Mosaic(WriteableBitmap sourceImage,
            GridFactory gridFactory)
        {
            this.SourceImage = sourceImage;
            this.GridFactory = gridFactory;
            
        }

        protected async Task<WriteableBitmap> writePixelDataToBitmap()
        {
            var sourcePixels = this.SourceImage.PixelWidth * this.SourceImage.PixelHeight;
            var mosaic = new WriteableBitmap(this.SourceImage.PixelWidth, this.SourceImage.PixelHeight);

            using (var stream = mosaic.PixelBuffer.AsStream())
            {
                var buffer = this.SetUpPixelData();

                await stream.WriteAsync(buffer, 0, sourcePixels * 4);
                await stream.FlushAsync();
            }

            return mosaic;
        }
        protected abstract byte[] SetUpPixelData();

        public abstract Task<WriteableBitmap> SetCellData();

        private async void getColors()
        {
            this.Colors = await this.SourceImage.GetPixelColors();
        }

        #endregion
    }
}
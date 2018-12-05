using Windows.UI.Xaml.Media.Imaging;

namespace ImageSandbox.Model
{
    public abstract class Mosaic
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the length of the cell side.
        /// </summary>
        /// <value>
        ///     The length of the cell side.
        /// </value>
        public int CellSideLength { get; set; }

        /// <summary>
        ///     Gets or sets the source image.
        /// </summary>
        /// <value>
        ///     The source image.
        /// </value>
        public WriteableBitmap SourceImage { get; set; }

        /// <summary>
        ///     Gets or sets the mosaic image.
        /// </summary>
        /// <value>
        ///     The mosaic image.
        /// </value>
        public WriteableBitmap MosaicImage { get; set; }

        /// <summary>
        ///     Gets or sets the grid factory.
        /// </summary>
        /// <value>
        ///     The grid factory.
        /// </value>
        public GridFactory GridFactory { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Mosaic" /> class.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        /// <param name="mosaicImage">The mosaic image.</param>
        /// <param name="cellSideLength">Length of the cell side.</param>
        /// <param name="gridFactory">The grid factory.</param>
        protected Mosaic(WriteableBitmap sourceImage, WriteableBitmap mosaicImage, int cellSideLength,
            GridFactory gridFactory)
        {
            this.SourceImage = sourceImage;
            this.MosaicImage = mosaicImage;
            this.CellSideLength = cellSideLength;
            this.GridFactory = gridFactory;
        }

        #endregion
    }
}
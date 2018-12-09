using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace ImageSandbox.Model
{
    public class SolidMosaic : Mosaic
    {
        #region Constructors
        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:ImageSandbox.Model.SolidMosaic" /> class.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        /// <param name="gridFactory">The grid factory.</param>
        public SolidMosaic(WriteableBitmap sourceImage,
            GridFactory gridFactory)
            : base(sourceImage, gridFactory)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Returns a WriteableBitmap with solid cell colors.
        /// </summary>
        /// <returns>WriteableBitmap with solid cell colors.</returns>
        public override async Task<WriteableBitmap> SetCellData()
        {
            GridFactory.CalculateCellAttributes();
            return await WritePixelDataToBitmap();
        }

        protected override byte[] SetUpPixelData()
        {
            var buffer = new byte[SourceImage.PixelWidth * SourceImage.PixelHeight * 4];

                Parallel.ForEach(GridFactory.Cells, cell =>
                {
                    Parallel.ForEach(cell.PixelOffsetsInByteArray, offset =>
                    {
                        buffer[offset] = cell.AverageColor.B;
                        buffer[offset + 1] = cell.AverageColor.G;
                        buffer[offset + 2] = cell.AverageColor.R;
                        buffer[offset + 3] = 255;
                    }); 
                });

            return buffer;
        }

        #endregion
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using ImageSandbox.Extensions;
using WriteableBitmapExtensions = Windows.UI.Xaml.Media.Imaging.WriteableBitmapExtensions;

namespace ImageSandbox.Model
{
    /// <summary>
    ///     A type of mosaic that is made with multiple individual images.
    /// </summary>
    /// <seealso cref="ImageSandbox.Model.Mosaic" />
    public class PictureMosaic : Mosaic
    {
        #region Properties

        private Palette Palette { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PictureMosaic" /> class.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        /// <param name="gridFactory">The grid factory.</param>
        /// <param name="palette">The palette.</param>
        public PictureMosaic(WriteableBitmap sourceImage, GridFactory gridFactory, Palette palette) : base(sourceImage,
            gridFactory)
        {
            this.Palette = palette;
        }

        #endregion

        #region Methods

        protected override byte[] SetUpPixelData()
        {
            var buffer = new byte[SourceImage.PixelWidth * SourceImage.PixelHeight * 4];
            foreach (var cell in GridFactory.Cells)
            {
                foreach (var current in this.setCellToPicture(cell).Result)
                {
                    buffer[current.Key] = current.Value;
                }
            }

            return buffer;
        }

        private async Task<Dictionary<int, byte>> setCellToPicture(Cell cell)
        {
            var picture = this.Palette.FindImageWithClosestColor(cell.AverageColor);
            picture = picture.Resize(GridFactory.CellSideLength, GridFactory.CellSideLength,
                WriteableBitmapExtensions.Interpolation.Bilinear);
            var picPixelData = await picture.GetPixelColors();
            var pixelIndex = 0;
            var offsetByteDictionary = new Dictionary<int, byte>();
            foreach (var pixel in cell.PixelOffsetsInByteArray)
            {
                offsetByteDictionary.Add(pixel, picPixelData[pixelIndex].B);
                offsetByteDictionary.Add(pixel + 1, picPixelData[pixelIndex].G);
                offsetByteDictionary.Add(pixel + 2, picPixelData[pixelIndex].R);
                offsetByteDictionary.Add(pixel + 3, 255);
                pixelIndex++;
            }

            return offsetByteDictionary;
        }

        /// <summary>
        ///     Sets the cell data.
        /// </summary>
        /// <returns></returns>
        public override async Task<WriteableBitmap> SetCellData()
        {
            GridFactory.CalculateCellAttributes();
            return await WritePixelDataToBitmap();
        }

        #endregion
    }
}
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace ImageSandbox.Model
{
    public abstract class Mosaic
    {
        #region Properties

        public int CellSideLength { get; set; }
        public WriteableBitmap SourceImage { get; set; }
        protected WriteableBitmap MosaicImage { get; set; }
        protected GridFactory GridFactory { get; set; }
        #endregion

        #region Constructors

        protected Mosaic(WriteableBitmap sourceImage, WriteableBitmap mosaicImage, int cellSideLength, GridFactory gridFactory)
        {
            this.SourceImage = sourceImage;
            this.MosaicImage = mosaicImage;
            this.CellSideLength = cellSideLength;
            this.GridFactory = gridFactory;
        }
 

        #endregion

    }
}
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

        public uint CellSide { get; set; }

        #endregion

        #region Constructors

        protected Mosaic(uint cellSideLength)
        {
            this.CellSide = cellSideLength;
        }

        #endregion

        #region Methods

        public async Task<BitmapImage> GetCellOfImage(BitmapDecoder decoder, uint xPoint, uint yPoint)
        {
            var ras = new InMemoryRandomAccessStream();
            var encoder = await BitmapEncoder.CreateForTranscodingAsync(ras, decoder);
            var bounds = new BitmapBounds {
                Height = this.CellSide,
                Width = this.CellSide,
                X = xPoint,
                Y = yPoint
            };
            encoder.BitmapTransform.Bounds = bounds;
            try
            {
                await encoder.FlushAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            var image = new BitmapImage();
            image.SetSource(ras);

            return image; 
        }

        #endregion
    }
}
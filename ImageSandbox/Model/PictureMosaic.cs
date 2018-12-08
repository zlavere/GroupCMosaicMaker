using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using ImageSandbox.Utility;
using Windows.UI;
using System.Linq;
using System.Threading.Tasks;

namespace ImageSandbox.Model
{
    public class PictureMosaic:Mosaic
    {
        private Palette Palette { get; set; }

        public PictureMosaic(WriteableBitmap sourceImage, GridFactory gridFactory, Palette palette) : base(sourceImage, gridFactory)
        {
            this.Palette = new Palette();
        }
        //        public WriteableBitmap CreatePictureMosaic(WriteableBitmap originalImage, List<WriteableBitmap> palette, int blockSize)
        //        {
        //            //TODO
        //        }
        //
        //        private void calculateAverageColorOfPaletteImages()
        //        {
        //            //TODO
        //        }


        public static WriteableBitmap ResizeImage(WriteableBitmap sourceImage, int blockSize)
        {
            var origHeight = sourceImage.PixelHeight;
            var origWidth = sourceImage.PixelWidth;
            var ratioX = blockSize/(float) origWidth;
            var ratioY = blockSize/(float) origHeight;
            var ratio = Math.Min(ratioX, ratioY);
            var newHeight = (int) (origHeight * ratio);
            var newWidth = (int) (origWidth * ratio);

            var newBitmap = new WriteableBitmap(newWidth, newHeight);

            var source = sourceImage.PixelBuffer.AsStream().AsRandomAccessStream();
            newBitmap.SetSource(source);

            return newBitmap;
        }

        protected override byte[] SetUpPixelData()
        {
            throw new NotImplementedException();
        }

        public override async Task<WriteableBitmap> SetCellData()
        {
            return new WriteableBitmap(10,10);
        }

        private void findClosestImageToCellColor(List<Color> averageColors, List<Color> colorsOfBlocks)
        {

            var closest = this.Palette.findImageWithClosestColor(Color.FromArgb(0,0,0,0)); //TODO Add color from cell
        }
//
//        private WriteableBitmap writePictureMosaicToBitmap(List<byte[]> selectedImages, int width, int height)
//        {
//            //TODO
//        }




    }
}
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
using ImageSandbox.Extensions;

namespace ImageSandbox.Model
{
    public class PictureMosaic:Mosaic
    {
        private Palette Palette { get; set; }

        public PictureMosaic(WriteableBitmap sourceImage, GridFactory gridFactory, Palette palette) : base(sourceImage, gridFactory)
        {
            this.Palette = palette;
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
            var buffer = new byte[SourceImage.PixelWidth * SourceImage.PixelHeight * 4];
            foreach (var cell in this.GridFactory.Cells)
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

        public override async Task<WriteableBitmap> SetCellData()
        {
            this.GridFactory.CalculateCellAttributes();
            return await this.writePixelDataToBitmap();
        }
  
    }
}
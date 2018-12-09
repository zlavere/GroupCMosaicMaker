using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using ImageSandbox.Extensions;
using WriteableBitmapExtensions = Windows.UI.Xaml.Media.Imaging.WriteableBitmapExtensions;

namespace ImageSandbox.Model
{
    public class PictureMosaic:Mosaic
    {
        private Palette Palette { get; set; }

        public PictureMosaic(WriteableBitmap sourceImage, GridFactory gridFactory, Palette palette) : base(sourceImage, gridFactory)
        {
            this.Palette = palette;
        }

//        public WriteableBitmap ResizeImage(WriteableBitmap sourceImage)
//        {
//            var origHeight = this.SourceImage.PixelHeight;
//            var origWidth = this.SourceImage.PixelWidth;
//            var ratioX = this.GridFactory.CellSideLength/(float) this.GridFactory.CellSideLength;
//            var ratioY = this.GridFactory.CellSideLength/(float) origHeight;
//            var ratio = Math.Min(ratioX, ratioY);
//            var newHeight = (int) (origHeight * ratio);
//            var newWidth = (int) (origWidth * ratio);
//
//            var newBitmap = new WriteableBitmap(newWidth, newHeight);
//
//            var source = sourceImage.PixelBuffer.AsStream().AsRandomAccessStream();
//            newBitmap.SetSource(source);
//            return newBitmap;
//        }

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

        public override async Task<WriteableBitmap> SetCellData()
        {
            GridFactory.CalculateCellAttributes();
            return await WritePixelDataToBitmap();
        }
    }
}
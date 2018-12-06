﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using ImageSandbox.Utility;
using Windows.UI;

namespace ImageSandbox.Model
{
    public class PictureMosaic:Mosaic
    {


        public List<Cell> Cells { get; set; }

        //TODO In SolidMosaic class we need to move the method up to the point at which it sets the values of the cells to the Mosaic part class at which point we will not need to pass cells as a param here
        //TODO In Mosaic we only need to pass GridFactory - GridFactory holds the most recent WriteableBitmaps
        public PictureMosaic(WriteableBitmap sourceImage, WriteableBitmap mosaicImage, int cellSideLength, GridFactory gridFactory, List<Cell> cells) : base(sourceImage, mosaicImage, cellSideLength, gridFactory)
        {
            //TODO
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

            WriteableBitmap newBitmap = new WriteableBitmap(newWidth, newHeight);

            IRandomAccessStream source = sourceImage.PixelBuffer.AsStream().AsRandomAccessStream();
            newBitmap.SetSource(source);

            return newBitmap;
        }

        private List<Color> GetAverageColorsOfPaletteImages(List<WriteableBitmap> palette)
        {
            List<Color> averageColors = new List<Color>();
            foreach (WriteableBitmap currentBitmap in palette)
            {
                var currentBitmapAsBytes = currentBitmap.PixelBuffer.ToArray();
                var imageWidth = (uint)currentBitmap.PixelWidth;
                var imageHeight = (uint)currentBitmap.PixelHeight;

                Color averagePixelColor =
                    BitmapUtilities.GetAveragePixelColor(currentBitmapAsBytes, imageWidth, imageHeight);
                averageColors.Add(averagePixelColor);
            }

            return averageColors;
        }

//        private void SelectImageForEachBlock(List<Color> averageColors, List<Color> colorsOfBlocks)
//        {
//            //TODO
//        }
//
//        private WriteableBitmap writePictureMosaicToBitmap(List<byte[]> selectedImages, int width, int height)
//        {
//            //TODO
//        }




    }
}
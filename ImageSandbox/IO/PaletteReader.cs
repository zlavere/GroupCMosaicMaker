using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using ImageSandbox.Utility;

namespace ImageSandbox.IO
{
    /// <summary>
    /// 
    /// </summary>
    public class PaletteReader
    {
        //TODO Reads the images in a folder as the mosaic palette.

        public async  Task<List<WriteableBitmap>> LoadPalette()
        {
            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.FileTypeFilter.Add(".jpg");
            var folder = await folderPicker.PickSingleFolderAsync();
            var filesList = await folder.GetFilesAsync();
            List<WriteableBitmap> imagePalette = new List<WriteableBitmap>();
            foreach (var currentFile in filesList)
            {
                var newBitmap = await BitmapUtilities.DecodeStorageFileToBitmap(currentFile);
                imagePalette.Add(newBitmap);
            }

            return imagePalette;
        }
    }
}

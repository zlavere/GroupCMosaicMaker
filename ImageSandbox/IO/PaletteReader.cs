using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using ImageSandbox.Utility;

namespace ImageSandbox.IO
{
    /// <summary>
    /// 
    /// </summary>
    public class PaletteReader
    {
        /// <summary>
        /// Gets or sets the editable palette.
        /// </summary>
        /// <value>
        /// The editable palette.
        /// </value>
        public List<WriteableBitmap> EditablePalette { get; set; }

        /// <summary>
        /// Gets or sets the displayable palette.
        /// </summary>
        /// <value>
        /// The displayable palette.
        /// </value>
        public List<Image> DisplayablePalette { get; set; }

        public async  Task LoadPalette()
        {
            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.FileTypeFilter.Add(".jpg");
            var folder = await folderPicker.PickSingleFolderAsync();
            var filesList = await folder.GetFilesAsync();
            List<WriteableBitmap> bitmapPalette = new List<WriteableBitmap>();
            List<Image> imagePalette = new List<Image>();
            foreach (var currentFile in filesList)
            {
                var newBitmap = await BitmapUtilities.DecodeStorageFileToBitmap(currentFile);
                Image newImage = new Image {Source = newBitmap};
                imagePalette.Add(newImage);
                bitmapPalette.Add(newBitmap);
            }

            this.EditablePalette = bitmapPalette;
            this.DisplayablePalette = imagePalette;

        }
    }
}

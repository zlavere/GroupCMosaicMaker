using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using ImageSandbox.Utility;

namespace ImageSandbox.IO
{
    /// <summary>
    ///     Reads in a folder of images and saves it as a displayable palette and a editable palette.
    /// </summary>
    public class PaletteReader
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the editable palette.
        /// </summary>
        /// <value>
        ///     The editable palette.
        /// </value>
        public List<WriteableBitmap> EditablePalette { get; set; }

        /// <summary>
        ///     Gets or sets the displayable palette.
        /// </summary>
        /// <value>
        ///     The displayable palette.
        /// </value>
        public List<Image> DisplayablePalette { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Loads the palette.
        /// </summary>
        public async Task LoadPalette()
        {
            var folderPicker = new FolderPicker();
            folderPicker.FileTypeFilter.Add(".jpg");
            var folder = await folderPicker.PickSingleFolderAsync();
            var filesList = await folder.GetFilesAsync();
            var bitmapPalette = new List<WriteableBitmap>();
            var imagePalette = new List<Image>();
            foreach (var currentFile in filesList)
            {
                try
                {
                    var newBitmap = await BitmapUtilities.DecodeStorageFileToBitmap(currentFile);
                    var newImage = new Image {Source = newBitmap};
                    imagePalette.Add(newImage);
                    bitmapPalette.Add(newBitmap);
                }
                catch (Exception)
                {
                    continue;
                }
                
            }

            this.EditablePalette = bitmapPalette;
            this.DisplayablePalette = imagePalette;
        }

        #endregion
    }
}
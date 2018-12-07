using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using ImageSandbox.Utility;

namespace ImageSandbox.Model
{
    public class Palette:IList<WriteableBitmap>, IDictionary<int, Color>
    {
        public IList<WriteableBitmap> PaletteImages { get; set; }

        /// <summary>
        /// Gets or sets the image average color dictionary.
        /// </summary>
        /// <key>
        /// The index of PaletteImages.
        /// </key>
        /// <values>
        /// The Average Color of the image.
        /// </values>
        /// <value>
        /// An dictionary of indexes and average color.
        /// </value>
        public IDictionary<int, Color> ImageAverageColorDictionary { get; set; }

        private IDictionary<int, Color> getAverageColorsOfPaletteImages()
        {

            for (var i = 0; i < this.PaletteImages.Count; i++)
            {
                var currentBitmapAsBytes = this.PaletteImages[i].PixelBuffer.ToArray();
                var imageWidth = (uint)this.PaletteImages[i].PixelWidth;
                var imageHeight = (uint)this.PaletteImages[i].PixelHeight;

                var averagePixelColor =
                    BitmapUtilities.GetAveragePixelColor(currentBitmapAsBytes, imageWidth, imageHeight);
                this.ImageAverageColorDictionary.Add(i, averagePixelColor);
            }

            return this.ImageAverageColorDictionary;
        }

        IEnumerator<KeyValuePair<int, Color>> IEnumerable<KeyValuePair<int, Color>>.GetEnumerator()
        {
            return this.ImageAverageColorDictionary.GetEnumerator();
        }

        public IEnumerator<WriteableBitmap> GetEnumerator()
        {
            return this.PaletteImages.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) this.PaletteImages).GetEnumerator();
        }

        public void Add(WriteableBitmap item)
        {
            this.PaletteImages.Add(item);
        }

        public void Add(KeyValuePair<int, Color> item)
        {
            this.ImageAverageColorDictionary.Add(item);
        }

        public void Clear()
        {
            this.PaletteImages.Clear();
        }

        public bool Contains(KeyValuePair<int, Color> item)
        {
            return this.ImageAverageColorDictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<int, Color>[] array, int arrayIndex)
        {
            this.ImageAverageColorDictionary.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<int, Color> item)
        {
            return this.ImageAverageColorDictionary.Remove(item);
        }

        public bool Contains(WriteableBitmap item)
        {
            return this.PaletteImages.Contains(item);
        }

        public void CopyTo(WriteableBitmap[] array, int arrayIndex)
        {
            this.PaletteImages.CopyTo(array, arrayIndex);
        }

        public bool Remove(WriteableBitmap item)
        {
            return this.PaletteImages.Remove(item);
        }

        public int Count => this.PaletteImages.Count;

        public bool IsReadOnly => this.PaletteImages.IsReadOnly;

        public int IndexOf(WriteableBitmap item)
        {
            return this.PaletteImages.IndexOf(item);
        }

        public void Insert(int index, WriteableBitmap item)
        {
            this.PaletteImages.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            this.PaletteImages.RemoveAt(index);
        }

        public void Add(int key, Color value)
        {
            this.ImageAverageColorDictionary.Add(key, value);
        }

        public bool ContainsKey(int key)
        {
            return this.ImageAverageColorDictionary.ContainsKey(key);
        }

        public bool Remove(int key)
        {
            return this.ImageAverageColorDictionary.Remove(key);
        }

        public bool TryGetValue(int key, out Color value)
        {
            return this.ImageAverageColorDictionary.TryGetValue(key, out value);
        }

        Color IDictionary<int, Color>.this[int key]
        {
            get => this.ImageAverageColorDictionary[key];
            set => this.ImageAverageColorDictionary[key] = value;
        }

        public ICollection<int> Keys => this.ImageAverageColorDictionary.Keys;

        public ICollection<Color> Values => this.ImageAverageColorDictionary.Values;

        public WriteableBitmap this[int index]
        {
            get => this.PaletteImages[index];
            set => this.PaletteImages[index] = value;
        }
    }
}

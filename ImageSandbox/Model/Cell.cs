using System.Collections.Generic;
using System.Linq;
using Windows.UI;

namespace ImageSandbox.Model
{
    /// <summary>
    ///     A cell is a subsection of an image used for
    /// </summary>
    public class Cell
    {
        #region Data members

        private Color averageColor;
        private List<byte> redValues;
        private List<byte> greenValues;
        private List<byte> blueValues;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the x dimension of a cell.
        /// </summary>
        /// <value>
        ///     The x value.
        /// </value>
        public int X { get; set; }

        /// <summary>
        ///     Gets or sets the y dimension of a cell.
        /// </summary>
        /// <value>
        ///     The y.
        /// </value>
        public int Y { get; set; }

        /// <summary>
        ///     A collection of colors contained in every pixel of the cell.
        /// </summary>
        /// <value>
        ///     The colors.
        /// </value>
        public List<Color> Colors { get; set; }

        /// <summary>
        ///     Gets or sets the pixel indexes.
        /// </summary>
        /// <value>
        ///     The pixel indexes.
        /// </value>
        public List<int> PixelIndexes { get; set; }

        /// <summary>
        ///     Gets or sets the pixel offsets in byte array.
        /// </summary>
        /// <value>
        ///     The pixel offsets in byte array.
        /// </value>
        public List<int> PixelOffsetsInByteArray { get; set; }

        /// <summary>
        ///     Gets or sets the red values.
        /// </summary>
        /// <value>
        ///     The red values.
        /// </value>
        public List<byte> RedValues
        {
            get
            {
                this.redValues = this.Colors.Select(color => color.R).ToList();
                return this.redValues;
            }
            set => this.redValues = value;
        }

        /// <summary>
        ///     Gets or sets the green values.
        /// </summary>
        /// <value>
        ///     The green values.
        /// </value>
        public List<byte> GreenValues
        {
            get
            {
                this.greenValues = this.Colors.Select(color => color.G).ToList();
                return this.greenValues;
            }
            set => this.greenValues = value;
        }

        /// <summary>
        ///     Gets or sets the blue values.
        /// </summary>
        /// <value>
        ///     The blue values.
        /// </value>
        public List<byte> BlueValues
        {
            get
            {
                this.blueValues = this.Colors.Select(color => color.B).ToList();
                return this.blueValues;
            }
            set => this.blueValues = value;
        }

        /// <summary>
        ///     Gets the average color.
        /// </summary>
        /// <value>
        ///     The average color.
        /// </value>
        public Color AverageColor
        {
            get
            {
                var averageRed = (int)this.RedValues.Average(value => value);
                var averageGreen = (int)this.GreenValues.Average(value => value);
                var averageBlue = (int)this.BlueValues.Average(value => value);
                this.averageColor = Color.FromArgb(1, (byte)averageRed, (byte)averageGreen, (byte)averageBlue);
                return this.averageColor;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Cell" /> class.
        /// </summary>
        public Cell()
        {
            this.Colors = new List<Color>();
            this.PixelIndexes = new List<int>();
            this.RedValues = new List<byte>();
            this.GreenValues = new List<byte>();
            this.BlueValues = new List<byte>();
            this.PixelOffsetsInByteArray = new List<int>();
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ImageSandbox.Converter
{
    public class TextInputToIntConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var stringValue = value.ToString();
            int.TryParse(stringValue, out var result);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value.ToString();
        }
    }
}

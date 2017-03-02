using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ImageMeta.Converters
{
    public class ColorToHexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;
            var color = (Color) value;
            return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return default(Color);
            var colorHex = (string) value;
            var rString = colorHex.Substring(1, 2);
            var gString = colorHex.Substring(3, 4);
            var bString = colorHex.Substring(5, 6);
            var r = System.Convert.ToByte(rString, 16);
            var g = System.Convert.ToByte(gString, 16);
            var b = System.Convert.ToByte(bString, 16);
            return Color.FromRgb(r, g, b);
        }
    }
}
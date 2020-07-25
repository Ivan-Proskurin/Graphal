using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

using DrawingColor = System.Drawing.Color;

namespace Graphal.VisualDebug.Converters
{
    public class DrawingColorToMediaBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is DrawingColor color))
            {
                return null;
            }

            return new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
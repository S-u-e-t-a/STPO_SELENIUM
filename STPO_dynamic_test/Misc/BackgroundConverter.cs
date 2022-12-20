using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;


namespace STPO_dynamic_test.Misc
{
    public class BackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return new SolidColorBrush((Color) ColorConverter.ConvertFromString("#fafafa")!);
            }

            if ((bool) value)
            {
                {
                    return new SolidColorBrush((Color) ColorConverter.ConvertFromString("#99ff99")!);
                }
            }

            return new SolidColorBrush((Color) ColorConverter.ConvertFromString("#ff5050")!);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

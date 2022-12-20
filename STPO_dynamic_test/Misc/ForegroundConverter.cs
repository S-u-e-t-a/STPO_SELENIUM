using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;


namespace STPO_dynamic_test.Misc
{
    public class ForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                //return new SolidColorBrush(Colors.Black);

                return new SolidColorBrush((Color) ColorConverter.ConvertFromString("#333")!);
            }

            return new SolidColorBrush((Color) ColorConverter.ConvertFromString("#333")!);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

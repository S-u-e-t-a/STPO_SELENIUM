using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;


namespace STPO_dynamic_test;

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
                return new SolidColorBrush((Color) ColorConverter.ConvertFromString("#6cc644")!);
            }
        }

        return new SolidColorBrush((Color) ColorConverter.ConvertFromString("#c9510c")!);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


public class ForegroundConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null)
        {
            //return new SolidColorBrush(Colors.Black);

            return new SolidColorBrush((Color) ColorConverter.ConvertFromString("#333")!);
        }

        return new SolidColorBrush((Color) ColorConverter.ConvertFromString("#f5f5f5")!);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

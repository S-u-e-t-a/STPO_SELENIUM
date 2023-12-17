using System.Globalization;

namespace STPO_dynamic_test.Misc
{
    public static class DoubleStringService
    {
        private static readonly string NumberSeparator = ".";
        
        public static bool StringToDouble(string str, out double value, string separator = null)
        {
            var culture = new CultureInfo(CultureInfo.CurrentCulture.Name);
            separator ??= NumberSeparator;
            culture.NumberFormat.NumberDecimalSeparator = separator;
            return double.TryParse(str,NumberStyles.Any,culture,out value);
        }

        public static string DoubleToString(double value, string separator = null)
        {
            separator ??= NumberSeparator;
            var nfi = new NumberFormatInfo
            {
                NumberDecimalSeparator = separator
            };
            return value.ToString(nfi);
        }
    }
}
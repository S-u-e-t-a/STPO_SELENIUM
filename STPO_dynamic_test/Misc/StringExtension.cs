using System.Globalization;


namespace STPO_dynamic_test.Misc
{
    public static class StringExtension
    {
        public static double ToDouble(this string str, string separator = null)
        {
            DoubleStringService.StringToDouble(str, out var value, separator);
            return value;
        }
    }
}

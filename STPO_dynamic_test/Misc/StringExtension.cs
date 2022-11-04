using System.Globalization;


namespace STPO_dynamic_test.Misc;

public static class StringExtension
{
    public static double ToDouble(this string str)
    {
        return double.Parse(str, CultureInfo.GetCultureInfo("de-DE").NumberFormat);
    }
}

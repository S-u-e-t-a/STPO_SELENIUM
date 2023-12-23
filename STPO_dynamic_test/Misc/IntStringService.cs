using System;
using System.Globalization;

namespace STPO_dynamic_test.Misc
{
    public static class IntStringService
    {
        
        public static bool StringToInt(string str, out int? value, Scale scale = Scale.Dec)
        {
            var culture = new CultureInfo(CultureInfo.CurrentCulture.Name);
            value = default;
            try
            {
                value = Convert.ToInt32(str, (int)scale);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string IntToString(int value, Scale scale = Scale.Dec)
        {
            return Convert.ToString(value, (int)scale);
        }
    }

    public enum Scale
    {
        Bin = 2,
        Oct = 8,
        Dec = 10,
        Hex= 16
    }
}
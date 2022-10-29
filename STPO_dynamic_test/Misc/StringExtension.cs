using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STPO_dynamic_test
{
    public static class StringExtension
    {
        public static double ToDouble(this string str)
        {
            return double.Parse(str, CultureInfo.GetCultureInfo("de-DE").NumberFormat);
        }
    }
}

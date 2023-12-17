using System.Globalization;
using System.Windows;
using STPO_dynamic_test.Misc;

namespace STPO_dynamic_test.Model
{
    public class Result
    {
        private readonly double doubleValue;

        public Result(string value)
        {
            Value = value;
            doubleValue = double.NaN;
            var s = value;

            if (value.StartsWith("S = "))
            {
                s = s.Remove(0, 4);
            }

            IsNumber = DoubleStringService.StringToDouble(s, out doubleValue);
        }

        public string Value { get; set; }

        public bool IsNumber { get; set; }

        public double ToDouble()
        {
            return doubleValue;
        }

        public static double operator -(Result lResult, Result rResult)
        {
            return lResult.ToDouble() - rResult.ToDouble();
        }

        public static bool operator ==(Result lResult, Result rResult)
        {
            if (lResult is null && rResult is null)
            {
                return true;
            }

            if (lResult is null || rResult is null)
            {
                return false;
            }

            if (!lResult.IsNumber && !rResult.IsNumber)
            {
                return lResult.Value == rResult.Value;
            }

            if (lResult.IsNumber && rResult.IsNumber)
            {
                return lResult.doubleValue == rResult.doubleValue;
            }

            return false;
        }

        public static bool operator !=(Result lResult, Result rResult)
        {
            return !(lResult == rResult);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}

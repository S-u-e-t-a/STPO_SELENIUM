using System;


namespace STPO_dynamic_test
{
    public class Result
    {
        public Result(string value)
        {
            Value = value;
            doubleValue = Double.NaN;
            var s = value;

            if (value.StartsWith("S = "))
            {
                s=s.Remove(0, 4);
            }
            IsNumber = double.TryParse(s, out doubleValue);
        }

        private double doubleValue;
        public string Value { get; init; }

        public bool IsNumber { get; init; }

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
            if (!lResult.IsNumber && !rResult.IsNumber)
            {
                return lResult.Value==rResult.Value;
            }
            else
            {
                throw new Exception("Значения не являются строками!");
            }
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

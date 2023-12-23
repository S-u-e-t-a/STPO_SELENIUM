using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using STPO_dynamic_test.Misc;

namespace STPO_dynamic_test.Model
{
    public class AggregatedResult
    {
        public AggregatedResult(string value)
        {
            var isInt = IntStringService.StringToInt(value, out var intVar);

            var bin = value;
            var oct = value;
            var hex = value;
            
            if (isInt)
            {
                bin = IntStringService.IntToString(intVar.Value, Scale.Bin);
                oct = IntStringService.IntToString(intVar.Value, Scale.Oct);
                hex = IntStringService.IntToString(intVar.Value, Scale.Hex);
            }
            BinResult = new Result(bin, Scale.Bin);
            OctResult = new Result(oct, Scale.Oct);
            HexResult = new Result(hex, Scale.Hex);
        }   
        public AggregatedResult(string binValue,string octValue,string hexValue)
        {
            BinResult = new Result(binValue, Scale.Bin);
            OctResult = new Result(octValue, Scale.Oct);
            HexResult = new Result(hexValue, Scale.Hex);
        }
        public Result OctResult { get; set; }
        public Result BinResult { get; set; }
        public Result HexResult { get; set; }

        public static bool operator ==(AggregatedResult lResult, AggregatedResult rResult)
        {
            if (lResult is null && rResult is null)
            {
                return true;
            }
            if (!(lResult is null) && rResult is null)
            {
                return false;
            }
            if (lResult is null && !(rResult is null))
            {
                return false;
            }
            var lResults = new List<Result>() { lResult.BinResult, lResult.OctResult, lResult.HexResult };
            var rResults = new List<Result>() { rResult.BinResult, rResult.OctResult, rResult.HexResult };
            
            for (int i = 0; i < 3; i++)
            {
                if (lResults[i] != rResults[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static bool operator !=(AggregatedResult lResult, AggregatedResult rResult)
        {
            return !(lResult == rResult);
        }
    }
    public class Result
    {
        private readonly int? intValue;

        public Result(string value, Scale scale = Scale.Dec)
        {
            Value = value;
            intValue = null;
            var s = value;
            

            IsNumber = IntStringService.StringToInt(s, out intValue, scale);
        }

        public string Value { get; set; }

        public bool IsNumber { get; set; }
        

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
                return lResult.intValue == rResult.intValue;
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

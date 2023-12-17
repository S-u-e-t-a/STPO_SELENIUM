using System.Collections.Generic;
using System.Linq;
using STPO_dynamic_test.Misc;


namespace STPO_dynamic_test.ParametersVM
{
    public class VariableParameterArray : VariableParameter
    {
        public int Count { get; set; }

        public new string GetValue()
        {
            if (!IsVariable)
            {
                return Value;
            }

            var coefs = new List<double>();
            for (int i = 0; i < Count; i++)
            {
                coefs.Add(RandomDouble.GetRandomDouble(Min, Max));
            }

            var coefsStr = string.Join(" ", coefs.Select(x => DoubleStringService.DoubleToString(x, ",")));
            
            return coefsStr;
        }
    }
}

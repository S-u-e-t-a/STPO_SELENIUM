using PropertyChanged;

using STPO_dynamic_test.Misc;


namespace STPO_dynamic_test.ParametersVM
{
    [AddINotifyPropertyChangedInterface]
    public class VariableParameter
    {
        public bool IsVariable { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public string Value { get; set; }

        // todo rename
        public string GetValue()
        {
            if (!IsVariable)
            {
                return Value;
            }

            return DoubleStringService.DoubleToString(RandomDouble.GetRandomDouble(Min, Max),",");
        }
    }
}

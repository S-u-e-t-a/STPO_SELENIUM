using PropertyChanged;

using STPO_dynamic_test.Misc;


namespace STPO_dynamic_test.ParametersVM
{
    [AddINotifyPropertyChangedInterface]
    public class VariableParameter
    {
        public bool IsVariable { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public string Value { get; set; }

        // todo rename
        public string GetValue()
        {
            if (!IsVariable)
            {
                return Value;
            }

            return IntStringService.IntToString(RandomInt.GetRandomInt(Min, Max));
        }
    }
}

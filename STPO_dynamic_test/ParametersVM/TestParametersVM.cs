using System.Collections.ObjectModel;

using PropertyChanged;


namespace STPO_dynamic_test.ParametersVM
{
    [AddINotifyPropertyChangedInterface]
    public class TestParametersVM
    {
        public VariableParameter InputNumber { get; set; }
    }
}

using System.Collections.ObjectModel;

using PropertyChanged;


namespace STPO_dynamic_test.ParametersVM;

[AddINotifyPropertyChangedInterface]
public class TestParametersVM
{
    public ObservableCollection<IntegrationMethod> SelectedMethods { get; set; }

    public VariableParameter Right { get; set; }
    public VariableParameter Left { get; set; }
    public VariableParameter Step { get; set; }
    public VariableParameterArray Coefs { get; set; }
}

using PropertyChanged;

using STPO_dynamic_test.Model;


namespace STPO_dynamic_test.ParametersVM
{
    [AddINotifyPropertyChangedInterface]
    public class TestInDataGrid
    {
        public TestInDataGrid(Test test)
        {
            Test = test;
        }

        public bool IsNeedToRun { get; set; } = true;
        public bool? IsPassed { get; set; }
        public Test Test { get; set; }
    }
}

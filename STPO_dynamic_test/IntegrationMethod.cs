using PropertyChanged;


namespace STPO_dynamic_test
{
    [AddINotifyPropertyChangedInterface]
    public class IntegrationMethod
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}

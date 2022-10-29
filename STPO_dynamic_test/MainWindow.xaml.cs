using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Controls;


namespace STPO_dynamic_test
{
    public partial class MainWindow
    {
        private ObservableCollection<IntegrationMethod> Methods;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new VM();
            Methods = new ObservableCollection<IntegrationMethod>(){((VM) DataContext).Methods[0]};
            ((VM) DataContext).SelectedMethods = Methods;
            MetodBox.SelectedItems.Add(((VM) DataContext).Methods[0]);
        }

        private void CheckComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
            {
                Methods.Add((IntegrationMethod)item);
            }

            foreach (var item in e.RemovedItems)
            {
                Methods.Remove((IntegrationMethod)item);
            }
        }
    }
}

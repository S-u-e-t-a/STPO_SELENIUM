using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace STPO_dynamic_test
{
    public partial class MainWindow
    {
        private readonly ObservableCollection<IntegrationMethod> Methods;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new VM();

            Methods = new ObservableCollection<IntegrationMethod>();

            ((VM)DataContext).Parameters.SelectedMethods = Methods;

            //((VM) DataContext).OnPropertyChanged("SelectedMethods");

            //MetodBox.SelectedItems.Add(((VM) DataContext).Methods[0]);
        }

        private void CheckComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems) Methods.Add((IntegrationMethod)item);

            foreach (var item in e.RemovedItems) Methods.Remove((IntegrationMethod)item);
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (sender as DataGrid).SelectedCells.Clear();
            (sender as DataGrid).SelectedItems.Clear();
        }

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
        }
    }
}
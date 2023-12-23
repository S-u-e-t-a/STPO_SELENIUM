using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace STPO_dynamic_test
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new VM();
        }
        
    }
}
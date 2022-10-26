using System.Collections.Generic;
using System.Diagnostics;

using HandyControl.Controls;
using HandyControl.Themes;
using HandyControl.Tools;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace STPO_dynamic_test
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            var initialTestData = new InitialTestData()
            {
                Min = "-6",
                Max = "7",
                Step = "0,0001",
                IntegrateMethod = "3",
                Coefs = new List<string>(){"7","2","3","1","8",},
            };

            var test = new Test(initialTestData, "TEST 1", 0.001);
            Debug.WriteLine(test);
            DataContext = new VM();
        }
    }
}

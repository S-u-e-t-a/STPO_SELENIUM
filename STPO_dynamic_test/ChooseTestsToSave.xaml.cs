using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Windows;

using HandyControl.Tools.Extension;

using Microsoft.Win32;

using STPO_dynamic_test.Misc;
using STPO_dynamic_test.ParametersVM;


namespace STPO_dynamic_test
{
    public partial class ChooseTestsToSave
    {
        private readonly ObservableCollection<TestInDataGrid> _tests;
        private readonly TestParametersVM _parametersVm;

        public ChooseTestsToSave(ObservableCollection<TestInDataGrid> tests, TestParametersVM parametersVm)
        {
            _tests = tests;
            _parametersVm = parametersVm;
            InitializeComponent();
        }

        //МНЕ ОЧЕНЬ ЛЕНЬ ПИСАТЬ ЭТО В VM!!
        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var tests = _tests.Clone().ToList();

            if ((bool) OnlyPositive.IsChecked)
            {
                tests = tests.Where(t => t.IsPassed == true).ToList();
            }

            else if ((bool) OnlyNegative.IsChecked)
            {
                tests = tests.Where(t => t.IsPassed == false).ToList();
            }
            else
            {
                tests = tests.OrderBy(t => t.IsPassed).ToList();
            }

            var saveFileDialog = new SaveFileDialog();

            if (saveFileDialog.ShowDialog() == true)
            {
                PdfExporter.Export(saveFileDialog.FileName, tests, _parametersVm);
            }

            Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.Win32;

using STPO_dynamic_test.Misc;
using STPO_dynamic_test.Model;
using STPO_dynamic_test.ParametersVM;
using MessageBox = HandyControl.Controls.MessageBox;


namespace STPO_dynamic_test
{
    internal class VM : ViewModelBase

    {
    #region Functions
    
    private List<Test> BuildTests(int count)
        {
            var tests = new List<Test>();
            for (int i = 0; i < count; i++)
            {
                var inputNumber = Parameters.InputNumber.Value;
                if (i == 0)
                {
                    if (Parameters.InputNumber.IsVariable)
                    {
                        inputNumber = IntStringService.IntToString(Parameters.InputNumber.Min);
                    }
                }
                else if (i == count - 1)
                {
                    if (Parameters.InputNumber.IsVariable)
                    {
                        inputNumber = IntStringService.IntToString(Parameters.InputNumber.Max);
                    }
                }
                else
                {
                    if (Parameters.InputNumber.IsVariable)
                    {
                        var step = (Parameters.InputNumber.Max - Parameters.InputNumber.Min) / (count-1);
                        inputNumber = IntStringService.IntToString(Parameters.InputNumber.Min + i * step);
                    }
                }

                var init = new InitialTestData()
                {
                    InputNumber = inputNumber
                };
                tests.Add(new Test(init, $"Test {i}"));
            }

            return tests;
        }

        #endregion


    #region Properties


        public TestParametersVM Parameters { get; set; } = new TestParametersVM()
        {
            InputNumber = new VariableParameter()
            {
                IsVariable = false,
                Min = 0,
                Max = 15,
                Value = "0",
            },
        };

        public ObservableCollection<TestInDataGrid> GeneratedTests { get; set; }

    #endregion


    #region Commands

        private RelayCommand _exportPdfCommand;

        public RelayCommand ExportPdfCommand
        {
            get
            {
                return _exportPdfCommand ??= new RelayCommand(o =>
                {
                    var chooseToSaveWin = new ChooseTestsToSave(GeneratedTests, Parameters);
                    chooseToSaveWin.Show();
                });
            }
        }


        private RelayCommand _startCommand;

        public RelayCommand StartCommand
        {
            get
            {
                return _startCommand ??= new RelayCommand(o =>
                {
                    foreach (var test in GeneratedTests)
                    {
                        if (test.IsNeedToRun)
                        {
                            test.Test.Run();
                            test.IsPassed = test.Test.IsTestPassed();
                        }
                    }
                    Test._driver?.Close();
                    Test._driver?.Quit();
                    Test._driver?.Dispose();
                    Test._driver = null;
                }, _ =>
                {
                    if (GeneratedTests is null)
                    {
                        return false;
                    }


                    return GeneratedTests.Count > 0;
                });
            }
        }

        private RelayCommand _makeTestsCommand;

        public RelayCommand MakeTestsCommand
        {
            get
            {
                return _makeTestsCommand ??= new RelayCommand(o =>
                    {
                        var number = this.Parameters.InputNumber;
                          var testsCount = number.IsVariable ? number.Max - number.Min +1  :  1;
                          GeneratedTests = new ObservableCollection<TestInDataGrid>();

                          if (!Parameters.InputNumber.IsVariable)
                          {
                              testsCount = 1;
                          }

                          GeneratedTests = new ObservableCollection<TestInDataGrid>(BuildTests(testsCount).Select(x => new TestInDataGrid(x)));
                      },
                      _ =>
                      {
                          return true;
                      });
            }
        }

        private RelayCommand _exportCommand;

        public RelayCommand ExportCommand
        {
            get
            {
                return _exportCommand ??= new RelayCommand(o =>
                {
                    var saveFileDialog = new SaveFileDialog();

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        using (var sw = new StreamWriter(saveFileDialog.FileName))
                        {
                            sw.WriteLine(JsonSerializer.Serialize(GeneratedTests.Select(t => t.Test).ToList()));
                        }
                    }
                }, _ =>
                {
                    if (GeneratedTests is null)
                    {
                        return false;
                    }

                    return GeneratedTests.Count > 0;
                });
            }
        }

        private RelayCommand _importCommand;

        public RelayCommand ImportCommand
        {
            get
            {
                return _importCommand ??= new RelayCommand(o =>
                {
                    var saveFileDialog = new OpenFileDialog();

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        using (var sr = new StreamReader(saveFileDialog.FileName))
                        {
                            var text = sr.ReadToEnd();
                            var tests = JsonSerializer.Deserialize<List<Test>>(text);
                            GeneratedTests = new ObservableCollection<TestInDataGrid>();

                            foreach (var test in tests)
                            {
                                GeneratedTests.Add(new TestInDataGrid(test));
                            }
                        }
                    }
                });
            }
        }

    #endregion
    }
}

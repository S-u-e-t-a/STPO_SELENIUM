using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
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

    #region Constructors

        public VM()
        {
            Methods = new List<IntegrationMethod>
            {
                new IntegrationMethod() {Id = "1", Name = "Метод парабол",},
                new IntegrationMethod() {Id = "2", Name = "Метод Трапеций",},
                new IntegrationMethod() {Id = "3", Name = "Метод Монте-Карло",},
            };
        }

    #endregion


        private IntegrationMethod getMethod()
        {
            var rnd = new Random();

            return Parameters.SelectedMethods[rnd.Next(0, Parameters.SelectedMethods.Count)];
        }


        private List<Test> BuildTests(int count)
        {
            var tests = new List<Test>();
            for (int i = 0; i < count; i++)
            {
                var coefs = Parameters.Coefs.GetValue().Split(" ").ToList();
                var method = getMethod().Id;
                var max = Parameters.Right.Value;
                var min = Parameters.Left.Value;
                var integrateStep = Parameters.Step.Value;
                if (i == 0)
                {
                    if (Parameters.Left.IsVariable)
                    {
                        min = DoubleStringService.DoubleToString(Parameters.Left.Min, ",");
                    }

                    if (Parameters.Right.IsVariable)
                    {
                        max = DoubleStringService.DoubleToString(Parameters.Right.Min, ",");
                    }

                    if (Parameters.Step.IsVariable)
                    {
                        integrateStep = DoubleStringService.DoubleToString(Parameters.Step.Min, ",");
                    }
                }
                else if (i == count - 1)
                {
                    if (Parameters.Left.IsVariable)
                    {
                        min = DoubleStringService.DoubleToString(Parameters.Left.Max, ",");
                    }

                    if (Parameters.Right.IsVariable)
                    {
                        max = DoubleStringService.DoubleToString(Parameters.Right.Max, ",");
                    }

                    if (Parameters.Step.IsVariable)
                    {
                        integrateStep = DoubleStringService.DoubleToString(Parameters.Step.Max, ",");
                    }
                }
                else
                {
                    if (Parameters.Left.IsVariable)
                    {
                        var step = (Parameters.Left.Max - Parameters.Left.Min) / (count-1);
                        min = DoubleStringService.DoubleToString(Parameters.Left.Min + i * step, ",");
                    }

                    if (Parameters.Right.IsVariable)
                    {
                        var step = (Parameters.Right.Max - Parameters.Right.Min) / (count-1);
                        max = DoubleStringService.DoubleToString(Parameters.Right.Min + i * step, ",");
                    }

                    if (Parameters.Step.IsVariable)
                    {
                        var step = (Parameters.Step.Max - Parameters.Step.Min) / (count-1);
                        integrateStep = DoubleStringService.DoubleToString(Parameters.Step.Min + i * step, ",");
                    }
                }

                var init = new InitialTestData()
                {
                    Coefs = coefs,
                    IntegrateMethod = method,
                    Max = max,
                    Min = min,
                    Step = integrateStep
                };
                tests.Add(new Test(init, $"Test {i}"));
            }

            return tests;
        }

        #endregion


    #region Properties

        public int CountOfCases { get; set; } = 10;
        public double Eps { get; set; } = 0.1;


        public TestParametersVM Parameters { get; set; } = new TestParametersVM()
        {
            Left = new VariableParameter()
            {
                IsVariable = false,
                Min = -10,
                Max = 0,
                Value = "0",
            },
            Right = new VariableParameter()
            {
                IsVariable = false,
                Min = 0,
                Max = 10,
                Value = "10",
            },
            Step = new VariableParameter()
            {
                IsVariable = false,
                Min = 0,
                Max = 1,
                Value = "0,1",
            },
            Coefs = new VariableParameterArray()
            {
                IsVariable = false,
                Min = 3,
                Max = 10,
                Value = "1 2 3",
                Count = 3,
            }
        };

        public ObservableCollection<TestInDataGrid> GeneratedTests { get; set; }
        public List<IntegrationMethod> Methods { get; set; }

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
                    if (Eps<0)
                    {
                        MessageBox.Error("Некорректная погрешность","Ошибка");

                        return;
                    }

                    foreach (var test in GeneratedTests)
                    {
                        if (test.IsNeedToRun)
                        {
                            test.Test.Run();
                            test.IsPassed = test.Test.IsTestPassed(Eps);
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

        private RelayCommand _makeTestsCommand;

        public RelayCommand MakeTestsCommand
        {
            get
            {
                return _makeTestsCommand ??= new RelayCommand(o =>
                      {
                          if (CountOfCases<1)
                          {
                              MessageBox.Error("Некорректное количество тест кейсов...","Ошибка");

                              return;
                          }

                          var testsCount = CountOfCases;
                          GeneratedTests = new ObservableCollection<TestInDataGrid>();

                          if (!Parameters.Coefs.IsVariable &&
                              !Parameters.Left.IsVariable &&
                              !Parameters.Right.IsVariable &&
                              Parameters.SelectedMethods.Count == 1 &&
                              !Parameters.Step.IsVariable)
                          {
                              testsCount = 1;
                          }

                          GeneratedTests = new ObservableCollection<TestInDataGrid>(BuildTests(testsCount).Select(x => new TestInDataGrid(x)));
                      },
                      _ =>
                      {
                          if (Parameters.SelectedMethods.Count < 1)
                          {
                              return false;
                          }

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

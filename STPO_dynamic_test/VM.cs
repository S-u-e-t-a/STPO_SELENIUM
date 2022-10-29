using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.Win32;

using PropertyChanged;


namespace STPO_dynamic_test
{
    [AddINotifyPropertyChangedInterface]
    internal class IntegrationMethod
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }


    [AddINotifyPropertyChangedInterface]
    internal class TestInDataGrid
    {
        public TestInDataGrid(Test test) => Test = test;
        public bool IsNeedToRun { get; set; } = true;
        public bool? IsPassed { get; set; }
        public Test Test { get; set; }
    }


    internal class VM : ViewModelBase

    {
    #region Functions

    #region Constructors

        public VM()
        {
            Methods = new List<IntegrationMethod>
            {
                new() {Id = "1", Name = "Метод парабол",},
                new() {Id = "2", Name = "Метод Трапеций",},
                new() {Id = "3", Name = "Метод Монте-Карло",},
            };
        }

    #endregion


        private double GetRandomDouble(double min, double max)
        {
            var rnd = new Random();

            return rnd.NextDouble() * (max - min) + min;
        }

        private string getLeft()
        {
            if (!IsLeftVariable)
            {
                return Left;
            }
            return GetRandomDouble(MinLeft, MaxLeft).ToString().Replace('.',',');
        }
        private string getRight()
        {
            if (!IsRightVariable)
            {
                return Right;
            }
            return GetRandomDouble(MinRight, MaxRight).ToString().Replace('.',',');
        }
        private string getCoefs()
        {
            if (!IsCoefsVariable)
            {
                return Coefs;
            }

            var coefs = "";

            for (int i = 0; i < CoefsCount; i++)
            {
                coefs += GetRandomDouble(MinCoef, MaxCoef).ToString().Replace('.',',');
                coefs += " ";
            }

            coefs = coefs.Substring(0, coefs.Length-1);
            return coefs;
        }

        private IntegrationMethod getMethod()
        {
            var rnd = new Random();
            return SelectedMethods[rnd.Next(0,SelectedMethods.Count)];
        }

        private string getStep()
        {
            if (!IsStepVariable)
            {
                return Step;
            }

            return GetRandomDouble(MinStep, MaxStep).ToString().Replace('.',',');
        }
        
        private Test buildTest(string name)
        {
            var coefs = getCoefs().Split(" ");
            var init = new InitialTestData()
            {
                Coefs = coefs.ToList(),
                IntegrateMethod = getMethod().Id,
                Max = getRight(),
                Min = getLeft(),
                Step = getStep(),
            };

            var test = new Test(init, name);

            return test;
        }
    #endregion


    #region Properties

        public string CountOfCases { get; set; } = "10";
        public string Eps { get; set; } = "0,1";


    #region left

        public string Left { get; set; } = "0";
        public bool IsLeftVariable { get; set; }
        public double MinLeft { get; set; } = -10;
        public double MaxLeft { get; set; } = 0;

    #endregion
    #region right


        public string Right { get; set; } = "10";
        public bool IsRightVariable { get; set; }
        public double MinRight { get; set; } = 0;
        public double MaxRight { get; set; } = 10;

    #endregion
    #region coefs

        public string Coefs { get; set; } = "1 2 3";
        public int CoefsCount { get; set; } = 3;
        public double MinCoef { get; set; } = 1;
        public double MaxCoef { get; set; } = 10;
        public bool IsCoefsVariable { get; set; }

    #endregion
    #region step

        public string Step { get; set; } = "0,1";
        public bool IsStepVariable { get; set; }
        public double MinStep { get; set; } = 0;
        public double MaxStep { get; set; } = 1;

    #endregion


        public ObservableCollection<TestInDataGrid> GeneratedTests { get; set; }
        public List<IntegrationMethod> Methods { get; set; }
        public ObservableCollection<IntegrationMethod> SelectedMethods { get; set; }

    #endregion


    #region Commands

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
                            test.IsPassed = test.Test.IsTestPassed(Eps.ToDouble());
                        }
                    }
                }, _ =>
                {
                    if (GeneratedTests is null)
                    {
                        return false;
                    }

                    return GeneratedTests.Count > 0;
                }

                );
            }
        }

        private RelayCommand _makeTestsCommand;

        public RelayCommand MakeTestsCommand
        {
            get
            {
                return _makeTestsCommand ??= new RelayCommand(o =>
                {
                    var testsCount = CountOfCases.ToDouble();
                    GeneratedTests = new ObservableCollection<TestInDataGrid>();

                    for (int i = 0; i < testsCount; i++)
                    {
                        GeneratedTests.Add(new TestInDataGrid(buildTest($"Test {i}")));
                    }
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
                    SaveFileDialog saveFileDialog = new SaveFileDialog();

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                        {
                            sw.WriteLine(JsonSerializer.Serialize(GeneratedTests.Select(t=>t.Test).ToList()));
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
                    OpenFileDialog saveFileDialog = new OpenFileDialog();

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        using (StreamReader sr = new StreamReader(saveFileDialog.FileName))
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

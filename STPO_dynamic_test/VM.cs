using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;

using Microsoft.Win32;

using STPO_dynamic_test.Misc;
using STPO_dynamic_test.Model;
using STPO_dynamic_test.ParametersVM;


namespace STPO_dynamic_test;

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


    private IntegrationMethod getMethod()
    {
        var rnd = new Random();

        return Parameters.SelectedMethods[rnd.Next(0, Parameters.SelectedMethods.Count)];
    }


    private Test buildTest(string name)
    {
        var coefs = Parameters.Coefs.GetValue().Split(" ");

        var init = new InitialTestData
        {
            Coefs = coefs.ToList(),
            IntegrateMethod = getMethod().Id,
            Max = Parameters.Right.GetValue(),
            Min = Parameters.Left.GetValue(),
            Step = Parameters.Step.GetValue(),
        };

        var test = new Test(init, name);

        return test;
    }

#endregion


#region Properties

    public string CountOfCases { get; set; } = "10";
    public string Eps { get; set; } = "0,1";


    public TestParametersVM Parameters { get; set; } = new TestParametersVM()
    {
        Left = new()
        {
            IsVariable = false,
            Min = -10,
            Max = 0,
            Value = "0",
        },
        Right = new()
        {
            IsVariable = false,
            Min = 0,
            Max = 10,
            Value = "10",
        },
        Step = new()
        {
            IsVariable = false,
            Min = 0,
            Max = 1,
            Value = "0,1",
        },
        Coefs = new()
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
                var saveFileDialog = new SaveFileDialog();

                if (saveFileDialog.ShowDialog() == true)
                {
                    PdfExporter.Export(saveFileDialog.FileName, GeneratedTests, Parameters);
                }
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
                var testsCount = CountOfCases.ToDouble();
                GeneratedTests = new ObservableCollection<TestInDataGrid>();

                for (var i = 0; i < testsCount; i++)
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

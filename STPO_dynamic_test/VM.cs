using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32;


namespace STPO_dynamic_test
{
    internal class IntegrationMethod
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
    internal class VM : ViewModelBase

    {
        #region Functions

        #region Constructors

            public VM()
            {
                Methods = new List<IntegrationMethod>()
                {
                    new () {Id = "1", Name = "Метод парабол"},
                    new () {Id = "2", Name = "Метод Трапеций"},
                    new () {Id = "3", Name = "Метод Монте-Карло"},
                };

                SelectedMethod = Methods[0];
                IsPositive = true;
            }


        #endregion


            private double getRandomDouble(double min, double max)
            {
                var rnd = new Random();
                return rnd.NextDouble() * (max - min) + min;
                
        }
            private double getRandomStep()
            {
                var rnd = new Random();
                if (IsPositive)
                {
                    return getRandomDouble(0.000001, 0.5);
                }
                else
                {
                    return getRandomDouble(-0.00001, 1);
                }
            }


        #endregion

        #region Properties

            private string _countOfCases;

            public string CountOfCases
            {
                get
                {
                    return _countOfCases;
                }
                set
                {
                    _countOfCases = value;
                    OnPropertyChanged();
                }
            }

            private bool _isPositive;

            public bool IsPositive
            {
                get
                {
                    return _isPositive;
                }
                set
                {
                    _isPositive = value;
                    OnPropertyChanged();
                }
            }
            private string _eps;

            public string Eps
            {
                get
                {
                    return _eps;
                }
                set
                {
                    _eps = value;
                    OnPropertyChanged();
                }
            }

            private string _left;

            public string Left
            {
                get
                {
                    return _left;
                }
                set
                {
                    _left = value;
                    OnPropertyChanged();
                }
            }

            private string _right;

            public string Right
            {
                get
                {
                    return _right;
                }
                set
                {
                    _right = value;
                    OnPropertyChanged();
                    Debug.WriteLine($"Изменилось!!! {_right}");
                }
            }

            private string _coefs;

            public string Coefs
            {
                get
                {
                    return _coefs;
                }
                set
                {
                    _coefs = value;
                    OnPropertyChanged();
                }
            }

            private List<IntegrationMethod> _methods;

            public List<IntegrationMethod> Methods
            {
                get
                {
                    return _methods;
                }
                set
                {
                    _methods = value;
                    OnPropertyChanged();
                }
            }

            private IntegrationMethod _selectedMethod;

            public IntegrationMethod SelectedMethod
            {
                get
                {
                    return _selectedMethod;
                }
                set
                {
                    _selectedMethod = value;
                    OnPropertyChanged();
                }
            }

            private string _result;

            public string Result
            {
                get
                {
                    return _result;
                }
                set
                {
                    _result = value;
                    OnPropertyChanged();
                }
            }
            
        #endregion

        #region Commands

            private RelayCommand _startCommand;

            public RelayCommand StartCommand
            {
                get
                {
                    return _startCommand ?? (_startCommand = new RelayCommand(o =>
                        {
                            Result = "";
                            var doubleCoefs = Coefs.Split(' ').ToList();

                            var count = (int)CountOfCases.ToDouble();

                            for (int i = 0; i < count; i++)
                            {
                                var initialTestData = new InitialTestData()
                                {
                                    Coefs = doubleCoefs,
                                    Max = Right,
                                    Min = Left,
                                    Step = getRandomStep().ToString(CultureInfo.CreateSpecificCulture("de-DE")),
                                    IntegrateMethod = SelectedMethod.Id
                                };

                                var test = new Test(initialTestData, $"Test {i}", Eps.ToDouble());
                                Result += test +"\n\n";
                            }
                           
                        }));
                }
            }

            private RelayCommand _saveResult;

            public RelayCommand SaveResult
            {
                get
                {
                    return _saveResult ?? (_saveResult = new RelayCommand(o =>
                     {
                         SaveFileDialog saveFileDialog = new SaveFileDialog();

                         if (saveFileDialog.ShowDialog() == true)
                         {
                             using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                             {
                                 sw.WriteLine(Result);
                             }
                         }
                         
                     }, _ => Result != "" && Result is not null));
                }
            }

        #endregion
    }
}

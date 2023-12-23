using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PropertyChanged;

using STPO_dynamic_test.Misc;
using MessageBox = HandyControl.Controls.MessageBox;


namespace STPO_dynamic_test.Model
{
    [AddINotifyPropertyChangedInterface]
    public class Test
    {
        public Test(InitialTestData initialTestData, string name)
        {
            InitialTestData = initialTestData;
            Name = name;

            if (!isNumeric(InitialTestData.InputNumber))
            {
                ResultExpected = new AggregatedResult("Вы не ввели число!");
            }

            if (ResultExpected is null)
            {
                ResultExpected = new AggregatedResult(InitialTestData.InputNumber);
            }
        }

        public InitialTestData InitialTestData { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public AggregatedResult ResultExpected { get; set; }

        [JsonIgnore]
        public AggregatedResult ResultFact { get; set; }

        private bool isNumeric(string str)
        {
            var symb = "0123456789.,-";
            var flag = false;

            for (var i = 0; i < str.Length; i++)
            {
                flag = false;

                for (var j = 0; j < symb.Length; j++)
                {
                    if (symb[j] == str[i])
                    {
                        flag = true;
                    }
                }

                if (flag == false)
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsTestPassed()
        {
            return ResultFact == ResultExpected;
        }


        public static ChromeDriver _driver;
        private (string,string,string) GetResultFromSelenium()
        {
            if (_driver is null)
            {
                _driver = new ChromeDriver();
            }
            _driver.Navigate().GoToUrl("https://worldlab.technolog.edu.ru/stud/group8/5/index.html?");
            IWebElement inputBox = _driver.FindElement(By.Id("dec_input"));
            inputBox.SendKeys(InitialTestData.ToString());

            var button = _driver.FindElement(By.Id("send_button"));
            button.Click();
            Thread.Sleep(200);
            var binVal = _driver.FindElement(By.Id("bin_div")).Text;
            var octVal = _driver.FindElement(By.Id("oct_div")).Text;
            var hexVal = _driver.FindElement(By.Id("hex_div")).Text;

            binVal = binVal.Replace("Двоичная система ", "");
            octVal = octVal.Replace("Восьмеричная система ", "");
            hexVal = hexVal.Replace("Шестнадцатеричная система ", "");
            return (binVal, octVal, hexVal);
        }

        public void Run()
        {
            var res = GetResultFromSelenium();
            
            ResultFact = new AggregatedResult(res.Item1, res.Item2, res.Item3);
        }

        // public override string ToString()
        // {
        //     var s = $"X = {InitialTestData}\n" +
        //             $"EPS = {EPS}\n" +
        //             $"YE: {Ye}\n" +
        //             $"YF: {Yf}\n";
        //
        //     if (Ye.IsNumber && Yf.IsNumber)
        //     {
        //         s += $"|SYE-SYF| = {Ye - Yf}";
        //
        //         if (Ye - Yf > EPS)
        //         {
        //             s += " > ";
        //         }
        //         else if (Ye - Yf < EPS)
        //         {
        //             s += " < ";
        //         }
        //         else //скорее всего никогда не сработает
        //         {
        //             s += " = ";
        //         }
        //
        //         s += $"{EPS}\n";
        //
        //         
        //     }
        //     if (isTestPassed)
        //     {
        //         s += "Тест пройден";
        //     }
        //     else
        //     {
        //         s += "Тест не пройден";
        //     }
        //     return s;
        // }
    }
}

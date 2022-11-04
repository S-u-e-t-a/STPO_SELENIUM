using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Windows.Input;

using PropertyChanged;

using STPO_dynamic_test.Misc;
using STPO_dynamic_test.Model.IntegrateMethods;


namespace STPO_dynamic_test.Model;

[AddINotifyPropertyChangedInterface]
public class Test
{
    private readonly Func<double, List<double>, double> func = (x, coefs) =>
    {
        double FS = 0;

        for (var i = 0; i < coefs.Count; i++)
        {
            FS += coefs[i] * Math.Pow(x, i);
        }

        return FS;
    };

    public Test(InitialTestData initialTestData, string name)
    {
        InitialTestData = initialTestData;
        Name = name;
        var numOfArgs = initialTestData.ToString().Split(' ').Length;

        if (numOfArgs < 5)
        {
            Ye = new Result("Число параметров не соответствует ожидаемому и должно быть, как минимум 5!");
        }

        if (!isNumeric(InitialTestData.Min))
        {
            Ye = new Result("Левая граница диапазона не является числом!");
        }

        if (!isNumeric(InitialTestData.Max))
        {
            Ye = new Result("Правая граница диапазона не является числом!");
        }

        if (isNumeric(initialTestData.Min) && isNumeric(initialTestData.Max))
        {
            if (initialTestData.Min.ToDouble() >= initialTestData.Max.ToDouble())
            {
                Ye = new Result("Левая граница диапазона должна быть < правой границы диапазона!");
            }
        }

        if (isNumeric(InitialTestData.Step))
        {
            if (InitialTestData.Step.ToDouble() < 0.000001 || InitialTestData.Step.ToDouble() > 0.5)
            {
                Ye = new Result("Шаг интегрирования должен быть в пределах [0.000001;0.5]");
            }
        }
        else
        {
            Ye = new Result("Шаг интегрирования должен быть в пределах [0.000001;0.5]");
        }

        if (isNumeric(InitialTestData.IntegrateMethod))
        {
            if (InitialTestData.IntegrateMethod.ToDouble() < 1 || InitialTestData.IntegrateMethod.ToDouble() > 3)
            {
                Ye = new Result("Четвертый параметр определяет метод интегрирования и должен быть в пределах[1; 3]");
            }
        }
        else
        {
            Ye = new Result("Четвертый параметр определяет метод интегрирования и должен быть в пределах[1; 3]");
        }

        IntegrateMethod = new CustomIntegral();

        if (Ye is null)
        {
            Ye = new Result($"S = {IntegrateMethod.Integrate(InitialTestData, func)}");
        }

        // var res = GetResultFromScript();
        //
        // if (res is null)
        // {
        //     res = "null";
        // }
        // Yf = new Result(res);
    }

    public InitialTestData InitialTestData { get; init; }
    public string Name { get; init; }

    [JsonIgnore]
    public Result Ye { get; init; }

    [JsonIgnore]
    public Result Yf { get; set; }

    [JsonIgnore]
    public IIntegral IntegrateMethod { get; init; }

    private bool isNumeric(string str)
    {
        var symb = "0123456789,-";
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

    public bool IsTestPassed(double EPS)
    {
        if (Yf.IsNumber && Ye.IsNumber)
        {
            if (Math.Abs(Yf - Ye) < EPS)
            {
                return true;
            }

            return false;
        }

        if (!Yf.IsNumber && !Ye.IsNumber)
        {
            return Yf == Ye;
        }

        return false;
    }


    private string GetResultFromScript()
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = @"Integral3x.exe",
                Arguments = InitialTestData.ToString(),
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
            };


            var proc = new Process();
            proc.StartInfo = startInfo;
            proc.Start();

            var buffer = string.Empty;
            char symb;
            symb = (char) proc.StandardOutput.Peek();
            buffer = proc.StandardOutput.ReadLine();
            proc.StandardInput.Write(Key.Enter);
            proc.WaitForExit();

            return buffer;
        }
        catch (Exception e)
        {
            return "Ошибка!";
        }
    }

    public void Run()
    {
        var res = GetResultFromScript();

        if (res is null)
        {
            res = "null";
        }

        Yf = new Result(res);
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;

using STPO_dynamic_test.IntegrateMethods;


namespace STPO_dynamic_test;


internal class Test
{
    public Test(InitialTestData initialTestData, string name, double ePS)
    {
        InitialTestData = initialTestData;
        Name = name;
        EPS = ePS;
        var numOfArgs = initialTestData.ToString().Split(' ').Length;
        if (numOfArgs<5)
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

        if (isNumeric(InitialTestData.Step))
        {
            if (InitialTestData.Step.ToDouble() < 0.000001 || InitialTestData.Step.ToDouble() > 0.5)
            {
                Ye = new Result("Шаг интегрирования должен быть в пределах[0.000001; 0.5]");
            }
        }
        else
        {
            Ye = new Result("Шаг интегрирования должен быть в пределах[0.000001; 0.5]");
        }

        if (isNumeric(InitialTestData.IntegrateMethod))
        {
            if (InitialTestData.Step.ToDouble() < 1 || InitialTestData.Step.ToDouble() > 3)
            {
                Ye = new Result("Четвертый параметр определяет метод интегрирования и должен быть в пределах[1; 3]");
            }
        }
        else
        {
            Ye = new Result("Четвертый параметр определяет метод интегрирования и должен быть в пределах[1; 3]");
        }

        //var m = int.Parse(InitialTestData.IntegrateMethod);
        //if (m==1)
        //{
        //    IntegrateMethod = new ParabolIntegral();
        //}
        //if (m==2)
        //{
        //    IntegrateMethod = new TrapeziaIntegral();
        //}
        //if (m==3)
        //{
        //    IntegrateMethod = new MonteCarloIntegral();
        //}
        IntegrateMethod = new CustomIntegral();
        if (Ye is not null)
        {
            Ye = new Result($"S = {IntegrateMethod.Integrate(InitialTestData, func)}");
        }

        var res = GetResultFromScript();

        if (res is null)
        {
            res = "null";
        }
        Yf = new Result(res);
    }

    private Func<double, List<double>, double> func = (x, coefs) =>
    {
        double FS = 0;
        for (int i = 0; i < coefs.Count; i++) FS += coefs[i] * Math.Pow(x, i);

        return FS;
    };
    public InitialTestData InitialTestData { get; init; }
    public string Name { get; init; }
    public double EPS { get; init; }
    public Result Ye { get; init; }
    public Result Yf { get; init; }
    public IIntegral IntegrateMethod { get; init; }

    private bool isNumeric(string str)
    {
        string symb = "0123456789,-";
        bool flag = false;

        for (int i = 0; i < str.Length; i++)
        {
            flag = false;

            for (int j = 0; j < symb.Length; j++)
            {
                if (symb[j] == str[i])
                {
                    flag=true;
                }
            }
            if (flag == false)
                return false;
        }

        return true;
    }
    private bool isTestPassed
    {
        get
        {
            if (Yf.IsNumber && Ye.IsNumber)
            {
                if (Math.Abs(Yf-Ye)<EPS)
                {
                    return true;
                }

                return false;
            }

            if (!Yf.IsNumber && !Ye.IsNumber)
            {
                return Yf==Ye;
            }

            return false;
        }
    }

    private string GetResultFromScript()
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = @"testsCPP.exe",
                Arguments = InitialTestData.ToString(),
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
                
            };


            var proc = new Process(); proc.StartInfo = startInfo; proc.Start();

            string buffer = string.Empty;
            char symb;
            symb = (char)proc.StandardOutput.Peek();
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


    public override string ToString()
    {
        var s = $"X = {InitialTestData}\n" +
                $"EPS = {EPS}\n" +
                $"YE: {Ye}\n" +
                $"YF: {Yf}\n";

        if (Ye.IsNumber && Yf.IsNumber)
        {
            s += $"|SYE-SYF| = {Ye - Yf}";

            if (Ye - Yf > EPS)
            {
                s += " > ";
            }
            else if (Ye - Yf < EPS)
            {
                s += " < ";
            }
            else //скорее всего никогда не сработает
            {
                s += " = ";
            }

            s += $"{EPS}\n";

            
        }
        if (isTestPassed)
        {
            s += "Тест пройден";
        }
        else
        {
            s += "Тест не пройден";
        }
        return s;
    }
}


using System;
using System.Collections.Generic;
using System.Linq;

using STPO_dynamic_test.Misc;


namespace STPO_dynamic_test.Model.IntegrateMethods;

public class ParabolIntegral : IIntegral
{
    public double Integrate(InitialTestData test, Func<double, List<double>, double> func)
    {
        var min = test.Min.ToDouble();
        var max = test.Max.ToDouble();
        var step = test.Step.ToDouble();
        var coefs = test.Coefs.Select(c => c.ToDouble()).ToList();
        var x = min + step;
        var res = 0.0;

        while (x < max)
        {
            res += 4 * func(x, coefs);
            x += step;

            if (x >= max)
            {
                break;
            }

            res += 2 * func(x, coefs);
            x += step;
        }

        res = step / 3 * (res + func(min, coefs) + func(max, coefs));

        return res;
    }
}

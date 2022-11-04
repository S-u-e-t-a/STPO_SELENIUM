using System;
using System.Collections.Generic;
using System.Linq;

using STPO_dynamic_test.Misc;


namespace STPO_dynamic_test.Model.IntegrateMethods;

internal class MonteCarloIntegral : IIntegral
{
    public double Integrate(InitialTestData test, Func<double, List<double>, double> func)
    {
        var min = test.Min.ToDouble();
        var max = test.Max.ToDouble();
        var step = test.Step.ToDouble();
        var coefs = test.Coefs.Select(c => c.ToDouble()).ToList();

        var S = 0.0;
        var N = (int) Math.Floor((max - min) / step);
        var rnd = new Random();

        for (var i = 0; i < N; i++)
        {
            S += func(min + rnd.NextDouble() * (max - min), coefs);
        }

        S = S / N * (max - min);

        return S;
    }
}

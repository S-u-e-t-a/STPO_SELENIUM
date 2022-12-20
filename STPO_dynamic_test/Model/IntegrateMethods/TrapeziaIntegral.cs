using System;
using System.Collections.Generic;
using System.Linq;

using STPO_dynamic_test.Misc;


namespace STPO_dynamic_test.Model.IntegrateMethods
{
    internal class TrapeziaIntegral : IIntegral
    {
        public double Integrate(InitialTestData test, Func<double, List<double>, double> func)
        {
            var min = test.Min.ToDouble();
            var max = test.Max.ToDouble();
            var step = test.Step.ToDouble();
            var coefs = test.Coefs.Select(c => c.ToDouble()).ToList();

            var N = (int) Math.Floor((max - min) / step);
            var S = func(max, coefs) - func(min, coefs) / 2;
            var x = min + step;

            for (var i = 1; i < N; i++)
            {
                S += func(x, coefs);
                x += step;
            }

            S *= step;

            return S;
        }
    }
}

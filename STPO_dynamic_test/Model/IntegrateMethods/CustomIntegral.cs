using System;
using System.Collections.Generic;
using System.Linq;

using STPO_dynamic_test.Misc;


namespace STPO_dynamic_test.Model.IntegrateMethods
{
    internal class CustomIntegral : IIntegral
    {
        public double Integrate(InitialTestData test, Func<double, List<double>, double> func)
        {
            var min = test.Min.ToDouble();
            var max = test.Max.ToDouble();
            var step = test.Step.ToDouble();
            var coefs = test.Coefs.Select(c => c.ToDouble()).ToList();

            double result = 0;

            for (var i = 0; i < coefs.Count(); i++)
            {
                result += coefs[i] * (Math.Pow(max, i + 1) / (i + 1) - Math.Pow(min, i + 1) / (i + 1));
            }

            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STPO_dynamic_test.IntegrateMethods
{
    internal class MonteCarloIntegral:IIntegral
    {
        public double Integrate(InitialTestData test, Func<double, List<double>, double> func)
        {
            double min = test.Min.ToDouble();
            double max = test.Max.ToDouble();
            double step = test.Step.ToDouble();
            List<double> coefs = test.Coefs.Select(c => c.ToDouble()).ToList();

            var S = 0.0;
            int N = (int) Math.Floor((max-min)/step);
            var rnd = new Random();
            for (int i = 0; i < N; i++)
            {
                S += func(min + rnd.NextDouble() * (max - min), coefs);
            }
            S = S/N*(max-min);

            return S;
        }
    }
}

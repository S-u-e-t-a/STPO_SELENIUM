using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STPO_dynamic_test
{
    internal class TrapeziaIntegral:IIntegral
    {
        public double Integrate(InitialTestData test, Func<double, List<double>, double> func)
        {
            double min = test.Min.ToDouble();
            double max = test.Max.ToDouble();
            double step = test.Step.ToDouble();
            List<double> coefs = test.Coefs.Select(c => c.ToDouble()).ToList();

            int N = (int) Math.Floor((max - min) / step);
            var S = (func(max, coefs) - func(min, coefs) / 2);
            var x = min+step;

            for (int i = 1; i < N; i++)
            {
                S += func(x, coefs);
                x += step;
            }

            S *= step;

            return S;
        }
    }
}

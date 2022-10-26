using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STPO_dynamic_test.IntegrateMethods
{
    internal class CustomIntegral:IIntegral
    {
        public double Integrate(InitialTestData test, Func<double, List<double>, double> func)
        {

            double min = test.Min.ToDouble();
            double max = test.Max.ToDouble();
            double step = test.Step.ToDouble();
            List<double> coefs = test.Coefs.Select(c => c.ToDouble()).ToList();

            double result = 0;
            for (int i = 0; i < coefs.Count(); i++)
            {
                result += coefs[i] * ((Math.Pow(max, i + 1) / (i + 1)) - (Math.Pow(min, i + 1) / (i + 1)));

            }

            return result;

        }
    }
}

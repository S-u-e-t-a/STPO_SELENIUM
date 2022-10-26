using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace STPO_dynamic_test
{
    public class ParabolIntegral: IIntegral
    {
        public double Integrate(InitialTestData test, Func<double, List<double>, double> func)
        {
            double min = test.Min.ToDouble();
            double max = test.Max.ToDouble();
            double step = test.Step.ToDouble();
            List<double> coefs = test.Coefs.Select(c => c.ToDouble()).ToList();
            var x = min+step;
            var res = 0.0;
            while (x<max)
            {
                res += 4 * func(x, coefs);
                x += step;

                if (x >= max)
                {
                    break;
                }
                res += 2 * func(x, coefs);
                x +=step;
            }

            res = (step / 3) * (res + func(min, coefs) + func(max, coefs));

            return res;
        }
    }
}

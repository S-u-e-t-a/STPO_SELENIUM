using System.Collections.Generic;
using System;

namespace STPO_dynamic_test
{
    public interface IIntegral
    {
        public double Integrate(InitialTestData test, Func<double, List<double>, double> func);
    }
}

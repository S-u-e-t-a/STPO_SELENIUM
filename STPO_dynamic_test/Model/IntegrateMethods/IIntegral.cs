using System;
using System.Collections.Generic;


namespace STPO_dynamic_test.Model.IntegrateMethods
{
    public interface IIntegral
    {
        public double Integrate(InitialTestData test, Func<double, List<double>, double> func);
    }
}

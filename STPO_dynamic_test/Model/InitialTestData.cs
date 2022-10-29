using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STPO_dynamic_test
{
    public class InitialTestData
    {
        public string Min { get; init; }
        public string Max { get; init; }
        public string Step { get; init; }
        public List<string> Coefs { get; init; }

        public string CoefsString
        {
            get
            {
                return String.Join(' ', Coefs);
            }
        }

        public string IntegrateMethod { get; init; }
        public override string ToString()
        {
            var strCoefs = String.Join(" ", Coefs);

            return $"{Min} {Max} {Step} {IntegrateMethod} {strCoefs}";
        }
    }
}

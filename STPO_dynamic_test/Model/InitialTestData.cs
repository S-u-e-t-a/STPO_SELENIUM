using System.Collections.Generic;


namespace STPO_dynamic_test.Model
{
    public class InitialTestData
    {
        public string Min { get; set; }
        public string Max { get; set; }
        public string Step { get; set; }
        public List<string> Coefs { get; set; }

        public string CoefsString => string.Join(' ', Coefs);

        public string IntegrateMethod { get; set; }

        public override string ToString()
        {
            var strCoefs = string.Join(" ", Coefs);

            return $"{Min} {Max} {Step} {IntegrateMethod} {strCoefs}";
        }
    }
}

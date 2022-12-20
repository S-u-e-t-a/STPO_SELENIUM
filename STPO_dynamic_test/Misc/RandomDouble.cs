using System;


namespace STPO_dynamic_test.Misc
{
    public static class RandomDouble
    {
        public static double GetRandomDouble(double min, double max)
        {
            var rnd = new Random();

            return rnd.NextDouble() * (max - min) + min;
        }
    }
}

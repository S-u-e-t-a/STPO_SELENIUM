using System;


namespace STPO_dynamic_test.Misc
{
    public static class RandomInt
    {
        public static int GetRandomInt(int min, int max)
        {
            var rnd = new Random();

            return rnd.Next(min, max);
        }
    }
}

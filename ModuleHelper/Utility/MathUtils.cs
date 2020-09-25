using System;
using System.Collections.Generic;
using System.Text;

namespace ModuleHelper.Utility
{
    public static class MathUtils
    {
        public static double CalculateFrequency(int n)
        {
            var freq = 440.0 * Math.Pow(2.0, (n - 49.0) / 12.0);

            //we're starting from 4th octave
            return freq * 4;
        }

        public static int Modulo(int x, int m)
        {
            return (x % m + m) % m;
        }
    }
}
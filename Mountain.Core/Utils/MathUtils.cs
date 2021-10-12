using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.Core.Utils
{
    public static class MathUtils
    {
        public static void MinMax(ref int min, ref int max)
        {
            if (min > max)
            {
                int temp = min;
                min = max;
                max = temp;
            }
        }

        public static int Clamp(this int val, int inclMin, int inclMax)
        {
            return val < inclMin ? inclMin : val > inclMax ? inclMax : val;
        }
    }
}

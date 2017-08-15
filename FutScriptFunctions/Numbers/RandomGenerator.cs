using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutScriptFunctions.Numbers
{
    public static class RandomGenerator
    {
        static Random Generator = new Random();

        /// <summary>
        /// Returns a non-negative random integer.
        /// </summary>
        /// <returns></returns>
        public static int NextInt(int MaxValue = Int32.MaxValue)
        {
            return Generator.Next(MaxValue);
        }

        /// <summary>
        /// Returns a random integer within the specified range.
        /// </summary>
        /// <param name="MinValue"></param>
        /// <param name="MaxValue"></param>
        /// <returns></returns>
        public static int NextInt(int MinValue, int MaxValue)
        {
            return Generator.Next(MinValue, MaxValue);
        }

        /// <summary>
        /// Returns a random floating-point number that is greater than or equal
        /// to 0.0, and less than 1.0.
        /// </summary>
        /// <returns></returns>
        public static double NextDouble()
        {
            return Generator.NextDouble();
        }

        public static double NextDouble(double minValue, double maxValue)
        {
            if (minValue > maxValue) throw new ArgumentException("minValue>maxValue");
            return Generator.NextDouble() * (maxValue - minValue) + minValue;
        }

        public static bool RandomBool(double percent_chance_of_true)
        {
            return Generator.NextDouble() < (percent_chance_of_true / 100.0);
        }
    }
}

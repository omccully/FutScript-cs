using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace FutScriptFunctions.Numbers
{
    public class RandomRange : NumberGenerator
    {
        public double LowerLimit { get; private set; }
        public double UpperLimit { get; private set; }

        public RandomRange(double lower_limit, double upper_limit)
        {
            this.LowerLimit = lower_limit;
            this.UpperLimit = upper_limit;
        }

        public override int GetInt()
        {
            return (int)Generate(LowerLimit, UpperLimit);
        }

        public override double GetDouble()
        {
            return Generate(LowerLimit, UpperLimit);
        }

        public new static RandomRange Parse(string text)
        {
            text = text.Trim();
            Match m = new Regex(@"^([0-9\-.]+):([0-9\-.]+)$").Match(text);
            if (m.Success)
            {
                double min = double.Parse(m.Groups[1].Value);
                double max = double.Parse(m.Groups[2].Value);
                return new RandomRange(min, max);
            }
            throw new ArgumentException("RandomRange literal text not in correct format.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lower_limit">Inclusive</param>
        /// <param name="upper_limit">Inclusive</param>
        /// <returns></returns>
        public static int Generate(int lower_limit, int upper_limit)
        {
            //return Generator.Next(upper_limit - lower_limit) + lower_limit;
            return RandomGenerator.NextInt(lower_limit, upper_limit + 1);
        }


        public static double Generate(double lower_limit, double upper_limit)
        {
            //return Generator.NextDouble() * (upper_limit - lower_limit) + lower_limit;
            return RandomGenerator.NextDouble(lower_limit, upper_limit);
        }
    }
}

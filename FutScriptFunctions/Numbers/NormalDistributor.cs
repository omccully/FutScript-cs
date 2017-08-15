using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace FutScriptFunctions.Numbers
{
    /// <summary>
    /// RandomNumber object represents a normal-distribution generator.
    /// Static methods provide static access to a shared 
    /// </summary>
    public class NormalDistributor : Number
    {
        double mean;
        double std_dev;
        double LowerLimit;
        double UpperLimit;

        public NormalDistributor(double mean, double std_dev = 0.0, 
            double lower_limit = double.MinValue, double upper_limit = double.MaxValue)
        {
            this.mean = mean;
            this.std_dev = std_dev;
            this.LowerLimit = lower_limit;
            this.UpperLimit = upper_limit;
        }

        public override int GetInt()
        {
            return (int)Clamp(Math.Round(GetDouble()));
        }

        public override double GetDouble()
        {
            if (std_dev <= 0.0)
            {
                return (int)this.mean;
            }
            return Clamp(NormalDistribution(this.mean, this.std_dev));
        }

        double Clamp(double num)
        {
            return (num < LowerLimit ? LowerLimit : (num > UpperLimit ? UpperLimit : num));
        }
        public new static NormalDistributor Parse(string text)
        {
            text = text.Trim();
            const int MEAN_INDEX = 1;
            const int STD_DEV_INDEX = 2;
            const int LOWER_LIMIT_INDEX = 4;
            const int UPPER_LIMIT_INDEX = 5;

            Match m = new Regex(@"^([0-9\-.]+)~([0-9\-.]+)(\[([0-9\-.]+):([0-9\-.]+)\])?$").Match(text);
            if (m.Success)
            {
                if (!String.IsNullOrWhiteSpace(m.Groups[LOWER_LIMIT_INDEX].Value))
                {
                    return new NormalDistributor(double.Parse(m.Groups[MEAN_INDEX].Value),
                        double.Parse(m.Groups[STD_DEV_INDEX].Value),
                        double.Parse(m.Groups[LOWER_LIMIT_INDEX].Value),
                        double.Parse(m.Groups[UPPER_LIMIT_INDEX].Value));
                }
                else
                {
                    return new NormalDistributor(double.Parse(m.Groups[MEAN_INDEX].Value), 
                        double.Parse(m.Groups[STD_DEV_INDEX].Value));
                }
                
            }
            throw new ArgumentException("NormalDistributor literal text not in correct format.");
        }

        public static double NormalDistributionReal(double mean, double std_deviation)
        {
            double u1 = RandomGenerator.NextDouble();
            double u2 = RandomGenerator.NextDouble();
            double std_normal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                         Math.Sin(2.0 * Math.PI * u2);
            return mean + std_deviation * std_normal;
        }

        public static int NormalDistribution(double mean, double std_deviation)
        {
            return (int)NormalDistributionReal(mean, std_deviation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target_min">Target minimum</param>
        /// <param name="target_max">Target maximum</param>
        /// <param name="success_chance"></param>
        /// <returns>A random value by normal distribution
        /// that is between min and max MOST of the time. </returns>
        public static int WeightedRandom(int target_min, int target_max, double success_chance = 6.5)
        {
            double range = target_max - target_min;
            double mean = target_min + range / 2.0;
            return NormalDistribution(mean, range / success_chance);
        }
    }
}

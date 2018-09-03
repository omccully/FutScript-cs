using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutScriptFunctions.Numbers
{
    public class StaticNumber : NumberGenerator
    {
        double Number;

        public StaticNumber(double number)
        {
            this.Number = number;
        }

        public override int GetInt()
        {
            return (int)Math.Round(Number);
        }

        public override double GetDouble()
        {
            return Number;
        }

        public new static StaticNumber Parse(string text)
        {
            return new StaticNumber(double.Parse(text));
        }
    }
}

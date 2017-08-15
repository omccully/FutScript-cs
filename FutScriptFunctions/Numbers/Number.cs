using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutScriptFunctions.Numbers
{
    public abstract class Number
    {
        abstract public int GetInt();
        abstract public double GetDouble();

        public delegate Number Parser(string text);

        public static Number Parse(string text)
        {
            Parser[] parsers = { // be careful of order here
                StaticNumber.Parse,
                RandomRange.Parse,
                NormalDistributor.Parse
            };

            foreach (Parser parser in parsers)
            {
                try
                {
                    return parser(text);
                }
                catch
                {

                }
            }

            throw new Exception($"Invalid Number literal '{text}'");
        }
    }
}

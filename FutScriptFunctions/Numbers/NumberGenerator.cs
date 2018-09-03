using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutScriptFunctions.Numbers
{
    public abstract class NumberGenerator
    {
        abstract public int GetInt();
        abstract public double GetDouble();

        public delegate NumberGenerator Parser(string text);

        public static NumberGenerator Parse(string text)
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

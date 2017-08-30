using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutScriptFunctions.Script
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class FunctionParserAttribute : Attribute
    {
        public readonly string Signature;
        public readonly string Category;
        public FunctionParserAttribute(string Signature, string Category = "Helpers")
        {
            this.Signature = Signature;
            this.Category = Category;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutScriptFunctions.Script
{
    public class ScriptFile
    {
        public string Path { get; private set; }
        public List<FunctionCall> FunctionCalls { get; set; }

        public ScriptFile(string Path, List<FunctionCall> FunctionCalls)
        {
            this.Path = Path;
            this.FunctionCalls = FunctionCalls;
        }
    }
}

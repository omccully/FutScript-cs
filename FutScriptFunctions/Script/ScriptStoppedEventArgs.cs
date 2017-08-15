using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutScriptFunctions.Script
{
    public class ScriptStoppedEventArgs : EventArgs
    {
        public Exception ExceptionThrown { get; private set; }

        public ScriptStoppedEventArgs(Exception ExceptionThrown = null)
        {
            this.ExceptionThrown = ExceptionThrown;
        }
    }
}

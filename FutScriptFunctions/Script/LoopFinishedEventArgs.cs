using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutScriptFunctions.Script
{
    public class LoopFinishedEventArgs : EventArgs
    {
        public int LoopCount { get; set; }
        public TimeSpan TimeElapsed { get; set; }

        public LoopFinishedEventArgs(int loop_count, TimeSpan time_elapsed)
        {
            this.LoopCount = loop_count;
            this.TimeElapsed = time_elapsed;
        }
    }
}

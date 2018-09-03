using FutScriptFunctions.Win32API;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FutScriptFunctions.Mouse
{
    public class AbsoluteCursorLocator : ICursorLocator
    {
        public Point GetLocation()
        {
            return User32.RelativeToAbsolutePoint(Cursor.Position);
        }
    }
}

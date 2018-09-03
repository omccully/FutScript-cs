using FutScriptFunctions.Win32API;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FutScriptFunctions.Mouse.LocationSetBehaviors
{
    public class MouseEventCursorLocationSetter : ICursorLocationSetter
    {
        // using mouse_event
        /// <summary>
        /// Requires mouse acceleration to be off
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        public void JumpPosition(int dx, int dy)
        {
            User32.mouse_event(0x0001, dx, dy, 0, 0);
        }

        // using mouse_event
        public void SetPosition(int x, int y)
        {

            Point loc = User32.RelativeToAbsolutePoint(Cursor.Position);
            JumpPosition(x - loc.X, y - loc.Y);
        }

        public override string ToString()
        {
            return "mouse_event";
        }
    }
}

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
    public class SetCursorPosCursorLocationSetter : ICursorLocationSetter
    {
        public void JumpPosition(int dx, int dy)
        {
            Point relative_start = Cursor.Position;
            User32.SetCursorPos(relative_start.X + dx, relative_start.Y + dy);
        }

        public void SetPosition(int x, int y)
        {
            Point relative = User32.AbsoluteToRelativePoint(x, y);
            User32.SetCursorPos(relative.X, relative.Y);
        }

        public override string ToString()
        {
            return "SetCursorPos";
        }
    }
}

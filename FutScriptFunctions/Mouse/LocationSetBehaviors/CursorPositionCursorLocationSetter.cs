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
    public class CursorPositionCursorLocationSetter : ICursorLocationSetter
    {
        public void JumpPosition(int dx, int dy)
        {
            Point start = Cursor.Position;
            Cursor.Position = new Point(start.X + dx, start.Y + dy);
        }

        public void SetPosition(int x, int y)
        {
            Cursor.Position = User32.AbsoluteToRelativePoint(x, y);
        }

        public override string ToString()
        {
            return "Cursor.Position";
        }
    }
}

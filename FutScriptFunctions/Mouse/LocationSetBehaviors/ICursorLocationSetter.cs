using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutScriptFunctions.Mouse.LocationSetBehaviors
{
    public interface ICursorLocationSetter
    {
        void JumpPosition(int dx, int dy);

        void SetPosition(int x, int y);
    }
}

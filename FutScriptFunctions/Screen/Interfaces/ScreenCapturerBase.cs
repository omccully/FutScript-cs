using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutScriptFunctions.Screen
{
    public abstract class ScreenCapturerBase
    {
        public abstract Color GetColorOfPx(int x, int y);

        public Color GetColorOfPx(Point p)
        {
            return GetColorOfPx(p.X, p.Y);
        }

        public abstract Bitmap CaptureScreenArea(int X, int Y, int Width, int Height);

        public Bitmap CaptureScreenArea(Point TopLeft, Point BottomRight)
        {
            return CaptureScreenArea(TopLeft.X, TopLeft.X,
                (BottomRight.X - TopLeft.X) + 1, (BottomRight.Y - TopLeft.Y) + 1);
        }

        public Bitmap CaptureScreenArea(Rectangle rect)
        {
            return CaptureScreenArea(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public abstract Bitmap CaptureAllScreens();
    }
}

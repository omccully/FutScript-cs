using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace FutScriptFunctions.Win32API
{
    internal static class User32
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);

        [DllImport("User32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern short VkKeyScan(char ch);

        // SetCursorPos does not create events that are captured by mouse hooks
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Point lpPoint);


        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        public const int MOUSEEVENTF_RIGHTUP = 0x10;
        public const int MOUSEEVENTF_MIDDLEDOWN = 0x20;
        public const int MOUSEEVENTF_MIDDLEUP = 0x40;

        public const int SM_XVIRTUALSCREEN = 76;
        public const int SM_YVIRTUALSCREEN = 77;
        public const int SM_CXVIRTUALSCREEN = 78;
        public const int SM_CYVIRTUALSCREEN = 79;

        // X offset between leftmost pixel on the display and the leftmost pixel of the main display
        public readonly static int ScreenOffsetX = User32.GetSystemMetrics(SM_XVIRTUALSCREEN);

        // Y offset between uppermost pixel on the display and the uppermost pixel of the main display
        public readonly static int ScreenOffsetY = User32.GetSystemMetrics(SM_YVIRTUALSCREEN);

        // Combined width of all screens
        public readonly static int ScreenWidth = User32.GetSystemMetrics(SM_CXVIRTUALSCREEN);

        // Combined height of all screens
        public readonly static int ScreenHeight = User32.GetSystemMetrics(SM_CYVIRTUALSCREEN);

        #region Point Converters
        // Mouse cursor coordinates and pixel locations of a full-Screen screenshot do not always match
        // If the user's primary monitor is on the right with a secondary monitor on the left,
        // point (0,0) of the screenshot will be on the top left corner of the left monitor,
        // but point (0,0) for mouse coordinates will be on the top left corner of the RIGHT monitor.
        // Therefore, we need methods to convert between the two.

        /// <summary>
        /// Converts a relative (cursor position) point to an absolute (screenshot) point
        /// </summary>
        /// <param name="p"></param>
        /// <returns>Point with nonnegative coordinates. Usable for getting pixels of a screenshot</returns>
        public static Point RelativeToAbsolutePoint(Point p)
        {
            return RelativeToAbsolutePoint(p.X, p.Y);
        }

        /// <summary>
        /// Converts a relative (cursor position) point to an absolute (screenshot) point
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns>Point with nonnegative coordinates. Usable for getting pixels of a screenshot</returns>
        public static Point RelativeToAbsolutePoint(int X, int Y)
        {
            // cursor points may be negative.
            // add offsets to ensure they are always nonnegative
            return new Point(X - ScreenOffsetX, Y - ScreenOffsetY);
        }

        /// <summary>
        /// Converts an absolute (screenshot) point to a relative (cursor position) point
        /// </summary>
        /// <param name="p"></param>
        /// <returns>Point relative to top left pixel of main display. Usable for setting the cursor position</returns>
        public static Point AbsoluteToRelativePoint(Point p)
        {
            return AbsoluteToRelativePoint(p.X, p.Y);
        }

        /// <summary>
        /// Converts an absolute (screenshot) point to a relative (cursor position) point
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns>Point relative to top left pixel of main display. Usable for setting the cursor position</returns>
        public static Point AbsoluteToRelativePoint(int X, int Y)
        {
            return new Point(X + ScreenOffsetX, Y + ScreenOffsetY);
        }
        #endregion
    }
}


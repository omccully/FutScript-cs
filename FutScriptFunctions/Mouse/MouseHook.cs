using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace FutScriptFunctions.Mouse
{
    public enum MouseEventIdentifier
    {
        MouseMove = 0x0200,
        LeftDown = 0x0201,
        LeftUp = 0x0202,
        RightDown = 0x0204,
        RightUp = 0x0205,
        WheelDown = 0x207,
        WheelUp = 0x208,
        MouseWheel = 0x020A,
        MouseHWheel = 0x020E
    }

    public delegate bool MouseCallback(Point p, MouseEventIdentifier id, uint timestamp);

    public class MouseHook
    {
        #region Constant, Structure and Delegate Definitions
        public delegate int mouseHookProc(int code, int wParam, ref MouseHookStruct lParam);

        public struct MouseHookStruct
        {
            public Point pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public int dwExtraInfo;
        }

        const int WH_MOUSE_LL = 14;
        #endregion

        #region Instance Variables
        IntPtr mousehook = IntPtr.Zero;

        mouseHookProc mouseHookProcDel;

        MouseCallback mouseCallback;
        #endregion

        #region Constructors and Destructors
        public MouseHook(MouseCallback mouseCallback)
        {
            this.mouseHookProcDel = mouseEventCall;
            this.mouseCallback = mouseCallback;

            Hook();
        }

        ~MouseHook()
        {
            Unhook();
        }
        #endregion

        #region Public Methods
        public void Hook()
        {
            IntPtr hInstance = LoadLibrary("User32");
            mousehook = SetWindowsHookEx(WH_MOUSE_LL, mouseHookProcDel, hInstance, 0);
        }

        public void Unhook()
        {
            UnhookWindowsHookEx(mousehook);
        }

        #endregion

        int mouseEventCall(int code, int wParam, ref MouseHookStruct lParam)
        {
            if (mouseCallback(lParam.pt, (MouseEventIdentifier)wParam, lParam.time)) return 1;
            return CallNextHookEx(mousehook, code, wParam, ref lParam);
        }

        #region DLL imports
        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, mouseHookProc callback, IntPtr hInstance, uint threadId);
        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);
        [DllImport("user32.dll")]
        static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref MouseHookStruct lParam);
        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);
        #endregion
    }
}

using FutScriptFunctions.Win32API;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutScriptFunctions.Screen
{
    public class ScreenCapturer : ScreenCapturerBase
    {
        public override Bitmap CaptureScreenArea(int X, int Y, int Width, int Height)
        {
            IntPtr hdcSrc = GDI32.CreateDC("DISPLAY", null, null, IntPtr.Zero);
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, Width, Height);
            GDI32.SelectObject(hdcDest, hBitmap);
            GDI32.BitBlt(hdcDest, 0, 0, Width, Height, hdcSrc, User32.ScreenOffsetX + X, User32.ScreenOffsetY + Y,
                        0x40000000 | 0x00CC0020); //SRCCOPY AND CAPTUREBLT
            Bitmap bmpret = Bitmap.FromHbitmap(hBitmap);
            GDI32.DeleteDC(hdcSrc);
            GDI32.DeleteDC(hdcDest);
            GDI32.DeleteObject(hBitmap);
            return bmpret;
        }

        public override Color GetColorOfPx(int x, int y)
        {
            Bitmap bmp = CaptureScreenArea(x, y, 1, 1);
            Color answer = bmp.GetPixel(0, 0);
            bmp.Dispose();
            return answer;
        }

        /// <summary>
        /// Captures a screenshot of all screens
        /// </summary>
        /// <returns>A screenshot of all screens</returns>
        public override Bitmap CaptureAllScreens()
        {
            return CaptureScreenArea(0, 0, User32.ScreenWidth, User32.ScreenHeight);
        }

        // <summary>
        /// Captures a screenshot of a given window
        /// </summary>
        /// <param name="handle">A handle to a window</param>
        /// <returns>A screenshot of the given window</returns>
        public static Bitmap CaptureWindow(IntPtr handle)
        {
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            User32.RECT windowRect = new User32.RECT();
            User32.GetWindowRect(handle, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
            GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
            GDI32.SelectObject(hdcDest, hOld);
            GDI32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);
            Bitmap bmp = Bitmap.FromHbitmap(hBitmap);
            GDI32.DeleteObject(hBitmap);
            return bmp;
        }

        /// <summary>
        /// Captures a screenshot of the main display
        /// </summary>
        /// <returns>A screenshot of the main display</returns>
        public static Bitmap CaptureMainDisplay()
        {
            return CaptureWindow(User32.GetDesktopWindow());
        }

        /// <summary>
        /// Captures a screenshot of the foreground window
        /// </summary>
        /// <returns>A screenshot of the foreground window</returns>
        public static Bitmap CaptureForegroundWindow()
        {
            return CaptureWindow(User32.GetForegroundWindow());
        }
    }
}

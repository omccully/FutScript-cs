using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FutScriptFunctions.Screen
{
    public class ScreenColorDetector
    {
        // some ColorDetection methods that rely on a ScreenCapture 
        // object are put here to make them testable

        ScreenCapturerBase screen_capture;
        public ScreenColorDetector(ScreenCapturerBase screen_capturer = null)
        {
            this.screen_capture = screen_capturer ?? new ScreenCapturer();
        }

        /// <summary>
        /// Waits for a pixel to turn a certain color
        /// </summary>
        /// <param name="coords">Absolute pixel coordinates</param>
        /// <param name="checker">Color rules. <see cref="ColorRule"/></param>
        /// <param name="timeout_ms">How long to wait before giving up, in milliseconds. 0 for unlimited time.</param>
        /// <returns></returns>
        public bool WaitForPx(Point coords, ColorChecker checker, int timeout_ms = 0)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (!checker(screen_capture.GetColorOfPx(coords)))
            {
                if (timeout_ms != 0 && stopwatch.ElapsedMilliseconds >= timeout_ms)
                    return false; // timed out
                Thread.Sleep(50);
            }
            return true; // success
        }

        /// <summary>
        /// Checks if a screen area includes a given color
        /// </summary>
        /// <param name="screen_area">Rectangle representing coordinates of the screen area</param>
        /// <param name="checker">Color rules. <see cref="ColorRule"/></param>
        /// <returns></returns>
        public bool ScreenAreaIncludesColor(Rectangle screen_area, ColorChecker checker)
        {
            using (Bitmap bmp = screen_capture.CaptureScreenArea(screen_area))
            {
                return bmp.IncludesColor(checker);
            } // implicit bmp.Dispose()
        }

        /// <summary>
        /// Waits for a certain color pixel to appear in the <paramref name="screen_area"/>
        /// </summary>
        /// <param name="screen_area">Rectangle representing coordinates of the screen area</param>
        /// <param name="checker">Color rules. <see cref="ColorRule"/></param>
        /// <param name="timeout_ms">How long to wait before giving up, in milliseconds. 0 for unlimited time.</param>
        /// <returns></returns>
        public bool WaitForAreaInclude(Rectangle screen_area, ColorChecker checker, int timeout_ms = 0)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (!ScreenAreaIncludesColor(screen_area, checker))
            {
                if (timeout_ms != 0 && stopwatch.ElapsedMilliseconds >= timeout_ms)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Waits for a given number of pixels within an area of the screen to change color.
        /// </summary>
        /// <param name="screen_area">Rectangle representing coordinates of the screen area</param>
        /// <param name="pixelreq">Number of pixels that must change</param>
        /// <param name="timeout_ms">How long to wait before giving up, in milliseconds. 0 for unlimited time.</param>
        /// <param name="comparer">A color comparer</param>
        /// <returns></returns>
        public bool WaitForAreaChange(Rectangle screen_area, int pixelreq, int timeout_ms = 0, ColorComparer comparer = null)
        {
            if (comparer == null) comparer = ColorCompare.Strict;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            using (Bitmap original_bmp = screen_capture.CaptureScreenArea(screen_area))
            {
                while (true)
                {
                    using (Bitmap latest_bmp = screen_capture.CaptureScreenArea(screen_area))
                    {
                        if (ColorDetection.BitmapsHaveNumberOfPixelsDifferent(original_bmp, latest_bmp, pixelreq, comparer))
                        {
                            // success! enough pixel changes were found.
                            return true;
                        }

                        if (timeout_ms != 0 && stopwatch.ElapsedMilliseconds >= timeout_ms)
                        {
                            // timed out
                            return false;
                        }

                        Thread.Sleep(100);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the location of a color within a given area of the screen
        /// </summary>
        /// <param name="screen_area">Rectangle representing coordinates of the screen area</param>
        /// <param name="checker">Color rules. <see cref="ColorRule"/></param>
        /// <returns></returns>
        public Point LocationOfColorWithinScreenArea(Rectangle screen_area, ColorChecker checker)
        {
            using (Bitmap bmp = screen_capture.CaptureScreenArea(screen_area))
            {
                // get location of color relative to start Point
                Point loc = bmp.LocationOfColor(checker);

                if (loc.X == -1 && loc.Y == -1) return loc; // not found

                // adjust location so location refers to the absolute screen coords
                loc.X += screen_area.X;
                loc.Y += screen_area.Y;

                return loc;
            } // implicit bmp.Dispose()
        }
    }
}

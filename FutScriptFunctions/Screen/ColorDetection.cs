using System;
using System.Drawing;
using System.Diagnostics;
using System.Threading;

namespace FutScriptFunctions.Screen
{
    // TODO: make code generator to generate method overloads for 
    // methods that accept Point or Rectangle objects for coordinates.
    public static class ColorDetection
    {
        /// <summary>
        /// Extension for Bitmap class
        /// </summary>
        /// <param name="bmp">A Bitmap image</param>
        /// <param name="checker">Color rules. <see cref="ColorRule"/></param>
        /// <returns></returns>
        public static bool IncludesColor(this Bitmap bmp, ColorChecker checker)
        {
            Point loc = bmp.LocationOfColor(checker);
            return (loc.X != -1) && (loc.Y != -1);
        }

        /// <summary>
        /// Finds a location of a given color within a Bitmap image.
        /// </summary>
        /// <param name="bmp">A Bitmap image</param>
        /// <param name="checker">Color rules. <see cref="ColorRule"/></param>
        /// <returns>Returns Point representing coordinates, or (-1, -1) if not found.</returns>
        public static Point LocationOfColor(this Bitmap bmp, ColorChecker checker)
        {
            // TODO: Update this implementation to increment y and x by about y/10 and x/10, respectively.
            // This will greatly improve the efficiency of the algorithm, since similar colors are normally
            // grouped together on a computer screen.
            // Be careful to ensure that all coordinates are checked (do proper testing on this).

            Color pix;
            for (int y = 0; y <= bmp.Height; y++)
            {
                for (int x = 0; x <= bmp.Width; x++)
                {
                    pix = bmp.GetPixel(x, y);

                    if (checker(pix))
                    {
                        return new Point(x, y);
                    }
                }
            }

            return new Point(-1, -1); // color not found
        }

        /// <summary>
        /// Checks if a screen area includes a given color
        /// </summary>
        /// <param name="screen_area">Rectangle representing coordinates of the screen area</param>
        /// <param name="checker">Color rules. <see cref="ColorRule"/></param>
        /// <returns></returns>
        public static bool ScreenAreaIncludesColor(Rectangle screen_area, ColorChecker checker)
        {
            using (Bitmap bmp = ScreenCapture.CaptureScreenArea(screen_area))
            {
                return bmp.IncludesColor(checker);
            } // implicit bmp.Dispose()
        }

        /// <summary>
        /// Gets the location of a color within a given area of the screen
        /// </summary>
        /// <param name="screen_area">Rectangle representing coordinates of the screen area</param>
        /// <param name="checker">Color rules. <see cref="ColorRule"/></param>
        /// <returns></returns>
        public static Point LocationOfColorWithinScreenArea(Rectangle screen_area, ColorChecker checker)
        {
            using (Bitmap bmp = ScreenCapture.CaptureScreenArea(screen_area))
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

        /// <summary>
        /// Waits for a pixel to turn a certain color
        /// </summary>
        /// <param name="coords">Absolute pixel coordinates</param>
        /// <param name="checker">Color rules. <see cref="ColorRule"/></param>
        /// <param name="timeout_ms">How long to wait before giving up, in milliseconds. 0 for unlimited time.</param>
        /// <returns></returns>
        public static bool WaitForPx(Point coords, ColorChecker checker, int timeout_ms = 0)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (!checker(ScreenCapture.GetColorOfPx(coords)))
            {
                if (timeout_ms != 0 && stopwatch.ElapsedMilliseconds >= timeout_ms)
                    return false; // timed out
                Thread.Sleep(50);
            }
            return true; // success
        }

        /// <summary>
        /// Waits for a certain color pixel to appear in the <paramref name="screen_area"/>
        /// </summary>
        /// <param name="screen_area">Rectangle representing coordinates of the screen area</param>
        /// <param name="checker">Color rules. <see cref="ColorRule"/></param>
        /// <param name="timeout_ms">How long to wait before giving up, in milliseconds. 0 for unlimited time.</param>
        /// <returns></returns>
        public static bool WaitForAreaInclude(Rectangle screen_area, ColorChecker checker, int timeout_ms = 0)
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
        public static bool WaitForAreaChange(Rectangle screen_area, int pixelreq, int timeout_ms = 0, ColorComparer comparer = null)
        {
            if (comparer == null) comparer = ColorCompare.Strict;
            
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            using (Bitmap original_bmp = ScreenCapture.CaptureScreenArea(screen_area))
            {
                while (true)
                {
                    using (Bitmap latest_bmp = ScreenCapture.CaptureScreenArea(screen_area))
                    {
                        if (BitmapsHaveNumberOfPixelsDifferent(original_bmp, latest_bmp, pixelreq, comparer))
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
        /// Checks if two images have at least <paramref name="PixelRequirement"/> number of pixels
        /// that are different based on <paramref name="comparer"/>
        /// </summary>
        /// <param name="image_a"></param>
        /// <param name="image_b"></param>
        /// <param name="PixelRequirement"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        private static bool BitmapsHaveNumberOfPixelsDifferent(Bitmap image_a, Bitmap image_b, int PixelRequirement, ColorComparer comparer = null)
        {
            if (image_a.Size != image_b.Size)
            {
                throw new ArgumentException("image_a and image_b must be the same size.");
            }

            int different_pixels_found = 0;

            for (int y = 0; y <= image_a.Height; y++)
            {
                for (int x = 0; x <= image_a.Width; x++)
                {
                    // check if latest (x,y) pixel is "different" from the original
                    if (!comparer(image_a.GetPixel(x, y), image_b.GetPixel(x, y)))
                    {
                        // this pixel is different from the original
                        different_pixels_found++;
                        if (different_pixels_found >= PixelRequirement)
                        {
                            // success! enough pixel changes were found.
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}

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

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    if (checker(bmp.GetPixel(x, y)))
                    {
                        return new Point(x, y);
                    }
                }
            }

            return new Point(-1, -1); // color not found
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
        public static bool BitmapsHaveNumberOfPixelsDifferent(Bitmap image_a, Bitmap image_b, int PixelRequirement, ColorComparer comparer = null)
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

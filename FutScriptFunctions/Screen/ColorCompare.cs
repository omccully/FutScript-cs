using System;
using System.Drawing;

namespace FutScriptFunctions.Screen
{
    public delegate bool ColorComparer(Color a, Color b);

    public static class ColorCompare
    {
        public static bool Strict(Color a, Color b)
        {
            return a.ToArgb() == b.ToArgb();
        }

        public static ColorComparer TolerantWithin(byte tolerance)
        {
            return delegate (Color a, Color b)
            {
                return AreColorsTolerant(a, b, tolerance);
            };
        }

        /// <summary>
        /// Checks to see if two colors are within a tolerance range.
        /// </summary>
        /// <param name="one">A color</param>
        /// <param name="two">A color</param>
        /// <param name="tolerance">How far off the color can be to be considered the same color. </param>
        /// <returns></returns>
        public static bool AreColorsTolerant(Color one, Color two, byte tolerance)
        {
            return (Math.Abs(one.R - two.R) <= tolerance &&
                    Math.Abs(one.G - two.G) <= tolerance &&
                    Math.Abs(one.B - two.B) <= tolerance);
        }
    }
}

using System;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

namespace FutScriptFunctions.Screen
{
    public delegate bool ColorChecker(Color sample);

    /// <summary>
    /// Static methods to generate ColorChecker delegates to pass to ColorDetection methods
    /// </summary>
    public static class ColorRule
    {
        /// <summary>
        /// Checks if sample color is equivalent to <paramref name="chosen_color"/>
        /// </summary>
        /// <param name="chosen_color"></param>
        /// <returns></returns>
        public static ColorChecker Is(Color chosen_color)
        {
            return delegate (Color sample)
            {
                return sample.ToArgb() == chosen_color.ToArgb();
            };
        }

        public static ColorChecker IsNot(Color chosen_color)
        {
            return delegate (Color sample)
            {
                return sample.ToArgb() != chosen_color.ToArgb();
            };
        }

        /// <summary>
        /// Checks if sample color is within a tolerance range of <paramref name="chosen_color"/>
        /// </summary>
        /// <param name="chosen_color"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static ColorChecker IsTolerantOf(Color chosen_color, byte tolerance)
        {
            return delegate (Color sample)
            {
                return ColorCompare.AreColorsTolerant(chosen_color, sample, tolerance);
            };
        }

        public static ColorChecker IsNotTolerantOf(Color chosen_color, byte tolerance)
        {
            return delegate (Color sample)
            {
                return !ColorCompare.AreColorsTolerant(chosen_color, sample, tolerance);
            };
        }

        public static ColorChecker Not(ColorChecker cc)
        {
            return delegate (Color sample)
            {
                return !cc(sample);
            };
        }


        /// <summary>
        /// Parses a color rule to create a ColorChecker.
        /// <example>
        /// ff0000 = exactly #FF0000
        /// ff0000~10 = #FF0000 with a tolerance of 10
        /// !ff0000 = anything except #FF0000'
        /// !FF0000~10 = any not within a tolerance of 10 of #FF0000
        /// </example>
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static ColorChecker Parse(string code)
        {
            Match match = new Regex("(!?)([A-Fa-f0-9]{6})(~[0-9]+)?").Match(code);
            if (!match.Success) throw new FormatException($"Invalid ColorRule literal '{code}'");
            
            bool not_operator = match.Groups[1].Value == "!";
            Color color = HexToColor(match.Groups[2].Value);
            byte tolerance = String.IsNullOrEmpty(match.Groups[4].Value) ?
                (byte)0 : byte.Parse(match.Groups[4].Value);

            if(not_operator)
            {
                if(tolerance == 0) return IsNot(color);
                return IsNotTolerantOf(color, tolerance);
            }
            else
            {
                if (tolerance == 0) return Is(color);
                return IsTolerantOf(color, tolerance);
            }
        }

        /// <summary>
        /// Converts color in hex format to a Color
        /// </summary>
        /// <param name="hexColor">Color in hex format (6 digits)</param>
        /// <returns>A Color representation of the hex input color</returns>
        static Color HexToColor(string hexColor)
        {
            int red = 0;
            int green = 0;
            int blue = 0;

            if (hexColor.Length == 6)
            {
                red = Int32.Parse(hexColor.Substring(0, 2), NumberStyles.AllowHexSpecifier);
                green = Int32.Parse(hexColor.Substring(2, 2), NumberStyles.AllowHexSpecifier);
                blue = Int32.Parse(hexColor.Substring(4, 2), NumberStyles.AllowHexSpecifier);
            }

            return Color.FromArgb(red, green, blue);
        }
    }
}

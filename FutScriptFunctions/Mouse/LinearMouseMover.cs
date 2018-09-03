using System;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using FutScriptFunctions.Win32API;

namespace FutScriptFunctions.Mouse
{
    public class LinearMouseMover : MouseActionPerformer
    {
        /// <summary>
        /// Moves mouse cursor in a straight line to its destination relative to the starting cursor position
        /// </summary>
        /// <param name="dx">X coordinate relative to current cursor position</param>
        /// <param name="dy">Y coordinate relative to current cursor position</param>
        /// <param name="speed">Speed in pixels per second</param>
        public override void MoveFrom(int dx, int dy, double speed = SpeedDefault)
        {
            Point current_location = Location;
            MoveTo(current_location.X + dx, current_location.Y + dy, speed);
        }

        /// <summary>
        /// Moves mouse cursor in a straight line to its destination
        /// </summary>
        /// <param name="x">Absolute X coordinate to move to</param>
        /// <param name="y">Absolute Y coordinate to move to</param>
        /// <param name="speed">Speed in pixels per second</param>
        public override void MoveTo(int x, int y, double speed = SpeedDefault)
        {
            // cursor movement functions use relative points
            Point relative = User32.AbsoluteToRelativePoint(x, y);
            x = relative.X;
            y = relative.Y;

            // used to check if integer value of position changed since last iteration
            Point loc_before_sleep = Cursor.Position;

            double current_x = loc_before_sleep.X;
            double current_y = loc_before_sleep.Y;
            double dest_x = (double)x;
            double dest_y = (double)y;

            while (Math.Abs(current_x - dest_x) > 0.5 || Math.Abs(current_y - dest_y) > 0.5)
            {
                double displacement_x = dest_x - current_x;
                double displacement_y = dest_y - current_y;

                // pythagorean theorem to get displacement from origin to dest
                double distance_direct = Math.Sqrt(displacement_x * displacement_x +
                    displacement_y * displacement_y);

                // hypotenuse is the portion of distance_direct that is being traversed this iteration
                // ({speed} pixels/sec) * (1 sec/1000 ms) * ({_PollingPeriod} ms) = 
                double hypotenuse = Math.Min(distance_direct, speed * PollingPeriod / 1000.0);

                if (Math.Abs(displacement_x) < 0.1)
                {
                    // no X displacement, so we're directly above or below destination
                    // this prevents divide by 0 from calculating tan_ratio
                    current_y += hypotenuse * (displacement_y < 0.0 ? -1.0 : 1.0);
                }
                else
                {
                    // x is adjacent component
                    // y is opposite component
                    double tan_ratio = displacement_y / displacement_x;

                    // the sign of tan_ratio doesn't matter to Atan
                    double angle_to_dest = Math.Atan(tan_ratio);

                    // Math.Cos(angle_to_dest) * hypotenuse  --- gets the correct x magnitude
                    // * (displacement_x < 0.0 ? -1.0 : 1.0) --- fixes the sign for the x component
                    double x_add = Math.Cos(angle_to_dest) * hypotenuse * (displacement_x < 0.0 ? -1.0 : 1.0);

                    // multiplying these always gives the correct sign for y
                    double y_add = x_add * tan_ratio;

                    current_x += x_add;
                    current_y += y_add;
                }

                int new_x = (int)Math.Round(current_x);
                int new_y = (int)Math.Round(current_y);

                loc_before_sleep = new Point(new_x, new_y);
                if (Cursor.Position != loc_before_sleep)
                {
                    //Cursor.Position = loc_before_sleep;
                    Point absolute = User32.RelativeToAbsolutePoint(loc_before_sleep);
                    SetPosition(absolute.X, absolute.Y);
                }

                WaitForNextPoll();

                // in case human user moved mouse cursor during the WaitForNextPoll():
                // if the cursor is not where it is expected to be, 
                // reset current_x and current_y back to the actual position
                if (Cursor.Position != loc_before_sleep)
                {
                    // TODO: just use JumpPosition instead and just keep track of relative changes?
                    current_x = Cursor.Position.X;
                    current_y = Cursor.Position.Y;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using FutScriptFunctions.Numbers;
using System.Drawing;

namespace FutScriptFunctions.Mouse.Recorded
{
    public class RecordedMouseMover : MouseActionPerformer
    {
        RecordedMousePaths Paths { get; set; }

        MouseActionPerformer BackupMethod; 

        public RecordedMouseMover(RecordedMousePaths Paths, MouseActionPerformer BackupMethod)
        {
            if (Paths == null || BackupMethod == null) throw new ArgumentNullException();

            this.Paths = Paths;
            this.BackupMethod = BackupMethod;
        }

        public override void MoveTo(int x, int y, double speed = SPEED_DEFAULT)
        {
            MoveToAndClick(x, y, null, speed); // move, but don't click
        }

        public override void MoveFrom(int dx, int dy, double speed = SPEED_DEFAULT)
        {
            Point current_location = Location;
            MoveTo(current_location.X + dx, current_location.Y + dy, speed);
        }

        public override void MoveToAndClick(int x, int y, Button ButtonToClick, double speed = SPEED_DEFAULT)
        {
            Point destination = new Point(x, y);
            Point current_location = new Point();
            bool clicked = false;

            // break out of loop if it clicked regardless of location because
            // the cursor may have moved during the click
            while ((current_location = Location) != destination && !clicked)
            {
                int dx = x - current_location.X;
                int dy = y - current_location.Y;

                SerializableCursorPath PathToUse = null;
                try
                {
                    PathToUse = Paths.GetRandomSuitablePath(dx, dy);
                }
                catch
                {
                    BackupMethod.MoveToAndClick(x, y, ButtonToClick, speed);
                    return;
                }

                clicked = MoveCursorUsingPath(PathToUse, dx, dy, speed, ButtonToClick,
                    new Rectangle(x, y, 1, 1));
            }
        }

        bool MoveCursorUsingPath(SerializableCursorPath path, int dx, int dy, double speed = SPEED_DEFAULT, Button ButtonToClick=null,
            Rectangle? OnlyClickWithin = null)
        {
            // if currently on same axis as destination, then use 1.0 for the scale
            // this may move your cursor off of the axis, but the next move should put you
            // in the correct spot
            double scale_x = (dx == 0 || path.Destination.X == 0 ? 1.0 : (double)dx / path.Destination.X);
            double scale_y = (dy == 0 || path.Destination.Y == 0 ? 1.0 : (double)dy / path.Destination.Y);

            /*double path_displacement_magnitude = Math.Sqrt(path.Destination.X * path.Destination.X +
                path.Destination.Y * path.Destination.Y);
            double path_speed = (path_displacement_magnitude / path.MouseDownTime) * 1000;
            double scale_speed = speed / path_speed;*/

            double scale_speed = speed / SPEED_DEFAULT;

            return FollowPath(path.Path, scale_x, scale_y, scale_speed,
                path.MouseDownTime, ButtonToClick);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="scale_x"></param>
        /// <param name="scale_y"></param>
        /// <param name="scale_speed"></param>
        /// <param name="MouseDownAtTime">if <paramref name="ButtonToClick"/> is null, 
        /// then the method returns after completing that movement</param>
        /// <param name="ButtonToClick">Button to click starting at <paramref name="MouseDownAtTime"/>
        /// and ending at the last datapoint</param>
        /// <param name="OnlyClickWithin"></param>
        /// <returns>True if success, false if a requested click did not occur</returns>
        bool FollowPath(int[][] path, double scale_x = 1.0, double scale_y = 1.0, 
            double scale_speed = 1.0, int MouseDownAtTime=Int32.MaxValue, Button ButtonToClick=null,
            Rectangle? OnlyClickWithin = null)
        {
            //scale_speed = NormalDistributor.NormalDistributionReal(scale_speed, scale_speed / 9);
            if (scale_speed < 0.1) scale_speed = 0.1;

            int position_x = 0; // non-scaled position
            int position_y = 0; // non-scaled position
            int path_index = 0;

            if (path.Length == 0) throw new ArgumentException("path is empty");
            int endtime = path[path.Length - 1][0];

            bool MouseDown = false;

            for (double time = PollingPeriod * scale_speed; 
                true; time += PollingPeriod * scale_speed)
            {
                int dx = 0, dy = 0;

                // total dx and dy values since last iteration of loop to current {time}
                while (path_index < path.Length && path[path_index][0] < time)
                {
                    int[] coords = path[path_index];
                    dx += coords[1] - (position_x + dx);
                    dy += coords[2] - (position_y + dy);
                    path_index++;
                }

                int temp_x = (int)(Math.Round((position_x + dx) * scale_x) - Math.Round(position_x * scale_x));
                int temp_y = (int)(Math.Round((position_y + dy) * scale_y) - Math.Round(position_y * scale_y));

                WaitForNextPoll();
                if (temp_x != 0 || temp_y != 0)
                {
                    JumpPosition(temp_x, temp_y);
                    position_x += dx;
                    position_y += dy;
                }

                if(!MouseDown && time >= MouseDownAtTime)
                {
                    if (ButtonToClick == null)
                    {
                        return true;
                    }
                    else
                    {
                        if(OnlyClickWithin.HasValue && !OnlyClickWithin.Value.Contains(Location))
                        {
                            return false; 
                        }
                        ButtonToClick.Down(true); // click button without polling
                        MouseDown = true;
                    }
                }

                if(time > endtime + PollingPeriod)
                {
                    // there's no more datapoints
                    break;
                }
            }

            if (MouseDown) ButtonToClick.Up(); // polled
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using FutScriptFunctions.Win32API;
using FutScriptFunctions.Numbers;

namespace FutScriptFunctions.Mouse
{
    public class JaggedMouseMover : MouseActionPerformer
    {
        public override void MoveTo(int x, int y, double speed = SPEED_DEFAULT)
        {
            const int MEAN = 100;
            const int DEV = 50;

            int loops_between_poll = Math.Max(1, (int)(speed * (PollingPeriod / 1000.0)));

            int maxrandy_x = NormalDistributor.NormalDistribution(MEAN, DEV);
            int maxrandy_y = NormalDistributor.NormalDistribution(MEAN, DEV);
            Point destination = User32.AbsoluteToRelativePoint(x, y);

            while (destination != Cursor.Position)
            {
                int xdif = destination.X - Cursor.Position.X;
                int ydif = destination.Y-Cursor.Position.Y;

                // +1 prevents div/0
                int xdif_magnitude = Math.Abs(xdif) + 1;
                int ydif_magnitude = Math.Abs(ydif) + 1;

                int x_addition = Math.Sign(xdif);
                int y_addition = Math.Sign(ydif);

                int relative_x = 0;
                int relative_y = 0;

                for (int i = 0; i < loops_between_poll && 
                    (xdif != relative_x || ydif != relative_y); i++)
                {
                    if (RandomGenerator.NextInt(0, maxrandy_x) <= (xdif_magnitude * MEAN) / ydif_magnitude)
                    {
                        relative_x += x_addition;
                    }
                    if (RandomGenerator.NextInt(0, maxrandy_y) <= (ydif_magnitude * MEAN) / xdif_magnitude)
                    {
                        relative_y += y_addition;
                    }
                    xdif_magnitude = Math.Abs(xdif + relative_x) + 1;
                    ydif_magnitude = Math.Abs(ydif + relative_y) + 1;
                }

                WaitForNextPoll();
                JumpPosition(relative_x, relative_y);
            }
        }

        public override void MoveFrom(int dx, int dy, double speed = SPEED_DEFAULT)
        {
            Point current_location = Location;
            MoveTo(current_location.X + dx, current_location.Y + dy, speed);
        }
    }
}

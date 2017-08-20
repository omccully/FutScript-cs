using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace FutScript
{
    public partial class EditorForm
    {
        bool StopFindCoordsHotKeyPressed(KeyEventArgs kea)
        {
            // stop timer to track mouse cursor
            CoordinateUpdateTimer.Stop();
            return true;
        }

        private void FindBut_Click(object sender, EventArgs e)
        {
            // start timer to track mouse cursor
            CoordinateUpdateTimer.Start();
        }

        private void StopBut_Click(object sender, EventArgs e)
        {
            // stop timer to track mouse cursor
            CoordinateUpdateTimer.Stop();
        }

        private void CoordinateUpdateTimer_Tick(object sender, EventArgs e)
        {
            // interval occurred, find coords
            Point p = GetReferencePoint();
            int x = MouseMover.Location.X - p.X;
            int y = MouseMover.Location.Y - p.Y;
            XFind.Text = x.ToString();
            YFind.Text = y.ToString();
        }
    }
}

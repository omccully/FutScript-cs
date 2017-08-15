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
            StopBut.PerformClick();
            return true;
        }

        private void FindBut_Click(object sender, EventArgs e)
        {
            if (UpdateCoordsThread == null)
            {
                UpdateCoordsThread = new Thread(UpdateLoopThread);
                UpdateCoordsThread.Start();
            }
        }

        private void StopBut_Click(object sender, EventArgs e)
        {
            if (UpdateCoordsThread != null)
            {
                UpdateCoordsThread.Abort();
                UpdateCoordsThread = null;
            }
        }

        void UpdateLoopThread()
        {
            Point p = GetReferencePoint();

            try
            {
                while (true)
                {
                    int x = MouseMover.Location.X - p.X;
                    int y = MouseMover.Location.Y - p.Y;

                    this.Invoke((MethodInvoker)delegate
                    {
                        XFind.Text = x.ToString();
                        YFind.Text = y.ToString();
                    });

                    Thread.Sleep(30);
                }
            }
            catch { }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FutScriptFunctions.Numbers;
using FutScriptFunctions.Mouse;
using FutScriptFunctions.Mouse.Recorded;

namespace FutScript
{
    public partial class MouseRecorderForm : Form
    {
        RecordedMousePaths Paths { get; set; }
        MousePathRecorder Recorder { get; set; }

        public MouseRecorderForm(RecordedMousePaths Paths)
        {
            this.Paths = Paths;
            Recorder = new MousePathRecorder(Paths);
            Recorder.StartRecording();

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Size = new Size(RandomGenerator.NextInt(20, 125),
                RandomGenerator.NextInt(20, 125));
            button1.Location = new Point(
                NormalDistributor.WeightedRandom(0, this.Size.Width - button1.Size.Width - 100),
                NormalDistributor.WeightedRandom(0, this.Size.Height - button1.Size.Height - 100));
        }

        private void MouseRecorderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Recorder.StopRecording();
        }
    }
}

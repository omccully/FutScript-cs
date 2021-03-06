﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using FutScriptFunctions;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using FutScriptFunctions.Script;
using FutScriptFunctions.Numbers;
using FutScriptFunctions.Screen;
using FutScriptFunctions.Mouse;
using FutScriptFunctions.Keyboard;
using FutScriptFunctions.Mouse.Recorded;
using FutScriptFunctions.Mouse.LocationSetBehaviors;

namespace FutScript
{
    public partial class EditorForm : Form
    {
        HotKeyManager HotKeyMngr = new HotKeyManager();

        const string PathsFilePath = "paths.json";

        Script script = new Script();
        MouseActionPerformer MouseMover = new MouseActionPerformer();
        Dictionary<string, MouseActionPerformer> MouseMovers;
        ICursorLocationSetter[] CursorLocationSetters = new ICursorLocationSetter[]
        {
            new MouseEventCursorLocationSetter(),
            new SetCursorPosCursorLocationSetter(),
            new CursorPositionCursorLocationSetter(),
        };

        Color InitialButtonColor { get; set; }
        Color InitialTextBoxColor { get; set; }

        //Dictionary<string, ICursorLocationSetter> MouseFunctions = 
           // new Dictionary<string, ICursorLocationSetter>();

        Thread ParentThread { get; set; }

        RecordedMousePathsFile MousePathsFile = new RecordedMousePathsFile(PathsFilePath);

        public EditorForm(UserSettings settings, string initial_file_path = null)
        {
            InitializeComponent();

            ColorPic.BackColor = Color.Black;
            SavedLabel.ForeColor = Color.Green;
            KeyCodeCombo.SelectedIndex = 0;
            InitialButtonColor = StartScriptButton.BackColor;
            InitialTextBoxColor = MouseSpeedTextBox.BackColor;
            ParentThread = Thread.CurrentThread;

            script.ScriptStopped += Script_ScriptStopped;
            script.LoopFinished += Script_LoopFinished;

            HotKeyMngr.CreateHotKey(Keys.F6, StopFindCoordsHotKeyPressed);
            HotKeyMngr.CreateHotKey(Keys.F3, ToggleScriptHotkeyPressed);

            JaggedMouseMover Jagged = new JaggedMouseMover();
            MouseMovers = new Dictionary<string, MouseActionPerformer>()
            {
                { "Linear", new LinearMouseMover() },
                { "Recorded", new RecordedMouseMover(MousePathsFile.Paths, Jagged) },
                { "Teleport", new MouseActionPerformer() },
                { "Jagged" , Jagged }
            };

            // Initialize MouseMoverComboBox
            foreach (string mover_name in MouseMovers.Keys)
            {
                MouseMoverComboBox.Items.Add(mover_name);
            }
            MouseMoverComboBox.SelectedIndex = 0;

            // Initialize MouseFunctionComboBox
            MouseFunctionComboBox.DataSource = CursorLocationSetters;
            MouseFunctionComboBox.SelectedIndex = 0;

            InitializeMenuStrip();

            // these are initialized here, so the script properties get set
            // by the TextChanged events
            XBox.Text = "0";
            YBox.Text = "0";
            MousePollTextBox.Text = "125";
            KeyboardPollTextBox.Text = "125";
            MouseSpeedTextBox.Text = "388~48";
            TypingSpeedTextBox.Text = "5.2~2.3";
            StandardWaitTextBox.Text = "49~11";
            
            LoadOptions(settings);
        }

        void ThreadSafeInvoke(MethodInvoker mi)
        {
            if(Thread.CurrentThread == ParentThread)
            {
                mi();
            } 
            else 
            {
                try
                {
                    this.Invoke(mi);
                }
                catch(ObjectDisposedException)
                {
                    // this occurs if form closes while script is running
                    // script exits and causes ScriptStopped event
                }
            }
        }

        private void GotoBut_Click(object sender, EventArgs e)
        {
            MouseMover.Location = GetReferencePoint();
        }

        Point GetReferencePoint()
        {
            try
            {
                Point p = new Point(Int32.Parse(XBox.Text), Int32.Parse(YBox.Text));
                return p;
            }
            catch
            {
                MessageBox.Show("Error parsing reference point. Using point (0,0).");
                return new Point(0, 0);
            }
        }

        ScreenCapturer screen_capturer = new ScreenCapturer();

        private void ColorBut_Click(object sender, EventArgs e)
        {
            try
            {
                // get the color at the Find point
                Point ReferencePoint = GetReferencePoint();
                Point p = new Point(ReferencePoint.X + Int32.Parse(XFind.Text),
                    ReferencePoint.Y + Int32.Parse(YFind.Text));

                Color pix = screen_capturer.GetColorOfPx(p);

                // display color to the user
                ColorBox.Text = pix.Name.Substring(2);
                ColorPic.BackColor = pix;
            }
            catch (FormatException)
            {
                MessageBox.Show("Enter in pixel coordinates in the boxes above first.");
            }
        } 

        private void MouseSpeedTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                script.MouseSpeed = NumberGenerator.Parse(MouseSpeedTextBox.Text);
                MouseSpeedTextBox.BackColor = InitialTextBoxColor;
            }
            catch
            {
                MouseSpeedTextBox.BackColor = Color.Red;
            }
        }

        private void TypingSpeedTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                script.TypingSpeed = NumberGenerator.Parse(TypingSpeedTextBox.Text);
                TypingSpeedTextBox.BackColor = InitialTextBoxColor;
            }
            catch
            {
                TypingSpeedTextBox.BackColor = Color.Red;
            }
        }

        private void MousePollTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (MouseActionPerformer mouse_mover in MouseMovers.Values)
                {
                    mouse_mover.PollingRate = Int32.Parse(MousePollTextBox.Text);
                }
                MousePollTextBox.BackColor = InitialTextBoxColor;
            }
            catch
            {
                MousePollTextBox.BackColor = Color.Red;
            }
        }

        private void KeyboardPollTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                KeyboardActionPerformer.PollingRate = Int32.Parse(KeyboardPollTextBox.Text);
                KeyboardPollTextBox.BackColor = InitialTextBoxColor;
            }
            catch
            {
                KeyboardPollTextBox.BackColor = Color.Red;
            }
        }

        private void StandardWaitTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                script.StandardWait = NumberGenerator.Parse(StandardWaitTextBox.Text);
                StandardWaitTextBox.BackColor = InitialTextBoxColor;
            }
            catch
            {
                StandardWaitTextBox.BackColor = Color.Red;
            }
        }

        private void MouseMoverComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            script.MouseMover = MouseMovers[MouseMoverComboBox.Text];
        }

        private void MouseFunctionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // set all mouse movers' movement functions to the new selected one.
            if (MouseFunctionComboBox.SelectedValue != null)
            {
                ICursorLocationSetter move_func =
                    (ICursorLocationSetter)MouseFunctionComboBox.SelectedValue;

                // apply this mouse_func to all of the mouse movers
                foreach (MouseActionPerformer mouse_mover in MouseMovers.Values)
                {
                    mouse_mover.CursorLocationSetter = move_func;
                }
            }
            else
            { 
                MessageBox.Show("Invalid Mouse Function selection");
            }
        }

        private void RecordMouseMovementsButton_Click(object sender, EventArgs e)
        {
            // MouseRecorderForm modifies MousePathsFile.Paths
            MouseRecorderForm mouse_recorder_form = new MouseRecorderForm(MousePathsFile.Paths);
            mouse_recorder_form.ShowDialog();

            // save paths
            MousePathsFile.Save();
        }
    }
}

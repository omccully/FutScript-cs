using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using FutScriptFunctions;
using FutScriptFunctions.Script;

namespace FutScript
{
    public partial class EditorForm
    {
        bool _ScriptRunning;
        bool ScriptRunning
        {
            get
            {
                return _ScriptRunning;
            }
            set
            {
                _ScriptRunning = value;
                if (_ScriptRunning)
                {
                    StartScriptButton.Text = "Stop Script (F3)";
                    StartScriptButton.BackColor = Color.Red;
                }
                else
                {
                    StartScriptButton.Text = "Start Script (F3)";
                    StartScriptButton.BackColor = InitialButtonColor;
                }
            }
        }

        void ToggleScript()
        {
            if (ScriptRunning)
            {
                script.Stop();

                // ScriptStopped event should do this anyway: 
                ScriptRunning = false; // change button text
            }
            else
            {
                ScriptRunning = true;
                script.RunLoop(ScriptBox.Text, false); // non-blocking
            }
        }

        bool ToggleScriptHotkeyPressed(KeyEventArgs kea)
        {
            ToggleScript();
            return true;
        }

        private void StartScriptButton_Click(object sender, EventArgs e)
        {
            ToggleScript();
        }

        private void Script_ScriptStopped(object sender, ScriptStoppedEventArgs e)
        {
            ThreadSafeInvoke(delegate () { ScriptRunning = false; });

            if (e.ExceptionThrown != null &&
                !(e.ExceptionThrown is ThreadAbortException))
            {
                MessageBox.Show("Script stopped with message:" + Environment.NewLine +
                    e.ExceptionThrown.ToString());
            }
        }

        private void Script_LoopFinished(object sender, LoopFinishedEventArgs e)
        {
            ThreadSafeInvoke(delegate ()
            {
                ScriptStatusLabel.Text = $"Loop #{e.LoopCount}, {Math.Round(e.TimeElapsed.TotalSeconds, 3)} seconds";
            });
        }
    }
}

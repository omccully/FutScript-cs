using System.Windows.Forms;
using System.IO;
using System;
using FutScriptFunctions.Mouse;
using System.Diagnostics;
using FutScriptFunctions.Mouse.LocationSetBehaviors;
using System.Collections.Generic;
using System.Linq;

namespace FutScript
{
    public partial class EditorForm
    {
        UserSettings Settings;

        Control[] _ControlsToSave;
        Control[] ControlsToSave
        {
            get
            {
                return _ControlsToSave ?? (_ControlsToSave = new Control[]
                {
                    XBox, YBox, MousePollTextBox,
                    KeyboardPollTextBox, MouseSpeedTextBox,
                    TypingSpeedTextBox, StandardWaitTextBox,
                    MouseMoverComboBox, MouseFunctionComboBox
                });
            }
        }

        Dictionary<string, ICursorLocationSetter> _CursorLocationSettersDict;
        Dictionary<string, ICursorLocationSetter> CursorLocationSettersDict
        {
            get
            {
                return _CursorLocationSettersDict ?? (_CursorLocationSettersDict = CursorLocationSetters.ToDictionary(
                    cls => cls.ToString(),
                    cls => cls));
            }
        }

        int IndexOfCursorLocationSetter(string cls_str, int default_index=-1)
        {
            if (CursorLocationSettersDict.ContainsKey(cls_str))
            {
                ICursorLocationSetter cls = CursorLocationSettersDict[cls_str];
                return Array.IndexOf(CursorLocationSetters, cls);
            }
            else
            {
                return default_index;
            }
        }

        void LoadSettingForControl(UserSettings settings, Control control)
        {
            string saved_val = Settings.GetString(control.Name, null);
            if (saved_val == null) return;

            if (control == MouseFunctionComboBox)
            {
                MouseFunctionComboBox.SelectedIndex =
                    IndexOfCursorLocationSetter(saved_val, 0);
            }
            else if (control == MouseMoverComboBox)
            {
                if (MouseMovers.ContainsKey(saved_val))
                {
                    MouseMover = MouseMovers[saved_val];
                    MouseMoverComboBox.Text = saved_val;
                }
            }
            else
            {
                control.Text = saved_val;
            }
        }

        void LoadOptions(UserSettings settings)
        {
            Settings = settings;

            foreach (Control control in ControlsToSave)
            {
                try
                {
                    LoadSettingForControl(settings, control);
                }
                catch
                {
                    MessageBox.Show("Could not restore setting for " + control.Name);
                }
            }
        }

        void SaveOptions()
        {
            foreach(Control control in ControlsToSave)
            {
                Settings.SetValue(control.Name, control.Text);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                SaveOptions();
                script.Stop();
            }
            catch { }
        }
    }
}

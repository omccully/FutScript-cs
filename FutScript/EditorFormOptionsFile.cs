using System.Windows.Forms;
using System.IO;
using System;
using FutScriptFunctions.Mouse;
using System.Diagnostics;
using FutScriptFunctions.Mouse.LocationSetBehaviors;

namespace FutScript
{
    public partial class EditorForm
    {
        void LoadOptions()
        {
            if (!File.Exists(OptionsFilePath))
            {
                return;
            }
            foreach (string Line in File.ReadAllLines(OptionsFilePath))
            {
                try
                {
                    string Value = Line.Substring(Line.IndexOf('=') + 1);
                    switch (Line.Substring(0, Line.IndexOf('=')))
                    {
                        case "xref":
                            XBox.Text = Value;
                            break;
                        case "yref":
                            YBox.Text = Value;
                            break;
                        case "mpoll":
                            MousePollTextBox.Text = Value;
                            break;
                        case "kpoll":
                            KeyboardPollTextBox.Text = Value;
                            break;
                        case "mspeed":
                            MouseSpeedTextBox.Text = Value;
                            break;
                        case "tspeed":
                            TypingSpeedTextBox.Text = Value;
                            break;
                        case "wait":
                            StandardWaitTextBox.Text = Value;
                            break;
                        case "mousemover":
                            if(MouseMovers.ContainsKey(Value))
                            {
                                MouseMover = MouseMovers[Value];
                                MouseMoverComboBox.Text = Value;
                            }
                            break;
                        case "mousefunc":
                            try
                            {
                                int index;
                                if(CursorLocationSettersDict.ContainsKey(Value))
                                {
                                    ICursorLocationSetter cls = CursorLocationSettersDict[Value];
                                    index = Array.IndexOf(CursorLocationSetters, cls);
                                }
                                else
                                {
                                    index = 0;
                                }
                                
                                // if move_func not found in enum somehow, use 0
                                MouseFunctionComboBox.SelectedIndex = index;
                            }
                            catch(ArgumentException) // Enum.Parse fails
                            {
                                MessageBox.Show("The options file was corrupted. The mouse func option failed to load.");
                            }

                            break;
                       
                    }
                }
                catch { }
            }
        }

        void SaveOptions()
        {
            string[] Lines = new string[]
                {
                    "xref=" + XBox.Text,
                    "yref=" + YBox.Text,
                    "mpoll=" + MousePollTextBox.Text,
                    "kpoll=" + KeyboardPollTextBox.Text,
                    "mspeed=" + MouseSpeedTextBox.Text,
                    "tspeed=" + TypingSpeedTextBox.Text,
                    "wait=" + StandardWaitTextBox.Text,
                    "mousemover=" + MouseMoverComboBox.Text,
                    "mousefunc=" + MouseFunctionComboBox.Text
                };
            string OptionsDir = Path.GetDirectoryName(Path.GetFullPath(OptionsFilePath));
            if (!Directory.Exists(OptionsDir))
            {
                Directory.CreateDirectory(OptionsDir);
            }
            File.WriteAllLines(OptionsFilePath, Lines);
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

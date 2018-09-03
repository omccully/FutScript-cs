using System;
using System.Windows.Forms;
using FutScriptFunctions;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FutScriptFunctions.Script;

namespace FutScript
{
    public partial class EditorForm
    {
        /// <summary>
        /// Get a built-in FutScript function's signature from a function name
        /// </summary>
        /// <param name="function_name"></param>
        /// <returns></returns>
        string GetFunctionSignature(string function_name)
        {
            MethodInfo method_info = Script.GetBuiltInFunction(function_name);

            FunctionParserAttribute function_info =
                    (FunctionParserAttribute)method_info.GetCustomAttribute(typeof(FunctionParserAttribute));

            return function_info.Signature;
        }

        /// <summary>
        /// If <paramref name="header"/> does not exist, it gets created.
        /// Returns the ToolStripMenuItem object with the name <paramref name="header"/>
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        ToolStripMenuItem GetOrAddDropDownMenu(MenuStrip parent, string header)
        {
            ToolStripItemCollection items = parent.Items;

            if (!items.ContainsKey(header))
            {
                ToolStripMenuItem ts_menu_item = new ToolStripMenuItem(header);
                ts_menu_item.Name = header;
                items.Add(ts_menu_item);
            }

            return (ToolStripMenuItem)items.Find(header, false)[0];
        }

        /// <summary>
        /// Adds menu strip sections based on FunctionParserAttribute.Category. 
        /// Adds menu strip items within the sections based on FunctionParserAttribute.Signature. 
        /// </summary>
        void InitializeMenuStrip()
        {
            foreach (MethodInfo mi in Script.GetBuiltInFunctions())
            {
                FunctionParserAttribute function_info =
                    (FunctionParserAttribute)mi.GetCustomAttribute(typeof(FunctionParserAttribute));

                ToolStripMenuItem dropdown_menu = GetOrAddDropDownMenu(menuStrip1, function_info.Category);
                ToolStripMenuItem func_sig_mene_item = new ToolStripMenuItem(function_info.Signature);
                func_sig_mene_item.Click += QuickCode;
                dropdown_menu.DropDownItems.Add(func_sig_mene_item);
            }
        }

        /// <summary>
        /// Event that generates code from <paramref name="sender"/>.Text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuickCode(object sender, EventArgs e)
        {
            ToolStripMenuItem ts = (ToolStripMenuItem)sender;
            try
            {
                string function = MapDefaultValues(ts.Text);
                InsertFunctionAtSelection(ScriptBox, function);
            }
            catch (Exception err)
            {
                MessageBox.Show("Error " + err.ToString());
            }
        }

        /// <summary>
        /// Selects text between parenthesis in a text box.
        /// </summary>
        /// <param name="text_box"></param>
        /// <param name="start_index"></param>
        void SelectTextInParenthesis(TextBoxBase text_box, int start_index = 0)
        {
            int left_index = text_box.Text.IndexOf('(', start_index);
            if (left_index == -1) throw new FormatException("Left parenthesis not found. ");

            int right_index = text_box.Text.IndexOf(')', start_index);
            if (right_index == -1) throw new FormatException("Right parenthesis not found. ");

            text_box.SelectionStart = left_index + 1;
            text_box.SelectionLength = right_index - left_index - 1;
        }

        /// <summary>
        /// Inserts <paramref name="new_line"/> on the line after the current
        /// keyboard cursor position within the text box.
        /// </summary>
        /// <param name="text_box"></param>
        /// <param name="new_line"></param>
        /// <returns></returns>
        int InsertLineAfterSelection(TextBoxBase text_box, string new_line)
        {
            int selection_index = text_box.SelectionStart;
            int new_line_index;

            if (selection_index == -1)
            {
                new_line_index = text_box.Text.Length + 1;
                text_box.Text += new_line + Environment.NewLine;
                return new_line_index;
            }

            new_line_index = text_box.Text.IndexOf('\n', selection_index) + 1;
            text_box.Text = text_box.Text.Insert(new_line_index, new_line + Environment.NewLine);
            return new_line_index;
        }

        /// <summary>
        /// Inserts a function and selects the text within the parenthesis
        /// </summary>
        /// <param name="text_box"></param>
        /// <param name="function"></param>
        void InsertFunctionAtSelection(TextBoxBase text_box, string function)
        {
            int new_line_index = InsertLineAfterSelection(text_box, function);
            SelectTextInParenthesis(text_box, new_line_index);
        }

        /// <summary>
        /// Replaces parameter names with default argument values.
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        string MapDefaultValues(string function)
        {
            function = function.Replace("[","").Replace("]","");

            int x = Int32.Parse(XFind.Text);
            int y = Int32.Parse(YFind.Text);

            Dictionary<string, string> default_values = new Dictionary<string, string>();
            default_values.Add("time_ms", "40~15");
            default_values.Add("timeout_ms", "10000~3000");
            default_values.Add("color_rule", ColorBox.Text + "~10");
            default_values.Add("key_code", KeyCodeCombo.Text.Substring(0, 4));
            default_values.Add("x", x.ToString());
            default_values.Add("y", y.ToString());

            Match m = new Regex(@"^([A-Za-z0-9]+)\((.+)\)$").Match(function);

            if (m.Success)
            {
                // the function signature matches the expected format
                string[] parameters = m.Groups[2].Value.Split(',');
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (default_values.ContainsKey(parameters[i]))
                    {
                        parameters[i] = default_values[parameters[i]];
                    }
                }

                function = m.Groups[1].Value + '(' + String.Join(",", parameters) + ')';
            }
            else
            {
                // the function signature doesn't match the expected format
                // so, just do a simple string replace
                foreach (string key in default_values.Keys)
                {
                    function = function.Replace(key, default_values[key]);
                }
            }

            return function;
        }

        /// <summary>
        /// Insert function signature with mapped default values from 
        /// its name (<paramref name="function_name"/>).
        /// </summary>
        /// <param name="function_name"></param>
        void InsertFunctionFromName(string function_name)
        {
            string signature = GetFunctionSignature(function_name);
            string with_defaults = MapDefaultValues(signature);
            InsertFunctionAtSelection(ScriptBox, with_defaults);
        }

        private void movebut_Click(object sender, EventArgs e)
        {
            InsertFunctionFromName("move");
        }

        private void waitforpxbut_Click(object sender, EventArgs e)
        {
            InsertFunctionFromName("waitforpx");
        }

        private void ifbut_Click(object sender, EventArgs e)
        {
            InsertFunctionFromName("ifcolor");
        }
    }
}

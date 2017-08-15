using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace FutScript
{
    public partial class EditorForm
    {
        string _OpenedFile = null;
        string OpenedFile
        {
            get
            {
                return _OpenedFile;
            }
            set
            {
                _OpenedFile = value;
                saveToolStripMenuItem.Text = (value == null ? "Save" :
                    "Save (" + Path.GetFileName(value) + ")");
            }
        }

        bool _Saved = true;
        bool Saved
        {
            get
            {
                return _Saved;
            }
            set
            {
                _Saved = value;
                SavedLabel.ForeColor = (value ? Color.Green : Color.Red);
                SavedLabel.Text = (value ? "Saved" : "Not saved");
            }
        }

        private string SelectDialog(string initialDirectory)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "fut files (*.fut)|*.fut|txt files (*.txt)|*.txt|All files (*.*)|*.*";
            dialog.InitialDirectory = initialDirectory;
            dialog.Title = "Select file";
            return (dialog.ShowDialog() == DialogResult.OK) ? dialog.FileName : null;
        }
        private string SaveDialog(string initialDirectory)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "fut files (*.fut)|*.fut|txt files (*.txt)|*.txt|All files (*.*)|*.*";
            dialog.InitialDirectory = initialDirectory;
            dialog.Title = "Save file";
            return (dialog.ShowDialog() == DialogResult.OK) ? dialog.FileName : null; ;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Saved)
            {
                DialogResult dlg = MessageBox.Show("Do you want to save changes?", "FutScript Editor",
                    MessageBoxButtons.YesNoCancel);
                if (dlg == DialogResult.Yes)
                {
                    if (OpenedFile == null)
                    {
                        string s = SaveDialog(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
                        if (s != null)
                            SaveFile(s);
                        else
                            return;
                    }
                    else
                    {
                        SaveFile(OpenedFile);
                    }
                }
                else if (dlg != DialogResult.No)
                {
                    return;
                }
            }
            ScriptBox.Text = "";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile(SelectDialog(Environment.GetFolderPath(Environment.SpecialFolder.Personal)));
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(OpenedFile == null ?
                SaveDialog(Environment.GetFolderPath(Environment.SpecialFolder.Personal)) : OpenedFile);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(SaveDialog(Environment.GetFolderPath(Environment.SpecialFolder.Personal)));
        }

        private void OpenFile(string FilePath)
        {
            if (FilePath != null)
            {
                ScriptBox.Lines = File.ReadAllLines(FilePath);
                OpenedFile = FilePath;
                Saved = true;
            }
        }

        private void SaveFile(string FilePath)
        {
            if (FilePath != null)
            {
                File.WriteAllLines(FilePath, ScriptBox.Lines);
                OpenedFile = FilePath;
                Saved = true;
            }
        }

        private void ScriptBox_TextChanged(object sender, EventArgs e)
        {
            Saved = false;
        }
    }
}

namespace FutScript
{
    partial class EditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ScriptBox = new System.Windows.Forms.RichTextBox();
            this.XBox = new System.Windows.Forms.TextBox();
            this.YBox = new System.Windows.Forms.TextBox();
            this.GotoBut = new System.Windows.Forms.Button();
            this.FindBut = new System.Windows.Forms.Button();
            this.StopBut = new System.Windows.Forms.Button();
            this.XFind = new System.Windows.Forms.TextBox();
            this.YFind = new System.Windows.Forms.TextBox();
            this.ColorBut = new System.Windows.Forms.Button();
            this.ColorBox = new System.Windows.Forms.TextBox();
            this.ColorPic = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.KeyCodeCombo = new System.Windows.Forms.ComboBox();
            this.SavedLabel = new System.Windows.Forms.Label();
            this.movebut = new System.Windows.Forms.Button();
            this.waitforpxbut = new System.Windows.Forms.Button();
            this.ifbut = new System.Windows.Forms.Button();
            this.StartScriptButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.MouseSpeedTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.StandardWaitTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.MouseMoverComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.MouseFunctionComboBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.MousePollTextBox = new System.Windows.Forms.TextBox();
            this.KeyboardPollTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.RecordMouseMovementsButton = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.TypingSpeedTextBox = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.CoordinateUpdateTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ColorPic)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ScriptBox
            // 
            this.ScriptBox.Location = new System.Drawing.Point(0, 24);
            this.ScriptBox.Name = "ScriptBox";
            this.ScriptBox.Size = new System.Drawing.Size(352, 448);
            this.ScriptBox.TabIndex = 0;
            this.ScriptBox.Text = "move(200,200)\nwait(500)\nmove(800,700)\nwait(100)";
            this.ScriptBox.TextChanged += new System.EventHandler(this.ScriptBox_TextChanged);
            // 
            // XBox
            // 
            this.XBox.Location = new System.Drawing.Point(374, 52);
            this.XBox.Name = "XBox";
            this.XBox.Size = new System.Drawing.Size(47, 20);
            this.XBox.TabIndex = 1;
            this.XBox.Text = "0";
            // 
            // YBox
            // 
            this.YBox.Location = new System.Drawing.Point(427, 52);
            this.YBox.Name = "YBox";
            this.YBox.Size = new System.Drawing.Size(47, 20);
            this.YBox.TabIndex = 2;
            this.YBox.Text = "0";
            // 
            // GotoBut
            // 
            this.GotoBut.Location = new System.Drawing.Point(480, 52);
            this.GotoBut.Name = "GotoBut";
            this.GotoBut.Size = new System.Drawing.Size(53, 20);
            this.GotoBut.TabIndex = 50;
            this.GotoBut.Text = "Go to";
            this.GotoBut.UseVisualStyleBackColor = true;
            this.GotoBut.Click += new System.EventHandler(this.GotoBut_Click);
            // 
            // FindBut
            // 
            this.FindBut.Location = new System.Drawing.Point(374, 94);
            this.FindBut.Name = "FindBut";
            this.FindBut.Size = new System.Drawing.Size(37, 20);
            this.FindBut.TabIndex = 51;
            this.FindBut.Text = "Find";
            this.FindBut.UseVisualStyleBackColor = true;
            this.FindBut.Click += new System.EventHandler(this.FindBut_Click);
            // 
            // StopBut
            // 
            this.StopBut.Location = new System.Drawing.Point(417, 94);
            this.StopBut.Name = "StopBut";
            this.StopBut.Size = new System.Drawing.Size(60, 20);
            this.StopBut.TabIndex = 52;
            this.StopBut.Text = "Stop (F6)";
            this.StopBut.UseVisualStyleBackColor = true;
            this.StopBut.Click += new System.EventHandler(this.StopBut_Click);
            // 
            // XFind
            // 
            this.XFind.Location = new System.Drawing.Point(374, 120);
            this.XFind.Name = "XFind";
            this.XFind.Size = new System.Drawing.Size(47, 20);
            this.XFind.TabIndex = 3;
            this.XFind.Text = "0";
            // 
            // YFind
            // 
            this.YFind.Location = new System.Drawing.Point(427, 120);
            this.YFind.Name = "YFind";
            this.YFind.Size = new System.Drawing.Size(47, 20);
            this.YFind.TabIndex = 4;
            this.YFind.Text = "0";
            // 
            // ColorBut
            // 
            this.ColorBut.Location = new System.Drawing.Point(480, 145);
            this.ColorBut.Name = "ColorBut";
            this.ColorBut.Size = new System.Drawing.Size(69, 20);
            this.ColorBut.TabIndex = 54;
            this.ColorBut.Text = "Get color";
            this.ColorBut.UseVisualStyleBackColor = true;
            this.ColorBut.Click += new System.EventHandler(this.ColorBut_Click);
            // 
            // ColorBox
            // 
            this.ColorBox.Location = new System.Drawing.Point(400, 146);
            this.ColorBox.Name = "ColorBox";
            this.ColorBox.Size = new System.Drawing.Size(74, 20);
            this.ColorBox.TabIndex = 5;
            this.ColorBox.Text = "000000";
            // 
            // ColorPic
            // 
            this.ColorPic.Location = new System.Drawing.Point(375, 149);
            this.ColorPic.Name = "ColorPic";
            this.ColorPic.Size = new System.Drawing.Size(20, 20);
            this.ColorPic.TabIndex = 7;
            this.ColorPic.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(557, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "Status";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // KeyCodeCombo
            // 
            this.KeyCodeCombo.FormattingEnabled = true;
            this.KeyCodeCombo.Items.AddRange(new object[] {
            "0x25 LEFT",
            "0x26 UP",
            "0x27 RIGHT",
            "0x28 DOWN",
            "0xA0 Left SHIFT",
            "0xA2 Left CTRL"});
            this.KeyCodeCombo.Location = new System.Drawing.Point(373, 197);
            this.KeyCodeCombo.Name = "KeyCodeCombo";
            this.KeyCodeCombo.Size = new System.Drawing.Size(104, 21);
            this.KeyCodeCombo.TabIndex = 9;
            // 
            // SavedLabel
            // 
            this.SavedLabel.AutoSize = true;
            this.SavedLabel.Location = new System.Drawing.Point(390, 5);
            this.SavedLabel.Name = "SavedLabel";
            this.SavedLabel.Size = new System.Drawing.Size(38, 13);
            this.SavedLabel.TabIndex = 10;
            this.SavedLabel.Text = "Saved";
            // 
            // movebut
            // 
            this.movebut.Location = new System.Drawing.Point(480, 120);
            this.movebut.Name = "movebut";
            this.movebut.Size = new System.Drawing.Size(69, 20);
            this.movebut.TabIndex = 53;
            this.movebut.Text = "Move";
            this.movebut.UseVisualStyleBackColor = true;
            this.movebut.Click += new System.EventHandler(this.movebut_Click);
            // 
            // waitforpxbut
            // 
            this.waitforpxbut.Location = new System.Drawing.Point(480, 171);
            this.waitforpxbut.Name = "waitforpxbut";
            this.waitforpxbut.Size = new System.Drawing.Size(69, 20);
            this.waitforpxbut.TabIndex = 56;
            this.waitforpxbut.Text = "waitforpx()";
            this.waitforpxbut.UseVisualStyleBackColor = true;
            this.waitforpxbut.Click += new System.EventHandler(this.waitforpxbut_Click);
            // 
            // ifbut
            // 
            this.ifbut.Location = new System.Drawing.Point(417, 171);
            this.ifbut.Name = "ifbut";
            this.ifbut.Size = new System.Drawing.Size(57, 20);
            this.ifbut.TabIndex = 55;
            this.ifbut.Text = "ifcolor()";
            this.ifbut.UseVisualStyleBackColor = true;
            this.ifbut.Click += new System.EventHandler(this.ifbut_Click);
            // 
            // StartScriptButton
            // 
            this.StartScriptButton.Location = new System.Drawing.Point(373, 409);
            this.StartScriptButton.Name = "StartScriptButton";
            this.StartScriptButton.Size = new System.Drawing.Size(170, 63);
            this.StartScriptButton.TabIndex = 57;
            this.StartScriptButton.Text = "Start Script (F3)";
            this.StartScriptButton.UseVisualStyleBackColor = true;
            this.StartScriptButton.Click += new System.EventHandler(this.StartScriptButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(370, 297);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 58;
            this.label1.Text = "Mouse speed:";
            // 
            // MouseSpeedTextBox
            // 
            this.MouseSpeedTextBox.Location = new System.Drawing.Point(456, 294);
            this.MouseSpeedTextBox.Name = "MouseSpeedTextBox";
            this.MouseSpeedTextBox.Size = new System.Drawing.Size(87, 20);
            this.MouseSpeedTextBox.TabIndex = 59;
            this.MouseSpeedTextBox.TextChanged += new System.EventHandler(this.MouseSpeedTextBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(370, 341);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 60;
            this.label2.Text = "Wait (ms):";
            // 
            // StandardWaitTextBox
            // 
            this.StandardWaitTextBox.Location = new System.Drawing.Point(456, 338);
            this.StandardWaitTextBox.Name = "StandardWaitTextBox";
            this.StandardWaitTextBox.Size = new System.Drawing.Size(87, 20);
            this.StandardWaitTextBox.TabIndex = 61;
            this.StandardWaitTextBox.TextChanged += new System.EventHandler(this.StandardWaitTextBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(370, 363);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 62;
            this.label3.Text = "Mouse move:";
            // 
            // MouseMoverComboBox
            // 
            this.MouseMoverComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MouseMoverComboBox.FormattingEnabled = true;
            this.MouseMoverComboBox.Location = new System.Drawing.Point(456, 360);
            this.MouseMoverComboBox.Name = "MouseMoverComboBox";
            this.MouseMoverComboBox.Size = new System.Drawing.Size(87, 21);
            this.MouseMoverComboBox.TabIndex = 63;
            this.MouseMoverComboBox.SelectedIndexChanged += new System.EventHandler(this.MouseMoverComboBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(372, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 64;
            this.label4.Text = "X ref:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(424, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 65;
            this.label5.Text = "Y ref:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(370, 385);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 13);
            this.label6.TabIndex = 62;
            this.label6.Text = "Mouse func:";
            // 
            // MouseFunctionComboBox
            // 
            this.MouseFunctionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MouseFunctionComboBox.FormattingEnabled = true;
            this.MouseFunctionComboBox.Location = new System.Drawing.Point(456, 382);
            this.MouseFunctionComboBox.Name = "MouseFunctionComboBox";
            this.MouseFunctionComboBox.Size = new System.Drawing.Size(87, 21);
            this.MouseFunctionComboBox.TabIndex = 63;
            this.MouseFunctionComboBox.SelectedIndexChanged += new System.EventHandler(this.MouseFunctionComboBox_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(370, 253);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 13);
            this.label7.TabIndex = 58;
            this.label7.Text = "Mouse Poll (Hz)";
            // 
            // MousePollTextBox
            // 
            this.MousePollTextBox.Location = new System.Drawing.Point(456, 250);
            this.MousePollTextBox.Name = "MousePollTextBox";
            this.MousePollTextBox.Size = new System.Drawing.Size(87, 20);
            this.MousePollTextBox.TabIndex = 59;
            this.MousePollTextBox.TextChanged += new System.EventHandler(this.MousePollTextBox_TextChanged);
            // 
            // KeyboardPollTextBox
            // 
            this.KeyboardPollTextBox.Location = new System.Drawing.Point(456, 272);
            this.KeyboardPollTextBox.Name = "KeyboardPollTextBox";
            this.KeyboardPollTextBox.Size = new System.Drawing.Size(87, 20);
            this.KeyboardPollTextBox.TabIndex = 59;
            this.KeyboardPollTextBox.TextChanged += new System.EventHandler(this.KeyboardPollTextBox_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(370, 275);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 13);
            this.label8.TabIndex = 58;
            this.label8.Text = "Keybd Poll (Hz)";
            // 
            // RecordMouseMovementsButton
            // 
            this.RecordMouseMovementsButton.Location = new System.Drawing.Point(375, 224);
            this.RecordMouseMovementsButton.Name = "RecordMouseMovementsButton";
            this.RecordMouseMovementsButton.Size = new System.Drawing.Size(164, 23);
            this.RecordMouseMovementsButton.TabIndex = 67;
            this.RecordMouseMovementsButton.Text = "Record mouse movements";
            this.RecordMouseMovementsButton.UseVisualStyleBackColor = true;
            this.RecordMouseMovementsButton.Click += new System.EventHandler(this.RecordMouseMovementsButton_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(370, 319);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(66, 13);
            this.label9.TabIndex = 58;
            this.label9.Text = "Type speed:";
            // 
            // TypingSpeedTextBox
            // 
            this.TypingSpeedTextBox.Location = new System.Drawing.Point(456, 316);
            this.TypingSpeedTextBox.Name = "TypingSpeedTextBox";
            this.TypingSpeedTextBox.Size = new System.Drawing.Size(87, 20);
            this.TypingSpeedTextBox.TabIndex = 59;
            this.TypingSpeedTextBox.TextChanged += new System.EventHandler(this.TypingSpeedTextBox_TextChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 475);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(557, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 68;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(59, 17);
            this.toolStripStatusLabel1.Text = "Launched";
            // 
            // CoordinateUpdateTimer
            // 
            this.CoordinateUpdateTimer.Interval = 30;
            this.CoordinateUpdateTimer.Tick += new System.EventHandler(this.CoordinateUpdateTimer_Tick);
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 497);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.RecordMouseMovementsButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.MouseFunctionComboBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.MouseMoverComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.StandardWaitTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.KeyboardPollTextBox);
            this.Controls.Add(this.MousePollTextBox);
            this.Controls.Add(this.TypingSpeedTextBox);
            this.Controls.Add(this.MouseSpeedTextBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.StartScriptButton);
            this.Controls.Add(this.movebut);
            this.Controls.Add(this.SavedLabel);
            this.Controls.Add(this.KeyCodeCombo);
            this.Controls.Add(this.ColorPic);
            this.Controls.Add(this.ColorBox);
            this.Controls.Add(this.ifbut);
            this.Controls.Add(this.waitforpxbut);
            this.Controls.Add(this.ColorBut);
            this.Controls.Add(this.YFind);
            this.Controls.Add(this.XFind);
            this.Controls.Add(this.StopBut);
            this.Controls.Add(this.FindBut);
            this.Controls.Add(this.GotoBut);
            this.Controls.Add(this.YBox);
            this.Controls.Add(this.XBox);
            this.Controls.Add(this.ScriptBox);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "EditorForm";
            this.Text = "FutScript Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.ColorPic)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox ScriptBox;
        private System.Windows.Forms.TextBox XBox;
        private System.Windows.Forms.TextBox YBox;
        private System.Windows.Forms.Button GotoBut;
        private System.Windows.Forms.Button FindBut;
        private System.Windows.Forms.Button StopBut;
        private System.Windows.Forms.TextBox XFind;
        private System.Windows.Forms.TextBox YFind;
        private System.Windows.Forms.Button ColorBut;
        private System.Windows.Forms.TextBox ColorBox;
        private System.Windows.Forms.PictureBox ColorPic;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ComboBox KeyCodeCombo;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.Label SavedLabel;
        private System.Windows.Forms.Button movebut;
        private System.Windows.Forms.Button waitforpxbut;
        private System.Windows.Forms.Button ifbut;
        private System.Windows.Forms.Button StartScriptButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox MouseSpeedTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox StandardWaitTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox MouseMoverComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox MouseFunctionComboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox MousePollTextBox;
        private System.Windows.Forms.TextBox KeyboardPollTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button RecordMouseMovementsButton;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox TypingSpeedTextBox;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Timer CoordinateUpdateTimer;
    }
}


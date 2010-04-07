namespace Mommosoft.Capi.Phone {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.comboBoxControllers = new System.Windows.Forms.ComboBox();
            this.buttonListen = new System.Windows.Forms.Button();
            this.listBoxInfo = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.textBoxCalledNumber = new System.Windows.Forms.TextBox();
            this.buttonCall = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.buttonDTFMStart = new System.Windows.Forms.Button();
            this.buttonDTFMStop = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonDTFMSend = new System.Windows.Forms.Button();
            this.textBoxDTFMChars = new System.Windows.Forms.TextBox();
            this.listBoxDTFMInfo = new System.Windows.Forms.ListBox();
            this.textBoxWaveFile = new System.Windows.Forms.TextBox();
            this.openFileDialogWave = new System.Windows.Forms.OpenFileDialog();
            this.buttonBrowseWave = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxTextToSpeak = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonSpeak = new System.Windows.Forms.Button();
            this.buttonAnswer = new System.Windows.Forms.Button();
            this.buttonHangup = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxControllers
            // 
            this.comboBoxControllers.DisplayMember = "ID";
            this.comboBoxControllers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxControllers.FormattingEnabled = true;
            this.comboBoxControllers.Location = new System.Drawing.Point(96, 21);
            this.comboBoxControllers.Name = "comboBoxControllers";
            this.comboBoxControllers.Size = new System.Drawing.Size(121, 21);
            this.comboBoxControllers.TabIndex = 0;
            // 
            // buttonListen
            // 
            this.buttonListen.Location = new System.Drawing.Point(235, 21);
            this.buttonListen.Name = "buttonListen";
            this.buttonListen.Size = new System.Drawing.Size(75, 23);
            this.buttonListen.TabIndex = 1;
            this.buttonListen.Text = "Listen";
            this.buttonListen.UseVisualStyleBackColor = true;
            this.buttonListen.Click += new System.EventHandler(this.buttonListen_Click);
            // 
            // listBoxInfo
            // 
            this.listBoxInfo.FormattingEnabled = true;
            this.listBoxInfo.Location = new System.Drawing.Point(11, 260);
            this.listBoxInfo.Name = "listBoxInfo";
            this.listBoxInfo.Size = new System.Drawing.Size(904, 95);
            this.listBoxInfo.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Controllers:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 358);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(933, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // textBoxCalledNumber
            // 
            this.textBoxCalledNumber.Location = new System.Drawing.Point(99, 189);
            this.textBoxCalledNumber.Name = "textBoxCalledNumber";
            this.textBoxCalledNumber.Size = new System.Drawing.Size(121, 21);
            this.textBoxCalledNumber.TabIndex = 5;
            // 
            // buttonCall
            // 
            this.buttonCall.Location = new System.Drawing.Point(238, 187);
            this.buttonCall.Name = "buttonCall";
            this.buttonCall.Size = new System.Drawing.Size(75, 23);
            this.buttonCall.TabIndex = 6;
            this.buttonCall.Text = "Call";
            this.buttonCall.UseVisualStyleBackColor = true;
            this.buttonCall.Click += new System.EventHandler(this.buttonCall_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 192);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Number:";
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Location = new System.Drawing.Point(319, 187);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(75, 23);
            this.buttonDisconnect.TabIndex = 8;
            this.buttonDisconnect.Text = "Disconnect";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // buttonDTFMStart
            // 
            this.buttonDTFMStart.Location = new System.Drawing.Point(7, 57);
            this.buttonDTFMStart.Name = "buttonDTFMStart";
            this.buttonDTFMStart.Size = new System.Drawing.Size(75, 23);
            this.buttonDTFMStart.TabIndex = 9;
            this.buttonDTFMStart.Text = "Start";
            this.buttonDTFMStart.UseVisualStyleBackColor = true;
            this.buttonDTFMStart.Click += new System.EventHandler(this.buttonDTFMStart_Click);
            // 
            // buttonDTFMStop
            // 
            this.buttonDTFMStop.Location = new System.Drawing.Point(88, 57);
            this.buttonDTFMStop.Name = "buttonDTFMStop";
            this.buttonDTFMStop.Size = new System.Drawing.Size(75, 23);
            this.buttonDTFMStop.TabIndex = 10;
            this.buttonDTFMStop.Text = "Stop";
            this.buttonDTFMStop.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonDTFMSend);
            this.groupBox1.Controls.Add(this.textBoxDTFMChars);
            this.groupBox1.Controls.Add(this.listBoxDTFMInfo);
            this.groupBox1.Controls.Add(this.buttonDTFMStart);
            this.groupBox1.Controls.Add(this.buttonDTFMStop);
            this.groupBox1.Location = new System.Drawing.Point(552, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(369, 237);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DTFM";
            // 
            // buttonDTFMSend
            // 
            this.buttonDTFMSend.Location = new System.Drawing.Point(169, 30);
            this.buttonDTFMSend.Name = "buttonDTFMSend";
            this.buttonDTFMSend.Size = new System.Drawing.Size(75, 23);
            this.buttonDTFMSend.TabIndex = 13;
            this.buttonDTFMSend.Text = "Send";
            this.buttonDTFMSend.UseVisualStyleBackColor = true;
            this.buttonDTFMSend.Click += new System.EventHandler(this.buttonDTFMSend_Click);
            // 
            // textBoxDTFMChars
            // 
            this.textBoxDTFMChars.Location = new System.Drawing.Point(7, 30);
            this.textBoxDTFMChars.Name = "textBoxDTFMChars";
            this.textBoxDTFMChars.Size = new System.Drawing.Size(156, 21);
            this.textBoxDTFMChars.TabIndex = 12;
            // 
            // listBoxDTFMInfo
            // 
            this.listBoxDTFMInfo.FormattingEnabled = true;
            this.listBoxDTFMInfo.Location = new System.Drawing.Point(7, 86);
            this.listBoxDTFMInfo.Name = "listBoxDTFMInfo";
            this.listBoxDTFMInfo.Size = new System.Drawing.Size(356, 147);
            this.listBoxDTFMInfo.TabIndex = 11;
            // 
            // textBoxWaveFile
            // 
            this.textBoxWaveFile.Location = new System.Drawing.Point(96, 50);
            this.textBoxWaveFile.Name = "textBoxWaveFile";
            this.textBoxWaveFile.Size = new System.Drawing.Size(214, 21);
            this.textBoxWaveFile.TabIndex = 12;
            // 
            // openFileDialogWave
            // 
            this.openFileDialogWave.Filter = "Sound files (*.snd)|*.snd|All files (*.*)|*.*";
            // 
            // buttonBrowseWave
            // 
            this.buttonBrowseWave.Location = new System.Drawing.Point(319, 50);
            this.buttonBrowseWave.Name = "buttonBrowseWave";
            this.buttonBrowseWave.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowseWave.TabIndex = 13;
            this.buttonBrowseWave.Text = "Browse";
            this.buttonBrowseWave.UseVisualStyleBackColor = true;
            this.buttonBrowseWave.Click += new System.EventHandler(this.buttonBrowseWave_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Answer sound:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(244, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Sound must be ISDN encoded a-Law, 8000Hz, Mono";
            // 
            // textBoxTextToSpeak
            // 
            this.textBoxTextToSpeak.Location = new System.Drawing.Point(96, 127);
            this.textBoxTextToSpeak.Name = "textBoxTextToSpeak";
            this.textBoxTextToSpeak.Size = new System.Drawing.Size(214, 21);
            this.textBoxTextToSpeak.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 130);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Text to speak:";
            // 
            // buttonSpeak
            // 
            this.buttonSpeak.Location = new System.Drawing.Point(319, 125);
            this.buttonSpeak.Name = "buttonSpeak";
            this.buttonSpeak.Size = new System.Drawing.Size(75, 23);
            this.buttonSpeak.TabIndex = 18;
            this.buttonSpeak.Text = "Speak";
            this.buttonSpeak.UseVisualStyleBackColor = true;
            this.buttonSpeak.Click += new System.EventHandler(this.buttonSpeak_Click);
            // 
            // buttonAnswer
            // 
            this.buttonAnswer.Location = new System.Drawing.Point(316, 21);
            this.buttonAnswer.Name = "buttonAnswer";
            this.buttonAnswer.Size = new System.Drawing.Size(75, 23);
            this.buttonAnswer.TabIndex = 19;
            this.buttonAnswer.Text = "Answer";
            this.buttonAnswer.UseVisualStyleBackColor = true;
            this.buttonAnswer.Click += new System.EventHandler(this.buttonAnswer_Click);
            // 
            // buttonHangup
            // 
            this.buttonHangup.Location = new System.Drawing.Point(397, 21);
            this.buttonHangup.Name = "buttonHangup";
            this.buttonHangup.Size = new System.Drawing.Size(75, 23);
            this.buttonHangup.TabIndex = 20;
            this.buttonHangup.Text = "Hangup";
            this.buttonHangup.UseVisualStyleBackColor = true;
            this.buttonHangup.Click += new System.EventHandler(this.buttonHangup_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 380);
            this.Controls.Add(this.buttonHangup);
            this.Controls.Add(this.buttonAnswer);
            this.Controls.Add(this.buttonSpeak);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxTextToSpeak);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonBrowseWave);
            this.Controls.Add(this.textBoxWaveFile);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonDisconnect);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonCall);
            this.Controls.Add(this.textBoxCalledNumber);
            this.Controls.Add(this.listBoxInfo);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonListen);
            this.Controls.Add(this.comboBoxControllers);
            this.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxControllers;
        private System.Windows.Forms.Button buttonListen;
        private System.Windows.Forms.ListBox listBoxInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.TextBox textBoxCalledNumber;
        private System.Windows.Forms.Button buttonCall;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonDisconnect;
        private System.Windows.Forms.Button buttonDTFMStart;
        private System.Windows.Forms.Button buttonDTFMStop;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listBoxDTFMInfo;
        private System.Windows.Forms.Button buttonDTFMSend;
        private System.Windows.Forms.TextBox textBoxDTFMChars;
        private System.Windows.Forms.TextBox textBoxWaveFile;
        private System.Windows.Forms.OpenFileDialog openFileDialogWave;
        private System.Windows.Forms.Button buttonBrowseWave;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxTextToSpeak;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonSpeak;
        private System.Windows.Forms.Button buttonAnswer;
        private System.Windows.Forms.Button buttonHangup;
    }
}
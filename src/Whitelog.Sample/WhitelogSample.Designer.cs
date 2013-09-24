namespace Whitelog.Sample
{
    partial class WhitelogSample
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
            this.m_groupBuilder = new System.Windows.Forms.GroupBox();
            this.m_bufferTypes = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.m_submitterType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.m_txtPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.m_btnLogBuilder = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.m_loggerTypes = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.m_cboLogTitles = new System.Windows.Forms.ComboBox();
            this.m_btnSimpleLog = new System.Windows.Forms.Button();
            this.m_txtSimpleLogMessage = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.m_txtScopeMessage = new System.Windows.Forms.TextBox();
            this.m_btnOpenScope = new System.Windows.Forms.Button();
            this.m_btnClose = new System.Windows.Forms.Button();
            this.m_groupBuilder.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_groupBuilder
            // 
            this.m_groupBuilder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_groupBuilder.Controls.Add(this.m_bufferTypes);
            this.m_groupBuilder.Controls.Add(this.label4);
            this.m_groupBuilder.Controls.Add(this.m_submitterType);
            this.m_groupBuilder.Controls.Add(this.label3);
            this.m_groupBuilder.Controls.Add(this.m_txtPath);
            this.m_groupBuilder.Controls.Add(this.label2);
            this.m_groupBuilder.Controls.Add(this.m_btnLogBuilder);
            this.m_groupBuilder.Controls.Add(this.label1);
            this.m_groupBuilder.Controls.Add(this.m_loggerTypes);
            this.m_groupBuilder.Location = new System.Drawing.Point(12, 12);
            this.m_groupBuilder.Name = "m_groupBuilder";
            this.m_groupBuilder.Size = new System.Drawing.Size(631, 135);
            this.m_groupBuilder.TabIndex = 0;
            this.m_groupBuilder.TabStop = false;
            this.m_groupBuilder.Text = "Logger Settings Builder";
            // 
            // m_bufferTypes
            // 
            this.m_bufferTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_bufferTypes.FormattingEnabled = true;
            this.m_bufferTypes.Location = new System.Drawing.Point(100, 76);
            this.m_bufferTypes.Name = "m_bufferTypes";
            this.m_bufferTypes.Size = new System.Drawing.Size(315, 21);
            this.m_bufferTypes.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Buffer:";
            // 
            // m_submitterType
            // 
            this.m_submitterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_submitterType.FormattingEnabled = true;
            this.m_submitterType.Location = new System.Drawing.Point(100, 48);
            this.m_submitterType.Name = "m_submitterType";
            this.m_submitterType.Size = new System.Drawing.Size(315, 21);
            this.m_submitterType.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Subbmiter:";
            // 
            // m_txtPath
            // 
            this.m_txtPath.Location = new System.Drawing.Point(100, 103);
            this.m_txtPath.Name = "m_txtPath";
            this.m_txtPath.Size = new System.Drawing.Size(443, 20);
            this.m_txtPath.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Path:";
            // 
            // m_btnLogBuilder
            // 
            this.m_btnLogBuilder.Location = new System.Drawing.Point(549, 101);
            this.m_btnLogBuilder.Name = "m_btnLogBuilder";
            this.m_btnLogBuilder.Size = new System.Drawing.Size(75, 23);
            this.m_btnLogBuilder.TabIndex = 2;
            this.m_btnLogBuilder.Text = "Build";
            this.m_btnLogBuilder.UseVisualStyleBackColor = true;
            this.m_btnLogBuilder.Click += new System.EventHandler(this.m_btnLogBuilder_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Logger Type:";
            // 
            // m_loggerTypes
            // 
            this.m_loggerTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_loggerTypes.FormattingEnabled = true;
            this.m_loggerTypes.Location = new System.Drawing.Point(100, 19);
            this.m_loggerTypes.Name = "m_loggerTypes";
            this.m_loggerTypes.Size = new System.Drawing.Size(315, 21);
            this.m_loggerTypes.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.m_btnClose);
            this.panel1.Controls.Add(this.m_btnOpenScope);
            this.panel1.Controls.Add(this.m_txtScopeMessage);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.m_cboLogTitles);
            this.panel1.Controls.Add(this.m_btnSimpleLog);
            this.panel1.Controls.Add(this.m_txtSimpleLogMessage);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Location = new System.Drawing.Point(13, 154);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(630, 317);
            this.panel1.TabIndex = 1;
            // 
            // m_cboLogTitles
            // 
            this.m_cboLogTitles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cboLogTitles.FormattingEnabled = true;
            this.m_cboLogTitles.Items.AddRange(new object[] {
            "Debug",
            "Error",
            "Fatal",
            "Info",
            "Warning"});
            this.m_cboLogTitles.Location = new System.Drawing.Point(268, 12);
            this.m_cboLogTitles.Name = "m_cboLogTitles";
            this.m_cboLogTitles.Size = new System.Drawing.Size(121, 21);
            this.m_cboLogTitles.TabIndex = 3;
            // 
            // m_btnSimpleLog
            // 
            this.m_btnSimpleLog.Location = new System.Drawing.Point(395, 11);
            this.m_btnSimpleLog.Name = "m_btnSimpleLog";
            this.m_btnSimpleLog.Size = new System.Drawing.Size(75, 23);
            this.m_btnSimpleLog.TabIndex = 2;
            this.m_btnSimpleLog.Text = "Hit It";
            this.m_btnSimpleLog.UseVisualStyleBackColor = true;
            this.m_btnSimpleLog.Click += new System.EventHandler(this.m_btnSimpleLog_Click);
            // 
            // m_txtSimpleLogMessage
            // 
            this.m_txtSimpleLogMessage.Location = new System.Drawing.Point(86, 13);
            this.m_txtSimpleLogMessage.Name = "m_txtSimpleLogMessage";
            this.m_txtSimpleLogMessage.Size = new System.Drawing.Size(175, 20);
            this.m_txtSimpleLogMessage.TabIndex = 1;
            this.m_txtSimpleLogMessage.Text = "My Message";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Simple Log:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Scope:";
            // 
            // m_txtScopeMessage
            // 
            this.m_txtScopeMessage.Location = new System.Drawing.Point(86, 43);
            this.m_txtScopeMessage.Name = "m_txtScopeMessage";
            this.m_txtScopeMessage.Size = new System.Drawing.Size(175, 20);
            this.m_txtScopeMessage.TabIndex = 5;
            this.m_txtScopeMessage.Text = "Scope Message";
            // 
            // m_btnOpenScope
            // 
            this.m_btnOpenScope.Location = new System.Drawing.Point(268, 43);
            this.m_btnOpenScope.Name = "m_btnOpenScope";
            this.m_btnOpenScope.Size = new System.Drawing.Size(75, 23);
            this.m_btnOpenScope.TabIndex = 6;
            this.m_btnOpenScope.Text = "Open";
            this.m_btnOpenScope.UseVisualStyleBackColor = true;
            this.m_btnOpenScope.Click += new System.EventHandler(this.m_btnOpenScope_Click);
            // 
            // m_btnClose
            // 
            this.m_btnClose.Enabled = false;
            this.m_btnClose.Location = new System.Drawing.Point(349, 43);
            this.m_btnClose.Name = "m_btnClose";
            this.m_btnClose.Size = new System.Drawing.Size(75, 23);
            this.m_btnClose.TabIndex = 7;
            this.m_btnClose.Text = "Close";
            this.m_btnClose.UseVisualStyleBackColor = true;
            this.m_btnClose.Click += new System.EventHandler(this.m_btnClose_Click);
            // 
            // WhitelogSample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 483);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.m_groupBuilder);
            this.Name = "WhitelogSample";
            this.Text = "Whitelog Sample";
            this.m_groupBuilder.ResumeLayout(false);
            this.m_groupBuilder.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox m_groupBuilder;
        private System.Windows.Forms.TextBox m_txtPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button m_btnLogBuilder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox m_loggerTypes;
        private System.Windows.Forms.ComboBox m_submitterType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox m_bufferTypes;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox m_cboLogTitles;
        private System.Windows.Forms.Button m_btnSimpleLog;
        private System.Windows.Forms.TextBox m_txtSimpleLogMessage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button m_btnClose;
        private System.Windows.Forms.Button m_btnOpenScope;
        private System.Windows.Forms.TextBox m_txtScopeMessage;
        private System.Windows.Forms.Label label6;
    }
}


namespace Whitelog.Sample
{
    partial class LoggingSamples
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_btnClose = new System.Windows.Forms.Button();
            this.m_btnOpenScope = new System.Windows.Forms.Button();
            this.m_txtScopeMessage = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.m_cboLogTitles = new System.Windows.Forms.ComboBox();
            this.m_btnSimpleLog = new System.Windows.Forms.Button();
            this.m_txtSimpleLogMessage = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // m_btnClose
            // 
            this.m_btnClose.Enabled = false;
            this.m_btnClose.Location = new System.Drawing.Point(339, 36);
            this.m_btnClose.Name = "m_btnClose";
            this.m_btnClose.Size = new System.Drawing.Size(75, 23);
            this.m_btnClose.TabIndex = 15;
            this.m_btnClose.Text = "Close";
            this.m_btnClose.UseVisualStyleBackColor = true;
            this.m_btnClose.Click += new System.EventHandler(this.m_btnClose_Click);
            // 
            // m_btnOpenScope
            // 
            this.m_btnOpenScope.Location = new System.Drawing.Point(258, 36);
            this.m_btnOpenScope.Name = "m_btnOpenScope";
            this.m_btnOpenScope.Size = new System.Drawing.Size(75, 23);
            this.m_btnOpenScope.TabIndex = 14;
            this.m_btnOpenScope.Text = "Open";
            this.m_btnOpenScope.UseVisualStyleBackColor = true;
            this.m_btnOpenScope.Click += new System.EventHandler(this.m_btnOpenScope_Click);
            // 
            // m_txtScopeMessage
            // 
            this.m_txtScopeMessage.Location = new System.Drawing.Point(76, 36);
            this.m_txtScopeMessage.Name = "m_txtScopeMessage";
            this.m_txtScopeMessage.Size = new System.Drawing.Size(175, 20);
            this.m_txtScopeMessage.TabIndex = 13;
            this.m_txtScopeMessage.Text = "Scope Message";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Scope:";
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
            this.m_cboLogTitles.Location = new System.Drawing.Point(258, 5);
            this.m_cboLogTitles.Name = "m_cboLogTitles";
            this.m_cboLogTitles.Size = new System.Drawing.Size(121, 21);
            this.m_cboLogTitles.TabIndex = 11;
            // 
            // m_btnSimpleLog
            // 
            this.m_btnSimpleLog.Location = new System.Drawing.Point(385, 4);
            this.m_btnSimpleLog.Name = "m_btnSimpleLog";
            this.m_btnSimpleLog.Size = new System.Drawing.Size(75, 23);
            this.m_btnSimpleLog.TabIndex = 10;
            this.m_btnSimpleLog.Text = "Hit It";
            this.m_btnSimpleLog.UseVisualStyleBackColor = true;
            this.m_btnSimpleLog.Click += new System.EventHandler(this.m_btnSimpleLog_Click);
            // 
            // m_txtSimpleLogMessage
            // 
            this.m_txtSimpleLogMessage.Location = new System.Drawing.Point(76, 6);
            this.m_txtSimpleLogMessage.Name = "m_txtSimpleLogMessage";
            this.m_txtSimpleLogMessage.Size = new System.Drawing.Size(175, 20);
            this.m_txtSimpleLogMessage.TabIndex = 9;
            this.m_txtSimpleLogMessage.Text = "My Message";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Simple Log:";
            // 
            // LoggingSamples
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_btnClose);
            this.Controls.Add(this.m_btnOpenScope);
            this.Controls.Add(this.m_txtScopeMessage);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.m_cboLogTitles);
            this.Controls.Add(this.m_btnSimpleLog);
            this.Controls.Add(this.m_txtSimpleLogMessage);
            this.Controls.Add(this.label5);
            this.Name = "LoggingSamples";
            this.Size = new System.Drawing.Size(491, 172);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_btnClose;
        private System.Windows.Forms.Button m_btnOpenScope;
        private System.Windows.Forms.TextBox m_txtScopeMessage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox m_cboLogTitles;
        private System.Windows.Forms.Button m_btnSimpleLog;
        private System.Windows.Forms.TextBox m_txtSimpleLogMessage;
        private System.Windows.Forms.Label label5;
    }
}

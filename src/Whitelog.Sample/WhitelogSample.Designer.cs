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
            this.m_loggerTabControl = new System.Windows.Forms.TabControl();
            this.m_binaryTabPage = new System.Windows.Forms.TabPage();
            this.m_stringLayoutTabPage = new System.Windows.Forms.TabPage();
            this.m_consoleAppanderTabPage = new System.Windows.Forms.TabPage();
            this.m_samplePanel = new System.Windows.Forms.Panel();
            this.m_previewGroupbox = new System.Windows.Forms.GroupBox();
            this.m_previewTabControl = new System.Windows.Forms.TabControl();
            this.m_groupBuilder.SuspendLayout();
            this.m_loggerTabControl.SuspendLayout();
            this.m_previewGroupbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_groupBuilder
            // 
            this.m_groupBuilder.Controls.Add(this.m_loggerTabControl);
            this.m_groupBuilder.Location = new System.Drawing.Point(12, 12);
            this.m_groupBuilder.Name = "m_groupBuilder";
            this.m_groupBuilder.Size = new System.Drawing.Size(548, 194);
            this.m_groupBuilder.TabIndex = 0;
            this.m_groupBuilder.TabStop = false;
            this.m_groupBuilder.Text = "Logger Settings Builder";
            // 
            // m_loggerTabControl
            // 
            this.m_loggerTabControl.Controls.Add(this.m_binaryTabPage);
            this.m_loggerTabControl.Controls.Add(this.m_stringLayoutTabPage);
            this.m_loggerTabControl.Controls.Add(this.m_consoleAppanderTabPage);
            this.m_loggerTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_loggerTabControl.Location = new System.Drawing.Point(3, 16);
            this.m_loggerTabControl.Name = "m_loggerTabControl";
            this.m_loggerTabControl.SelectedIndex = 0;
            this.m_loggerTabControl.Size = new System.Drawing.Size(542, 175);
            this.m_loggerTabControl.TabIndex = 0;
            // 
            // m_binaryTabPage
            // 
            this.m_binaryTabPage.Location = new System.Drawing.Point(4, 22);
            this.m_binaryTabPage.Name = "m_binaryTabPage";
            this.m_binaryTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.m_binaryTabPage.Size = new System.Drawing.Size(534, 149);
            this.m_binaryTabPage.TabIndex = 0;
            this.m_binaryTabPage.Text = "BinaryFile";
            this.m_binaryTabPage.UseVisualStyleBackColor = true;
            // 
            // m_stringLayoutTabPage
            // 
            this.m_stringLayoutTabPage.Location = new System.Drawing.Point(4, 22);
            this.m_stringLayoutTabPage.Name = "m_stringLayoutTabPage";
            this.m_stringLayoutTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.m_stringLayoutTabPage.Size = new System.Drawing.Size(534, 149);
            this.m_stringLayoutTabPage.TabIndex = 1;
            this.m_stringLayoutTabPage.Text = "String Layout";
            this.m_stringLayoutTabPage.UseVisualStyleBackColor = true;
            // 
            // m_consoleAppanderTabPage
            // 
            this.m_consoleAppanderTabPage.Location = new System.Drawing.Point(4, 22);
            this.m_consoleAppanderTabPage.Name = "m_consoleAppanderTabPage";
            this.m_consoleAppanderTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.m_consoleAppanderTabPage.Size = new System.Drawing.Size(534, 149);
            this.m_consoleAppanderTabPage.TabIndex = 2;
            this.m_consoleAppanderTabPage.Text = "Color Appender";
            this.m_consoleAppanderTabPage.UseVisualStyleBackColor = true;
            // 
            // m_samplePanel
            // 
            this.m_samplePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.m_samplePanel.AutoScroll = true;
            this.m_samplePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_samplePanel.Location = new System.Drawing.Point(13, 212);
            this.m_samplePanel.Name = "m_samplePanel";
            this.m_samplePanel.Size = new System.Drawing.Size(547, 417);
            this.m_samplePanel.TabIndex = 1;
            // 
            // m_previewGroupbox
            // 
            this.m_previewGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_previewGroupbox.Controls.Add(this.m_previewTabControl);
            this.m_previewGroupbox.Location = new System.Drawing.Point(564, 13);
            this.m_previewGroupbox.Name = "m_previewGroupbox";
            this.m_previewGroupbox.Size = new System.Drawing.Size(408, 616);
            this.m_previewGroupbox.TabIndex = 2;
            this.m_previewGroupbox.TabStop = false;
            this.m_previewGroupbox.Text = "Preview";
            // 
            // m_previewTabControl
            // 
            this.m_previewTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_previewTabControl.Location = new System.Drawing.Point(3, 16);
            this.m_previewTabControl.Name = "m_previewTabControl";
            this.m_previewTabControl.SelectedIndex = 0;
            this.m_previewTabControl.Size = new System.Drawing.Size(402, 597);
            this.m_previewTabControl.TabIndex = 0;
            // 
            // WhitelogSample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 642);
            this.Controls.Add(this.m_previewGroupbox);
            this.Controls.Add(this.m_samplePanel);
            this.Controls.Add(this.m_groupBuilder);
            this.Name = "WhitelogSample";
            this.Text = "Whitelog Sample";
            this.m_groupBuilder.ResumeLayout(false);
            this.m_loggerTabControl.ResumeLayout(false);
            this.m_previewGroupbox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox m_groupBuilder;
        private System.Windows.Forms.Panel m_samplePanel;
        private System.Windows.Forms.TabControl m_loggerTabControl;
        private System.Windows.Forms.TabPage m_binaryTabPage;
        private System.Windows.Forms.TabPage m_stringLayoutTabPage;
        private System.Windows.Forms.TabPage m_consoleAppanderTabPage;
        private System.Windows.Forms.GroupBox m_previewGroupbox;
        private System.Windows.Forms.TabControl m_previewTabControl;
    }
}


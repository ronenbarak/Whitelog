namespace Whitelog.Sample
{
    partial class ColorLoggerBuilder
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
            this.m_btnBuild = new System.Windows.Forms.Button();
            this.m_cboLayout = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.m_chkDefaultColors = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.m_cboInfoForeground = new System.Windows.Forms.ComboBox();
            this.m_cboErrorForeground = new System.Windows.Forms.ComboBox();
            this.m_cboErrorBackground = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // m_btnBuild
            // 
            this.m_btnBuild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btnBuild.Location = new System.Drawing.Point(312, 117);
            this.m_btnBuild.Name = "m_btnBuild";
            this.m_btnBuild.Size = new System.Drawing.Size(75, 23);
            this.m_btnBuild.TabIndex = 0;
            this.m_btnBuild.Text = "Build";
            this.m_btnBuild.UseVisualStyleBackColor = true;
            this.m_btnBuild.Click += new System.EventHandler(this.m_btnBuild_Click);
            // 
            // m_cboLayout
            // 
            this.m_cboLayout.DisplayMember = "Layout";
            this.m_cboLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cboLayout.FormattingEnabled = true;
            this.m_cboLayout.Location = new System.Drawing.Point(51, 13);
            this.m_cboLayout.Name = "m_cboLayout";
            this.m_cboLayout.Size = new System.Drawing.Size(248, 21);
            this.m_cboLayout.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Layout:";
            // 
            // m_chkDefaultColors
            // 
            this.m_chkDefaultColors.AutoSize = true;
            this.m_chkDefaultColors.Checked = true;
            this.m_chkDefaultColors.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_chkDefaultColors.Location = new System.Drawing.Point(6, 40);
            this.m_chkDefaultColors.Name = "m_chkDefaultColors";
            this.m_chkDefaultColors.Size = new System.Drawing.Size(92, 17);
            this.m_chkDefaultColors.TabIndex = 3;
            this.m_chkDefaultColors.Text = "Default Colors";
            this.m_chkDefaultColors.UseVisualStyleBackColor = true;
            this.m_chkDefaultColors.CheckedChanged += new System.EventHandler(this.m_chkDefaultColors_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Info Foreground:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Error Foreground:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Error Background:";
            // 
            // m_cboInfoForeground
            // 
            this.m_cboInfoForeground.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cboInfoForeground.FormattingEnabled = true;
            this.m_cboInfoForeground.Items.AddRange(new object[] {
            "None",
            "Black",
            "DarkBlue",
            "DarkGreen",
            "DarkCyan",
            "DarkRed",
            "DarkMagenta",
            "DarkYellow",
            "Gray",
            "DarkGray",
            "Blue",
            "Green",
            "Cyan",
            "Red",
            "Magenta",
            "Yellow",
            "White"});
            this.m_cboInfoForeground.Location = new System.Drawing.Point(94, 57);
            this.m_cboInfoForeground.Name = "m_cboInfoForeground";
            this.m_cboInfoForeground.Size = new System.Drawing.Size(158, 21);
            this.m_cboInfoForeground.TabIndex = 7;
            // 
            // m_cboErrorForeground
            // 
            this.m_cboErrorForeground.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cboErrorForeground.FormattingEnabled = true;
            this.m_cboErrorForeground.Items.AddRange(new object[] {
            "None",
            "Black",
            "DarkBlue",
            "DarkGreen",
            "DarkCyan",
            "DarkRed",
            "DarkMagenta",
            "DarkYellow",
            "Gray",
            "DarkGray",
            "Blue",
            "Green",
            "Cyan",
            "Red",
            "Magenta",
            "Yellow",
            "White"});
            this.m_cboErrorForeground.Location = new System.Drawing.Point(94, 86);
            this.m_cboErrorForeground.Name = "m_cboErrorForeground";
            this.m_cboErrorForeground.Size = new System.Drawing.Size(158, 21);
            this.m_cboErrorForeground.TabIndex = 8;
            // 
            // m_cboErrorBackground
            // 
            this.m_cboErrorBackground.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cboErrorBackground.FormattingEnabled = true;
            this.m_cboErrorBackground.Items.AddRange(new object[] {
            "None",
            "Black",
            "DarkBlue",
            "DarkGreen",
            "DarkCyan",
            "DarkRed",
            "DarkMagenta",
            "DarkYellow",
            "Gray",
            "DarkGray",
            "Blue",
            "Green",
            "Cyan",
            "Red",
            "Magenta",
            "Yellow",
            "White"});
            this.m_cboErrorBackground.Location = new System.Drawing.Point(94, 117);
            this.m_cboErrorBackground.Name = "m_cboErrorBackground";
            this.m_cboErrorBackground.Size = new System.Drawing.Size(158, 21);
            this.m_cboErrorBackground.TabIndex = 9;
            // 
            // ColorLoggerBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_cboErrorBackground);
            this.Controls.Add(this.m_cboErrorForeground);
            this.Controls.Add(this.m_cboInfoForeground);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.m_chkDefaultColors);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_cboLayout);
            this.Controls.Add(this.m_btnBuild);
            this.Name = "ColorLoggerBuilder";
            this.Size = new System.Drawing.Size(390, 147);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_btnBuild;
        private System.Windows.Forms.ComboBox m_cboLayout;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox m_chkDefaultColors;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox m_cboInfoForeground;
        private System.Windows.Forms.ComboBox m_cboErrorForeground;
        private System.Windows.Forms.ComboBox m_cboErrorBackground;
    }
}

namespace Whitelog.Sample
{
    partial class BinaryLoggerBuilder
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
            this.m_bufferTypes = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.m_submitterType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.m_txtPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.m_btnLogBuilder = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.m_loggerTypes = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // m_bufferTypes
            // 
            this.m_bufferTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_bufferTypes.FormattingEnabled = true;
            this.m_bufferTypes.Location = new System.Drawing.Point(94, 63);
            this.m_bufferTypes.Name = "m_bufferTypes";
            this.m_bufferTypes.Size = new System.Drawing.Size(315, 21);
            this.m_bufferTypes.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Buffer:";
            // 
            // m_submitterType
            // 
            this.m_submitterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_submitterType.FormattingEnabled = true;
            this.m_submitterType.Location = new System.Drawing.Point(94, 35);
            this.m_submitterType.Name = "m_submitterType";
            this.m_submitterType.Size = new System.Drawing.Size(315, 21);
            this.m_submitterType.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Submitter:";
            // 
            // m_txtPath
            // 
            this.m_txtPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_txtPath.Location = new System.Drawing.Point(94, 90);
            this.m_txtPath.Name = "m_txtPath";
            this.m_txtPath.Size = new System.Drawing.Size(443, 20);
            this.m_txtPath.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Path:";
            // 
            // m_btnLogBuilder
            // 
            this.m_btnLogBuilder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btnLogBuilder.Location = new System.Drawing.Point(543, 88);
            this.m_btnLogBuilder.Name = "m_btnLogBuilder";
            this.m_btnLogBuilder.Size = new System.Drawing.Size(75, 23);
            this.m_btnLogBuilder.TabIndex = 11;
            this.m_btnLogBuilder.Text = "Build";
            this.m_btnLogBuilder.UseVisualStyleBackColor = true;
            this.m_btnLogBuilder.Click += new System.EventHandler(this.m_btnLogBuilder_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Logger Type:";
            // 
            // m_loggerTypes
            // 
            this.m_loggerTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_loggerTypes.FormattingEnabled = true;
            this.m_loggerTypes.Location = new System.Drawing.Point(94, 6);
            this.m_loggerTypes.Name = "m_loggerTypes";
            this.m_loggerTypes.Size = new System.Drawing.Size(315, 21);
            this.m_loggerTypes.TabIndex = 9;
            // 
            // BinaryLoggerBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_bufferTypes);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.m_submitterType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.m_txtPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.m_btnLogBuilder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_loggerTypes);
            this.Name = "BinaryLoggerBuilder";
            this.Size = new System.Drawing.Size(638, 138);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox m_bufferTypes;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox m_submitterType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox m_txtPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button m_btnLogBuilder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox m_loggerTypes;
    }
}

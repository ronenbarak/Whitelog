namespace Whitelog.Sample
{
    partial class StringLayoutLoggerBuilder
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
            this.label1 = new System.Windows.Forms.Label();
            this.m_txtLayout = new System.Windows.Forms.TextBox();
            this.m_btnBuild = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.m_extensions = new System.Windows.Forms.ComboBox();
            this.m_btnAddExtension = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.m_lstBuildLayouts = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Layout:";
            // 
            // m_txtLayout
            // 
            this.m_txtLayout.Location = new System.Drawing.Point(79, 42);
            this.m_txtLayout.Name = "m_txtLayout";
            this.m_txtLayout.Size = new System.Drawing.Size(323, 20);
            this.m_txtLayout.TabIndex = 1;
            this.m_txtLayout.Text = "${longdate} ${title} ${message}";
            // 
            // m_btnBuild
            // 
            this.m_btnBuild.Location = new System.Drawing.Point(409, 42);
            this.m_btnBuild.Name = "m_btnBuild";
            this.m_btnBuild.Size = new System.Drawing.Size(75, 23);
            this.m_btnBuild.TabIndex = 2;
            this.m_btnBuild.Text = "Build";
            this.m_btnBuild.UseVisualStyleBackColor = true;
            this.m_btnBuild.Click += new System.EventHandler(this.m_btnBuild_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Extensions:";
            // 
            // m_extensions
            // 
            this.m_extensions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_extensions.FormattingEnabled = true;
            this.m_extensions.Location = new System.Drawing.Point(79, 12);
            this.m_extensions.Name = "m_extensions";
            this.m_extensions.Size = new System.Drawing.Size(323, 21);
            this.m_extensions.TabIndex = 4;
            // 
            // m_btnAddExtension
            // 
            this.m_btnAddExtension.Location = new System.Drawing.Point(409, 10);
            this.m_btnAddExtension.Name = "m_btnAddExtension";
            this.m_btnAddExtension.Size = new System.Drawing.Size(75, 23);
            this.m_btnAddExtension.TabIndex = 5;
            this.m_btnAddExtension.Text = "Add Extension";
            this.m_btnAddExtension.UseVisualStyleBackColor = true;
            this.m_btnAddExtension.Click += new System.EventHandler(this.m_btnAddExtension_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Build Layouts:";
            // 
            // m_lstBuildLayouts
            // 
            this.m_lstBuildLayouts.FormattingEnabled = true;
            this.m_lstBuildLayouts.Location = new System.Drawing.Point(90, 78);
            this.m_lstBuildLayouts.Name = "m_lstBuildLayouts";
            this.m_lstBuildLayouts.Size = new System.Drawing.Size(394, 56);
            this.m_lstBuildLayouts.TabIndex = 7;
            // 
            // StringLayoutLoggerBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_lstBuildLayouts);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.m_btnAddExtension);
            this.Controls.Add(this.m_extensions);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.m_btnBuild);
            this.Controls.Add(this.m_txtLayout);
            this.Controls.Add(this.label1);
            this.Name = "StringLayoutLoggerBuilder";
            this.Size = new System.Drawing.Size(501, 151);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox m_txtLayout;
        private System.Windows.Forms.Button m_btnBuild;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox m_extensions;
        private System.Windows.Forms.Button m_btnAddExtension;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox m_lstBuildLayouts;
    }
}

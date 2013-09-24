namespace Whitelog.Viewer
{
    partial class WhiteLogViewerForm
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_logTreeView = new BrightIdeasSoftware.DataTreeListView();
            this.olvTimeColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvTitleColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvMessageColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_logTreeView)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 535);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(673, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(673, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(100, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // m_logTreeView
            // 
            this.m_logTreeView.AllColumns.Add(this.olvTimeColumn);
            this.m_logTreeView.AllColumns.Add(this.olvTitleColumn);
            this.m_logTreeView.AllColumns.Add(this.olvMessageColumn);
            this.m_logTreeView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvTimeColumn,
            this.olvTitleColumn,
            this.olvMessageColumn});
            this.m_logTreeView.DataSource = null;
            this.m_logTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_logTreeView.FullRowSelect = true;
            this.m_logTreeView.HeaderUsesThemes = false;
            this.m_logTreeView.HideSelection = false;
            this.m_logTreeView.Location = new System.Drawing.Point(0, 24);
            this.m_logTreeView.Name = "m_logTreeView";
            this.m_logTreeView.OwnerDraw = true;
            this.m_logTreeView.RootKeyValueString = "";
            this.m_logTreeView.ShowGroups = false;
            this.m_logTreeView.Size = new System.Drawing.Size(673, 511);
            this.m_logTreeView.TabIndex = 2;
            this.m_logTreeView.UseCompatibleStateImageBehavior = false;
            this.m_logTreeView.UseFilterIndicator = true;
            this.m_logTreeView.UseFiltering = true;
            this.m_logTreeView.View = System.Windows.Forms.View.Details;
            this.m_logTreeView.VirtualMode = true;
            // 
            // olvTimeColumn
            // 
            this.olvTimeColumn.AspectName = "Time";
            this.olvTimeColumn.CellPadding = null;
            this.olvTimeColumn.Groupable = false;
            this.olvTimeColumn.Sortable = false;
            this.olvTimeColumn.Text = "Time";
            this.olvTimeColumn.Width = 200;
            // 
            // olvTitleColumn
            // 
            this.olvTitleColumn.AspectName = "Title";
            this.olvTitleColumn.CellPadding = null;
            this.olvTitleColumn.Sortable = false;
            this.olvTitleColumn.Text = "Title";
            // 
            // olvMessageColumn
            // 
            this.olvMessageColumn.AspectName = "Message";
            this.olvMessageColumn.CellPadding = null;
            this.olvMessageColumn.FillsFreeSpace = true;
            this.olvMessageColumn.Groupable = false;
            this.olvMessageColumn.Sortable = false;
            this.olvMessageColumn.Text = "Message";
            // 
            // WhiteLogViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 557);
            this.Controls.Add(this.m_logTreeView);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "WhiteLogViewerForm";
            this.Text = "Whitelog Viewer";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_logTreeView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private BrightIdeasSoftware.DataTreeListView m_logTreeView;
        private BrightIdeasSoftware.OLVColumn olvTimeColumn;
        private BrightIdeasSoftware.OLVColumn olvTitleColumn;
        private BrightIdeasSoftware.OLVColumn olvMessageColumn;
    }
}


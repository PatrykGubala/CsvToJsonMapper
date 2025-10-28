namespace CsvJsonMapper.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openCsvToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.leftPanelSplitContainer = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tvSourceFiles = new System.Windows.Forms.TreeView();
            this.groupBoxJsonStructure = new System.Windows.Forms.GroupBox();
            this.tvJsonStructure = new System.Windows.Forms.TreeView();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.tabPageCsvPreview = new System.Windows.Forms.TabPage();
            this.groupBoxCsvViews = new System.Windows.Forms.GroupBox();
            this.tabControlCsvViews = new System.Windows.Forms.TabControl();
            this.tabPageJsonPreview = new System.Windows.Forms.TabPage();
            this.groupBoxJsonPreview = new System.Windows.Forms.GroupBox();
            this.rtbJsonPreview = new System.Windows.Forms.RichTextBox();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.leftPanelSplitContainer)).BeginInit();
            this.leftPanelSplitContainer.Panel1.SuspendLayout();
            this.leftPanelSplitContainer.Panel2.SuspendLayout();
            this.leftPanelSplitContainer.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBoxJsonStructure.SuspendLayout();
            this.mainTabControl.SuspendLayout();
            this.tabPageCsvPreview.SuspendLayout();
            this.groupBoxCsvViews.SuspendLayout();
            this.tabPageJsonPreview.SuspendLayout();
            this.groupBoxJsonPreview.SuspendLayout();
            this.SuspendLayout();
             
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1184, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
           
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openCsvToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.fileToolStripMenuItem.Text = "Plik";
          
            this.openCsvToolStripMenuItem.Name = "openCsvToolStripMenuItem";
            this.openCsvToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openCsvToolStripMenuItem.Text = "Importuj pliki CSV...";
            this.openCsvToolStripMenuItem.Click += new System.EventHandler(this.openCsvToolStripMenuItem_Click);
            
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Zakończ";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 639);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1184, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
           
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(46, 17);
            this.lblStatus.Text = "Gotowy";
           
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 24);
            this.mainSplitContainer.Name = "mainSplitContainer";
          
            this.mainSplitContainer.Panel1.Controls.Add(this.leftPanelSplitContainer);
           
            this.mainSplitContainer.Panel2.Controls.Add(this.mainTabControl);
            this.mainSplitContainer.Size = new System.Drawing.Size(1184, 615);
            this.mainSplitContainer.SplitterDistance = 450;
            this.mainSplitContainer.TabIndex = 2;
           
            this.leftPanelSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftPanelSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.leftPanelSplitContainer.Name = "leftPanelSplitContainer";
            this.leftPanelSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
           
            this.leftPanelSplitContainer.Panel1.Controls.Add(this.groupBox1);
            
            this.leftPanelSplitContainer.Panel2.Controls.Add(this.groupBoxJsonStructure);
            this.leftPanelSplitContainer.Size = new System.Drawing.Size(450, 615);
            this.leftPanelSplitContainer.SplitterDistance = 300;
            this.leftPanelSplitContainer.TabIndex = 0;
           
            this.groupBox1.Controls.Add(this.tvSourceFiles);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox1.Size = new System.Drawing.Size(450, 300);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Źródła danych (Pliki CSV)";
            
            this.tvSourceFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvSourceFiles.Location = new System.Drawing.Point(6, 19);
            this.tvSourceFiles.Name = "tvSourceFiles";
            this.tvSourceFiles.Size = new System.Drawing.Size(438, 275);
            this.tvSourceFiles.TabIndex = 0;
           
            this.groupBoxJsonStructure.Controls.Add(this.tvJsonStructure);
            this.groupBoxJsonStructure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxJsonStructure.Location = new System.Drawing.Point(0, 0);
            this.groupBoxJsonStructure.Name = "groupBoxJsonStructure";
            this.groupBoxJsonStructure.Padding = new System.Windows.Forms.Padding(6);
            this.groupBoxJsonStructure.Size = new System.Drawing.Size(450, 311);
            this.groupBoxJsonStructure.TabIndex = 0;
            this.groupBoxJsonStructure.TabStop = false;
            this.groupBoxJsonStructure.Text = "Kreator struktury JSON";
            
            this.tvJsonStructure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvJsonStructure.Location = new System.Drawing.Point(6, 19);
            this.tvJsonStructure.Name = "tvJsonStructure";
            this.tvJsonStructure.Size = new System.Drawing.Size(438, 286);
            this.tvJsonStructure.TabIndex = 0;
            
            this.mainTabControl.Controls.Add(this.tabPageCsvPreview);
            this.mainTabControl.Controls.Add(this.tabPageJsonPreview);
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Location = new System.Drawing.Point(0, 0);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(730, 615);
            this.mainTabControl.TabIndex = 0;
            
            this.tabPageCsvPreview.Controls.Add(this.groupBoxCsvViews);
            this.tabPageCsvPreview.Location = new System.Drawing.Point(4, 22);
            this.tabPageCsvPreview.Name = "tabPageCsvPreview";
            this.tabPageCsvPreview.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCsvPreview.Size = new System.Drawing.Size(722, 589);
            this.tabPageCsvPreview.TabIndex = 0;
            this.tabPageCsvPreview.Text = "Podgląd CSV (Surowy)";
            this.tabPageCsvPreview.UseVisualStyleBackColor = true;
            
            this.groupBoxCsvViews.Controls.Add(this.tabControlCsvViews);
            this.groupBoxCsvViews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxCsvViews.Location = new System.Drawing.Point(3, 3);
            this.groupBoxCsvViews.Name = "groupBoxCsvViews";
            this.groupBoxCsvViews.Padding = new System.Windows.Forms.Padding(6);
            this.groupBoxCsvViews.Size = new System.Drawing.Size(716, 583);
            this.groupBoxCsvViews.TabIndex = 1;
            this.groupBoxCsvViews.TabStop = false;
            this.groupBoxCsvViews.Text = "Podgląd plików";
            
            this.tabControlCsvViews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlCsvViews.Location = new System.Drawing.Point(6, 19);
            this.tabControlCsvViews.Name = "tabControlCsvViews";
            this.tabControlCsvViews.SelectedIndex = 0;
            this.tabControlCsvViews.Size = new System.Drawing.Size(704, 558);
            this.tabControlCsvViews.TabIndex = 0;
            
            this.tabPageJsonPreview.Controls.Add(this.groupBoxJsonPreview);
            this.tabPageJsonPreview.Location = new System.Drawing.Point(4, 22);
            this.tabPageJsonPreview.Name = "tabPageJsonPreview";
            this.tabPageJsonPreview.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageJsonPreview.Size = new System.Drawing.Size(722, 589);
            this.tabPageJsonPreview.TabIndex = 1;
            this.tabPageJsonPreview.Text = "Podgląd JSON";
            this.tabPageJsonPreview.UseVisualStyleBackColor = true;
           
            this.groupBoxJsonPreview.Controls.Add(this.rtbJsonPreview);
            this.groupBoxJsonPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxJsonPreview.Location = new System.Drawing.Point(3, 3);
            this.groupBoxJsonPreview.Name = "groupBoxJsonPreview";
            this.groupBoxJsonPreview.Padding = new System.Windows.Forms.Padding(6);
            this.groupBoxJsonPreview.Size = new System.Drawing.Size(716, 583);
            this.groupBoxJsonPreview.TabIndex = 0;
            this.groupBoxJsonPreview.TabStop = false;
            this.groupBoxJsonPreview.Text = "Podgląd JSON (Przetworzony)";
           
            this.rtbJsonPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbJsonPreview.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rtbJsonPreview.Location = new System.Drawing.Point(6, 19);
            this.rtbJsonPreview.Name = "rtbJsonPreview";
            this.rtbJsonPreview.ReadOnly = true;
            this.rtbJsonPreview.Size = new System.Drawing.Size(704, 558);
            this.rtbJsonPreview.TabIndex = 0;
            this.rtbJsonPreview.Text = "";
            this.rtbJsonPreview.WordWrap = false;
           
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 661);
            this.Controls.Add(this.mainSplitContainer);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "CSV to JSON Mapper";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.leftPanelSplitContainer.Panel1.ResumeLayout(false);
            this.leftPanelSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.leftPanelSplitContainer)).EndInit();
            this.leftPanelSplitContainer.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBoxJsonStructure.ResumeLayout(false);
            this.mainTabControl.ResumeLayout(false);
            this.tabPageCsvPreview.ResumeLayout(false);
            this.groupBoxCsvViews.ResumeLayout(false);
            this.tabPageJsonPreview.ResumeLayout(false);
            this.groupBoxJsonPreview.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openCsvToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private System.Windows.Forms.SplitContainer leftPanelSplitContainer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TreeView tvSourceFiles;
        private System.Windows.Forms.GroupBox groupBoxJsonStructure;
        private System.Windows.Forms.TreeView tvJsonStructure;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage tabPageCsvPreview;
        private System.Windows.Forms.GroupBox groupBoxCsvViews;
        private System.Windows.Forms.TabControl tabControlCsvViews;
        private System.Windows.Forms.TabPage tabPageJsonPreview;
        private System.Windows.Forms.GroupBox groupBoxJsonPreview;
        private System.Windows.Forms.RichTextBox rtbJsonPreview;
    }
}
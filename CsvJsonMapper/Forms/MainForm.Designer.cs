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
            this.mappingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manageRelationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.leftPanelSplitContainer = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tvSourceFiles = new System.Windows.Forms.TreeView();
            this.groupBoxJsonTemplate = new System.Windows.Forms.GroupBox();
            this.structurePreviewSplitter = new System.Windows.Forms.SplitContainer();
            this.tvJsonStructure = new System.Windows.Forms.TreeView();
            this.groupBoxJsonStructurePreview = new System.Windows.Forms.GroupBox();
            this.rtbJsonStructurePreview = new System.Windows.Forms.RichTextBox();
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
            this.groupBoxJsonTemplate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.structurePreviewSplitter)).BeginInit();
            this.structurePreviewSplitter.Panel1.SuspendLayout();
            this.structurePreviewSplitter.Panel2.SuspendLayout();
            this.structurePreviewSplitter.SuspendLayout();
            this.groupBoxJsonStructurePreview.SuspendLayout();
            this.mainTabControl.SuspendLayout();
            this.tabPageCsvPreview.SuspendLayout();
            this.groupBoxCsvViews.SuspendLayout();
            this.tabPageJsonPreview.SuspendLayout();
            this.groupBoxJsonPreview.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.mappingToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1184, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openCsvToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.fileToolStripMenuItem.Text = "Plik";
            // 
            // openCsvToolStripMenuItem
            // 
            this.openCsvToolStripMenuItem.Name = "openCsvToolStripMenuItem";
            this.openCsvToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openCsvToolStripMenuItem.Text = "Importuj pliki CSV...";
            this.openCsvToolStripMenuItem.Click += new System.EventHandler(this.openCsvToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Zakończ";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // mappingToolStripMenuItem
            // 
            this.mappingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manageRelationsToolStripMenuItem});
            this.mappingToolStripMenuItem.Name = "mappingToolStripMenuItem";
            this.mappingToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.mappingToolStripMenuItem.Text = "Mapowanie";
            // 
            // manageRelationsToolStripMenuItem
            // 
            this.manageRelationsToolStripMenuItem.Name = "manageRelationsToolStripMenuItem";
            this.manageRelationsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.manageRelationsToolStripMenuItem.Text = "Zarządzaj Relacjami...";
            this.manageRelationsToolStripMenuItem.Click += new System.EventHandler(this.manageRelationsToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 639);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1184, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(46, 17);
            this.lblStatus.Text = "Gotowy";
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 24);
            this.mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.Controls.Add(this.leftPanelSplitContainer);
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.mainTabControl);
            this.mainSplitContainer.Size = new System.Drawing.Size(1184, 615);
            this.mainSplitContainer.SplitterDistance = 450;
            this.mainSplitContainer.TabIndex = 2;
            // 
            // leftPanelSplitContainer
            // 
            this.leftPanelSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftPanelSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.leftPanelSplitContainer.Name = "leftPanelSplitContainer";
            this.leftPanelSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // leftPanelSplitContainer.Panel1
            // 
            this.leftPanelSplitContainer.Panel1.Controls.Add(this.groupBox1);
            // 
            // leftPanelSplitContainer.Panel2
            // 
            this.leftPanelSplitContainer.Panel2.Controls.Add(this.groupBoxJsonTemplate);
            this.leftPanelSplitContainer.Size = new System.Drawing.Size(450, 615);
            this.leftPanelSplitContainer.SplitterDistance = 300;
            this.leftPanelSplitContainer.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tvSourceFiles);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox1.Size = new System.Drawing.Size(450, 300);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Źródła danych CSV";
            // 
            // tvSourceFiles
            // 
            this.tvSourceFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvSourceFiles.Location = new System.Drawing.Point(6, 19);
            this.tvSourceFiles.Name = "tvSourceFiles";
            this.tvSourceFiles.Size = new System.Drawing.Size(438, 275);
            this.tvSourceFiles.TabIndex = 0;
            // 
            // groupBoxJsonTemplate
            // 
            this.groupBoxJsonTemplate.Controls.Add(this.structurePreviewSplitter);
            this.groupBoxJsonTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxJsonTemplate.Location = new System.Drawing.Point(0, 0);
            this.groupBoxJsonTemplate.Name = "groupBoxJsonTemplate";
            this.groupBoxJsonTemplate.Padding = new System.Windows.Forms.Padding(6);
            this.groupBoxJsonTemplate.Size = new System.Drawing.Size(450, 311);
            this.groupBoxJsonTemplate.TabIndex = 0;
            this.groupBoxJsonTemplate.TabStop = false;
            this.groupBoxJsonTemplate.Text = "Struktura JSON";
            // 
            // structurePreviewSplitter
            // 
            this.structurePreviewSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.structurePreviewSplitter.Location = new System.Drawing.Point(6, 19);
            this.structurePreviewSplitter.Name = "structurePreviewSplitter";
            this.structurePreviewSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // structurePreviewSplitter.Panel1
            // 
            this.structurePreviewSplitter.Panel1.Controls.Add(this.tvJsonStructure);
            // 
            // structurePreviewSplitter.Panel2
            // 
            this.structurePreviewSplitter.Panel2.Controls.Add(this.groupBoxJsonStructurePreview);
            this.structurePreviewSplitter.Size = new System.Drawing.Size(438, 286);
            this.structurePreviewSplitter.SplitterDistance = 140;
            this.structurePreviewSplitter.TabIndex = 1;
            // 
            // tvJsonStructure
            // 
            this.tvJsonStructure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvJsonStructure.Location = new System.Drawing.Point(0, 0);
            this.tvJsonStructure.Name = "tvJsonStructure";
            this.tvJsonStructure.Size = new System.Drawing.Size(438, 140);
            this.tvJsonStructure.TabIndex = 0;
            // 
            // groupBoxJsonStructurePreview
            // 
            this.groupBoxJsonStructurePreview.Controls.Add(this.rtbJsonStructurePreview);
            this.groupBoxJsonStructurePreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxJsonStructurePreview.Location = new System.Drawing.Point(0, 0);
            this.groupBoxJsonStructurePreview.Name = "groupBoxJsonStructurePreview";
            this.groupBoxJsonStructurePreview.Padding = new System.Windows.Forms.Padding(6);
            this.groupBoxJsonStructurePreview.Size = new System.Drawing.Size(438, 142);
            this.groupBoxJsonStructurePreview.TabIndex = 0;
            this.groupBoxJsonStructurePreview.TabStop = false;
            this.groupBoxJsonStructurePreview.Text = "Podgląd Struktury (na żywo)";
            // 
            // rtbJsonStructurePreview
            // 
            this.rtbJsonStructurePreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbJsonStructurePreview.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rtbJsonStructurePreview.Location = new System.Drawing.Point(6, 19);
            this.rtbJsonStructurePreview.Name = "rtbJsonStructurePreview";
            this.rtbJsonStructurePreview.ReadOnly = true;
            this.rtbJsonStructurePreview.Size = new System.Drawing.Size(426, 117);
            this.rtbJsonStructurePreview.TabIndex = 0;
            this.rtbJsonStructurePreview.Text = "";
            this.rtbJsonStructurePreview.WordWrap = false;
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.tabPageCsvPreview);
            this.mainTabControl.Controls.Add(this.tabPageJsonPreview);
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Location = new System.Drawing.Point(0, 0);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(730, 615);
            this.mainTabControl.TabIndex = 0;
            // 
            // tabPageCsvPreview
            // 
            this.tabPageCsvPreview.Controls.Add(this.groupBoxCsvViews);
            this.tabPageCsvPreview.Location = new System.Drawing.Point(4, 22);
            this.tabPageCsvPreview.Name = "tabPageCsvPreview";
            this.tabPageCsvPreview.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCsvPreview.Size = new System.Drawing.Size(722, 589);
            this.tabPageCsvPreview.TabIndex = 0;
            this.tabPageCsvPreview.Text = "Podgląd CSV";
            this.tabPageCsvPreview.UseVisualStyleBackColor = true;
            // 
            // groupBoxCsvViews
            // 
            this.groupBoxCsvViews.Controls.Add(this.tabControlCsvViews);
            this.groupBoxCsvViews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxCsvViews.Location = new System.Drawing.Point(3, 3);
            this.groupBoxCsvViews.Name = "groupBoxCsvViews";
            this.groupBoxCsvViews.Padding = new System.Windows.Forms.Padding(6);
            this.groupBoxCsvViews.Size = new System.Drawing.Size(716, 583);
            this.groupBoxCsvViews.TabIndex = 1;
            this.groupBoxCsvViews.TabStop = false;
            this.groupBoxCsvViews.Text = "Podgląd plików";
            // 
            // tabControlCsvViews
            // 
            this.tabControlCsvViews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlCsvViews.Location = new System.Drawing.Point(6, 19);
            this.tabControlCsvViews.Name = "tabControlCsvViews";
            this.tabControlCsvViews.SelectedIndex = 0;
            this.tabControlCsvViews.Size = new System.Drawing.Size(704, 558);
            this.tabControlCsvViews.TabIndex = 0;
            // 
            // tabPageJsonPreview
            // 
            this.tabPageJsonPreview.Controls.Add(this.groupBoxJsonPreview);
            this.tabPageJsonPreview.Location = new System.Drawing.Point(4, 22);
            this.tabPageJsonPreview.Name = "tabPageJsonPreview";
            this.tabPageJsonPreview.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageJsonPreview.Size = new System.Drawing.Size(722, 589);
            this.tabPageJsonPreview.TabIndex = 1;
            this.tabPageJsonPreview.Text = "Stworzony JSON";
            this.tabPageJsonPreview.UseVisualStyleBackColor = true;
            // 
            // groupBoxJsonPreview
            // 
            this.groupBoxJsonPreview.Controls.Add(this.rtbJsonPreview);
            this.groupBoxJsonPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxJsonPreview.Location = new System.Drawing.Point(3, 3);
            this.groupBoxJsonPreview.Name = "groupBoxJsonPreview";
            this.groupBoxJsonPreview.Padding = new System.Windows.Forms.Padding(6);
            this.groupBoxJsonPreview.Size = new System.Drawing.Size(716, 583);
            this.groupBoxJsonPreview.TabIndex = 0;
            this.groupBoxJsonPreview.TabStop = false;
            this.groupBoxJsonPreview.Text = "Stworzony JSON (z danymi)";
            // 
            // rtbJsonPreview
            // 
            this.rtbJsonPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbJsonPreview.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rtbJsonPreview.Location = new System.Drawing.Point(6, 19);
            this.rtbJsonPreview.Name = "rtbJsonPreview";
            this.rtbJsonPreview.ReadOnly = true;
            this.rtbJsonPreview.Size = new System.Drawing.Size(704, 558);
            this.rtbJsonPreview.TabIndex = 0;
            this.rtbJsonPreview.Text = "";
            this.rtbJsonPreview.WordWrap = false;
            // 
            // MainForm
            // 
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
            this.groupBoxJsonTemplate.ResumeLayout(false);
            this.structurePreviewSplitter.Panel1.ResumeLayout(false);
            this.structurePreviewSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.structurePreviewSplitter)).EndInit();
            this.structurePreviewSplitter.ResumeLayout(false);
            this.groupBoxJsonStructurePreview.ResumeLayout(false);
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
        private System.Windows.Forms.GroupBox groupBoxJsonTemplate;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage tabPageCsvPreview;
        private System.Windows.Forms.GroupBox groupBoxCsvViews;
        private System.Windows.Forms.TabControl tabControlCsvViews;
        private System.Windows.Forms.TabPage tabPageJsonPreview;
        private System.Windows.Forms.GroupBox groupBoxJsonPreview;
        private System.Windows.Forms.RichTextBox rtbJsonPreview;
        private System.Windows.Forms.TreeView tvJsonStructure;
        private System.Windows.Forms.SplitContainer structurePreviewSplitter;
        private System.Windows.Forms.GroupBox groupBoxJsonStructurePreview;
        private System.Windows.Forms.RichTextBox rtbJsonStructurePreview;
        private System.Windows.Forms.ToolStripMenuItem mappingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manageRelationsToolStripMenuItem;
    }
}
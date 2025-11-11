namespace CsvJsonMapper.Forms.Dialogs
{
    partial class ImportConfigurationDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.lbFiles = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblSelectedFile = new System.Windows.Forms.Label();
            this.txtMetadataRows = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numHeaderRow = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbRootFile = new System.Windows.Forms.ComboBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.rightSplitContainer = new System.Windows.Forms.SplitContainer();
            this.topRightSplitContainer = new System.Windows.Forms.SplitContainer();
            this.groupBoxColumnTypes = new System.Windows.Forms.GroupBox();
            this.dgvColumnTypes = new System.Windows.Forms.DataGridView();
            this.groupBoxPreview = new System.Windows.Forms.GroupBox();
            this.dgvPreview = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHeaderRow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            this.leftPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rightSplitContainer)).BeginInit();
            this.rightSplitContainer.Panel1.SuspendLayout();
            this.rightSplitContainer.Panel2.SuspendLayout();
            this.rightSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.topRightSplitContainer)).BeginInit();
            this.topRightSplitContainer.Panel1.SuspendLayout();
            this.topRightSplitContainer.Panel2.SuspendLayout();
            this.topRightSplitContainer.SuspendLayout();
            this.groupBoxColumnTypes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvColumnTypes)).BeginInit();
            this.groupBoxPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Wczytane pliki:";
            // 
            // lbFiles
            // 
            this.lbFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFiles.FormattingEnabled = true;
            this.lbFiles.Location = new System.Drawing.Point(15, 25);
            this.lbFiles.Name = "lbFiles";
            this.lbFiles.Size = new System.Drawing.Size(217, 342);
            this.lbFiles.TabIndex = 1;
            this.lbFiles.SelectedIndexChanged += new System.EventHandler(this.lbFiles_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lblSelectedFile);
            this.groupBox1.Controls.Add(this.txtMetadataRows);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.numHeaderRow);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(273, 204);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Konfiguracja pliku";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(142, 116);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "(np. 1, 2, 5 lub 1-3, 5)";
            // 
            // lblSelectedFile
            // 
            this.lblSelectedFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSelectedFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblSelectedFile.Location = new System.Drawing.Point(6, 25);
            this.lblSelectedFile.Name = "lblSelectedFile";
            this.lblSelectedFile.Size = new System.Drawing.Size(261, 23);
            this.lblSelectedFile.TabIndex = 4;
            this.lblSelectedFile.Text = "[Wybierz plik]";
            this.lblSelectedFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtMetadataRows
            // 
            this.txtMetadataRows.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMetadataRows.Location = new System.Drawing.Point(145, 93);
            this.txtMetadataRows.Name = "txtMetadataRows";
            this.txtMetadataRows.Size = new System.Drawing.Size(122, 20);
            this.txtMetadataRows.TabIndex = 3;
            this.txtMetadataRows.TextChanged += new System.EventHandler(this.txtMetadataRows_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Wiersze metadanych:";
            // 
            // numHeaderRow
            // 
            this.numHeaderRow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numHeaderRow.Location = new System.Drawing.Point(145, 63);
            this.numHeaderRow.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numHeaderRow.Name = "numHeaderRow";
            this.numHeaderRow.Size = new System.Drawing.Size(122, 20);
            this.numHeaderRow.TabIndex = 1;
            this.numHeaderRow.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numHeaderRow.ValueChanged += new System.EventHandler(this.numHeaderRow_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Numer wiersza nagłówka:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 381);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(130, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Wybierz plik główny (Root):";
            // 
            // cmbRootFile
            // 
            this.cmbRootFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbRootFile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRootFile.FormattingEnabled = true;
            this.cmbRootFile.Location = new System.Drawing.Point(15, 397);
            this.cmbRootFile.Name = "cmbRootFile";
            this.cmbRootFile.Size = new System.Drawing.Size(217, 21);
            this.cmbRootFile.TabIndex = 4;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(74, 433);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(155, 433);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Anuluj";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.Controls.Add(this.leftPanel);
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.rightSplitContainer);
            this.mainSplitContainer.Size = new System.Drawing.Size(934, 471);
            this.mainSplitContainer.SplitterDistance = 245;
            this.mainSplitContainer.TabIndex = 7;
            // 
            // leftPanel
            // 
            this.leftPanel.Controls.Add(this.label1);
            this.leftPanel.Controls.Add(this.btnCancel);
            this.leftPanel.Controls.Add(this.lbFiles);
            this.leftPanel.Controls.Add(this.btnOk);
            this.leftPanel.Controls.Add(this.label4);
            this.leftPanel.Controls.Add(this.cmbRootFile);
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftPanel.Location = new System.Drawing.Point(0, 0);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Size = new System.Drawing.Size(245, 471);
            this.leftPanel.TabIndex = 0;
            // 
            // rightSplitContainer
            // 
            this.rightSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.rightSplitContainer.Name = "rightSplitContainer";
            this.rightSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // rightSplitContainer.Panel1
            // 
            this.rightSplitContainer.Panel1.Controls.Add(this.topRightSplitContainer);
            // 
            // rightSplitContainer.Panel2
            // 
            this.rightSplitContainer.Panel2.Controls.Add(this.groupBoxPreview);
            this.rightSplitContainer.Size = new System.Drawing.Size(685, 471);
            this.rightSplitContainer.SplitterDistance = 204;
            this.rightSplitContainer.TabIndex = 0;
            // 
            // topRightSplitContainer
            // 
            this.topRightSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topRightSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.topRightSplitContainer.Name = "topRightSplitContainer";
            // 
            // topRightSplitContainer.Panel1
            // 
            this.topRightSplitContainer.Panel1.Controls.Add(this.groupBox1);
            // 
            // topRightSplitContainer.Panel2
            // 
            this.topRightSplitContainer.Panel2.Controls.Add(this.groupBoxColumnTypes);
            this.topRightSplitContainer.Size = new System.Drawing.Size(685, 204);
            this.topRightSplitContainer.SplitterDistance = 273;
            this.topRightSplitContainer.TabIndex = 3;
            // 
            // groupBoxColumnTypes
            // 
            this.groupBoxColumnTypes.Controls.Add(this.dgvColumnTypes);
            this.groupBoxColumnTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxColumnTypes.Location = new System.Drawing.Point(0, 0);
            this.groupBoxColumnTypes.Name = "groupBoxColumnTypes";
            this.groupBoxColumnTypes.Size = new System.Drawing.Size(408, 204);
            this.groupBoxColumnTypes.TabIndex = 0;
            this.groupBoxColumnTypes.TabStop = false;
            this.groupBoxColumnTypes.Text = "Ustawienia Typów Kolumn";
            // 
            // dgvColumnTypes
            // 
            this.dgvColumnTypes.AllowUserToAddRows = false;
            this.dgvColumnTypes.AllowUserToDeleteRows = false;
            this.dgvColumnTypes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvColumnTypes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvColumnTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvColumnTypes.Location = new System.Drawing.Point(3, 16);
            this.dgvColumnTypes.Name = "dgvColumnTypes";
            this.dgvColumnTypes.RowHeadersVisible = false;
            this.dgvColumnTypes.Size = new System.Drawing.Size(402, 185);
            this.dgvColumnTypes.TabIndex = 0;
            this.dgvColumnTypes.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvColumnTypes_CellValueChanged);
            // 
            // groupBoxPreview
            // 
            this.groupBoxPreview.Controls.Add(this.dgvPreview);
            this.groupBoxPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxPreview.Location = new System.Drawing.Point(0, 0);
            this.groupBoxPreview.Name = "groupBoxPreview";
            this.groupBoxPreview.Size = new System.Drawing.Size(685, 263);
            this.groupBoxPreview.TabIndex = 0;
            this.groupBoxPreview.TabStop = false;
            this.groupBoxPreview.Text = "Podgląd Pliku CSV";
            // 
            // dgvPreview
            // 
            this.dgvPreview.AllowUserToAddRows = false;
            this.dgvPreview.AllowUserToDeleteRows = false;
            this.dgvPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPreview.Location = new System.Drawing.Point(3, 16);
            this.dgvPreview.Name = "dgvPreview";
            this.dgvPreview.ReadOnly = true;
            this.dgvPreview.RowHeadersWidth = 51;
            this.dgvPreview.Size = new System.Drawing.Size(679, 244);
            this.dgvPreview.TabIndex = 0;
            this.dgvPreview.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvPreview_CellFormatting);
            // 
            // ImportConfigurationDialog
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(934, 471);
            this.Controls.Add(this.mainSplitContainer);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 510);
            this.Name = "ImportConfigurationDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Konfiguracja importu CSV";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHeaderRow)).EndInit();
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.leftPanel.ResumeLayout(false);
            this.leftPanel.PerformLayout();
            this.rightSplitContainer.Panel1.ResumeLayout(false);
            this.rightSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rightSplitContainer)).EndInit();
            this.rightSplitContainer.ResumeLayout(false);
            this.topRightSplitContainer.Panel1.ResumeLayout(false);
            this.topRightSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.topRightSplitContainer)).EndInit();
            this.topRightSplitContainer.ResumeLayout(false);
            this.groupBoxColumnTypes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvColumnTypes)).EndInit();
            this.groupBoxPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreview)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbFiles;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblSelectedFile;
        private System.Windows.Forms.TextBox txtMetadataRows;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numHeaderRow;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbRootFile;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private System.Windows.Forms.Panel leftPanel;
        private System.Windows.Forms.SplitContainer rightSplitContainer;
        private System.Windows.Forms.GroupBox groupBoxPreview;
        private System.Windows.Forms.DataGridView dgvPreview;
        private System.Windows.Forms.SplitContainer topRightSplitContainer;
        private System.Windows.Forms.GroupBox groupBoxColumnTypes;
        private System.Windows.Forms.DataGridView dgvColumnTypes;
    }
}
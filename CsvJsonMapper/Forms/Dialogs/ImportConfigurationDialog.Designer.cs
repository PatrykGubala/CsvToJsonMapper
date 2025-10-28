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
            this.lblSelectedFile = new System.Windows.Forms.Label();
            this.txtMetadataRows = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numHeaderRow = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbRootFile = new System.Windows.Forms.ComboBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHeaderRow)).BeginInit();
            this.SuspendLayout();
         
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 0;
         
            this.lbFiles.FormattingEnabled = true;
            this.lbFiles.Location = new System.Drawing.Point(15, 25);
            this.lbFiles.Name = "lbFiles";
            this.lbFiles.Size = new System.Drawing.Size(193, 199);
            this.lbFiles.TabIndex = 1;
            this.lbFiles.SelectedIndexChanged += new System.EventHandler(this.lbFiles_SelectedIndexChanged);
           
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lblSelectedFile);
            this.groupBox1.Controls.Add(this.txtMetadataRows);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.numHeaderRow);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(214, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(258, 199);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Konfiguracja pliku";
           
            this.lblSelectedFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblSelectedFile.Location = new System.Drawing.Point(6, 25);
            this.lblSelectedFile.Name = "lblSelectedFile";
            this.lblSelectedFile.Size = new System.Drawing.Size(246, 23);
            this.lblSelectedFile.TabIndex = 4;
            this.lblSelectedFile.Text = "[Wybierz plik]";
            this.lblSelectedFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            this.txtMetadataRows.Location = new System.Drawing.Point(145, 93);
            this.txtMetadataRows.Name = "txtMetadataRows";
            this.txtMetadataRows.Size = new System.Drawing.Size(107, 20);
            this.txtMetadataRows.TabIndex = 3;
            this.txtMetadataRows.TextChanged += new System.EventHandler(this.txtMetadataRows_TextChanged);
            
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Wiersze metadanych:";
           
            this.numHeaderRow.Location = new System.Drawing.Point(145, 63);
            this.numHeaderRow.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numHeaderRow.Name = "numHeaderRow";
            this.numHeaderRow.Size = new System.Drawing.Size(107, 20);
            this.numHeaderRow.TabIndex = 1;
            this.numHeaderRow.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numHeaderRow.ValueChanged += new System.EventHandler(this.numHeaderRow_ValueChanged);
           
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Numer wiersza nagłówka:";
           
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 239);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(130, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Wybierz plik główny (Root):";
          
            this.cmbRootFile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRootFile.FormattingEnabled = true;
            this.cmbRootFile.Location = new System.Drawing.Point(148, 236);
            this.cmbRootFile.Name = "cmbRootFile";
            this.cmbRootFile.Size = new System.Drawing.Size(324, 21);
            this.cmbRootFile.TabIndex = 4;
           
            this.btnOk.Location = new System.Drawing.Point(316, 272);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
           
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(397, 272);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Anuluj";
            this.btnCancel.UseVisualStyleBackColor = true;
            
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(142, 116);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "(np. 1, 2, 5 lub 1-3, 5)";
            
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(484, 307);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.cmbRootFile);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lbFiles);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportConfigurationDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Konfiguracja importu CSV";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHeaderRow)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}
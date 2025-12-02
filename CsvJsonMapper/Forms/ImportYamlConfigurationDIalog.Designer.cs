namespace CsvJsonMapper.Forms.Dialogs
{
    partial class ImportYamlConfigDialog
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
            this._dgvFiles = new System.Windows.Forms.DataGridView();
            this._btnImport = new System.Windows.Forms.Button();
            this._btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._dgvFiles)).BeginInit();
            this.SuspendLayout();
            // 
            // _dgvFiles
            // 
            this._dgvFiles.AllowUserToAddRows = false;
            this._dgvFiles.AllowUserToDeleteRows = false;
            this._dgvFiles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._dgvFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._dgvFiles.Dock = System.Windows.Forms.DockStyle.Top;
            this._dgvFiles.Location = new System.Drawing.Point(0, 0);
            this._dgvFiles.MultiSelect = false;
            this._dgvFiles.Name = "_dgvFiles";
            this._dgvFiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._dgvFiles.Size = new System.Drawing.Size(800, 300);
            this._dgvFiles.TabIndex = 0;
            // 
            // _btnImport
            // 
            this._btnImport.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnImport.Location = new System.Drawing.Point(600, 320);
            this._btnImport.Name = "_btnImport";
            this._btnImport.Size = new System.Drawing.Size(80, 30);
            this._btnImport.TabIndex = 1;
            this._btnImport.Text = "Importuj";
            this._btnImport.UseVisualStyleBackColor = true;
            this._btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // _btnCancel
            // 
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point(690, 320);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(80, 30);
            this._btnCancel.TabIndex = 2;
            this._btnCancel.Text = "Anuluj";
            this._btnCancel.UseVisualStyleBackColor = true;
            // 
            // ImportYamlConfigDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 362);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._btnImport);
            this.Controls.Add(this._dgvFiles);
            this.Name = "ImportYamlConfigDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Konfiguracja importu YAML";
            ((System.ComponentModel.ISupportInitialize)(this._dgvFiles)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView _dgvFiles;
        private System.Windows.Forms.Button _btnImport;
        private System.Windows.Forms.Button _btnCancel;
    }
}
namespace CsvJsonMapper.Forms.Dialogs
{
    partial class AddEditRelationDialog
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
            this.txtRelationName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbParentKey = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbParentFile = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblChildFkWarning = new System.Windows.Forms.Label();
            this.lbChildKey = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbChildFile = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbRelationType = new System.Windows.Forms.ComboBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nazwa relacji:";
            // 
            // txtRelationName
            // 
            this.txtRelationName.Location = new System.Drawing.Point(95, 12);
            this.txtRelationName.Name = "txtRelationName";
            this.txtRelationName.Size = new System.Drawing.Size(477, 20);
            this.txtRelationName.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbParentKey);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbParentFile);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(15, 47);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(557, 155);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rodzic (Parent)";
            // 
            // lbParentKey
            // 
            this.lbParentKey.FormattingEnabled = true;
            this.lbParentKey.Location = new System.Drawing.Point(138, 59);
            this.lbParentKey.Name = "lbParentKey";
            this.lbParentKey.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbParentKey.Size = new System.Drawing.Size(404, 82);
            this.lbParentKey.TabIndex = 1;
            this.lbParentKey.SelectedIndexChanged += new System.EventHandler(this.lbParentKey_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Klucz nadrzędny (PK):";
            // 
            // cmbParentFile
            // 
            this.cmbParentFile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbParentFile.FormattingEnabled = true;
            this.cmbParentFile.Location = new System.Drawing.Point(138, 25);
            this.cmbParentFile.Name = "cmbParentFile";
            this.cmbParentFile.Size = new System.Drawing.Size(404, 21);
            this.cmbParentFile.TabIndex = 0;
            this.cmbParentFile.SelectedIndexChanged += new System.EventHandler(this.cmbParentFile_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Plik nadrzędny:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblChildFkWarning);
            this.groupBox2.Controls.Add(this.lbChildKey);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.cmbChildFile);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(15, 208);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(557, 155);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Dziecko (Child)";
            // 
            // lblChildFkWarning
            // 
            this.lblChildFkWarning.AutoSize = true;
            this.lblChildFkWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblChildFkWarning.Location = new System.Drawing.Point(267, 131);
            this.lblChildFkWarning.Name = "lblChildFkWarning";
            this.lblChildFkWarning.Size = new System.Drawing.Size(262, 13);
            this.lblChildFkWarning.TabIndex = 4;
            this.lblChildFkWarning.Text = "(Liczba kolumn musi zgadzać się z liczbą kolumn PK)";
            // 
            // lbChildKey
            // 
            this.lbChildKey.FormattingEnabled = true;
            this.lbChildKey.Location = new System.Drawing.Point(138, 59);
            this.lbChildKey.Name = "lbChildKey";
            this.lbChildKey.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbChildKey.Size = new System.Drawing.Size(404, 69);
            this.lbChildKey.TabIndex = 1;
            this.lbChildKey.SelectedIndexChanged += new System.EventHandler(this.lbChildKey_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Klucz obcy (FK):";
            // 
            // cmbChildFile
            // 
            this.cmbChildFile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChildFile.FormattingEnabled = true;
            this.cmbChildFile.Location = new System.Drawing.Point(138, 25);
            this.cmbChildFile.Name = "cmbChildFile";
            this.cmbChildFile.Size = new System.Drawing.Size(404, 21);
            this.cmbChildFile.TabIndex = 0;
            this.cmbChildFile.SelectedIndexChanged += new System.EventHandler(this.cmbChildFile_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Plik podrzędny:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 378);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Typ relacji:";
            // 
            // cmbRelationType
            // 
            this.cmbRelationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRelationType.FormattingEnabled = true;
            this.cmbRelationType.Location = new System.Drawing.Point(95, 375);
            this.cmbRelationType.Name = "cmbRelationType";
            this.cmbRelationType.Size = new System.Drawing.Size(183, 21);
            this.cmbRelationType.TabIndex = 3;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(416, 412);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(497, 412);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Anuluj";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // AddEditRelationDialog
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(584, 447);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.cmbRelationType);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtRelationName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddEditRelationDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Dodaj/Edytuj Relację";
            this.Load += new System.EventHandler(this.AddEditRelationDialog_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRelationName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbParentFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lbChildKey;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbChildFile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbRelationType;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblChildFkWarning;
        private System.Windows.Forms.ListBox lbParentKey;
    }
}
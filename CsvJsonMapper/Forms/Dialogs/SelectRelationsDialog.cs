using CsvJsonMapper.Models.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CsvJsonMapper.Forms.Dialogs
{
    public partial class SelectRelationDialog : Form
    {
        public Relation SelectedRelation { get; private set; }

        public SelectRelationDialog(List<Relation> relations, RelationType type)
        {
            InitializeComponent();
            var availableRelations = relations.Where(r => r.Type == type).ToList();
            lbRelations.DataSource = availableRelations;
            lbRelations.DisplayMember = "Description";
        }

        private void SelectRelationDialog_Load(object sender, EventArgs e)
        {
            if (lbRelations.Items.Count == 0)
            {
                MessageBox.Show($"Brak zdefiniowanych relacji typu {lbRelations.Tag}.", "Brak Relacji");
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (lbRelations.SelectedItem == null)
            {
                MessageBox.Show("Musisz wybrać relację.", "Błąd");
                return;
            }
            SelectedRelation = (Relation)lbRelations.SelectedItem;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
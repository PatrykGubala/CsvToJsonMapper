using CsvJsonMapper.Models;
using CsvJsonMapper.Models.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CsvJsonMapper.Forms.Dialogs
{
    public partial class ManageRelationsDialog : Form
    {
        private List<Relation> _relations;
        private List<CsvSourceFile> _files;

        public ManageRelationsDialog(List<Relation> relations, List<CsvSourceFile> files)
        {
            InitializeComponent();
            _relations = relations;
            _files = files;
        }

        private void ManageRelationsDialog_Load(object sender, EventArgs e)
        {
            RefreshRelationList();
        }

        private void RefreshRelationList()
        {
            lvRelations.Items.Clear();
            foreach (var relation in _relations)
            {
                var lvi = new ListViewItem(relation.Name);
                lvi.SubItems.Add(relation.Description);
                lvi.Tag = relation;
                lvRelations.Items.Add(lvi);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var dialog = new AddEditRelationDialog(_files))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _relations.Add(dialog.Relation);
                    RefreshRelationList();
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lvRelations.SelectedItems.Count == 0) return;
            var relationToEdit = (Relation)lvRelations.SelectedItems[0].Tag;

            using (var dialog = new AddEditRelationDialog(_files, relationToEdit))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    RefreshRelationList();
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lvRelations.SelectedItems.Count == 0) return;
            var relationToRemove = (Relation)lvRelations.SelectedItems[0].Tag;

            if (MessageBox.Show($"Czy na pewno chcesz usunąć relację '{relationToRemove.Name}'?", "Potwierdzenie", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _relations.Remove(relationToRemove);
                RefreshRelationList();
            }
        }
    }
}
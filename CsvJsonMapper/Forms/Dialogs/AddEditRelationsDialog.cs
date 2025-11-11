using CsvJsonMapper.Models;
using CsvJsonMapper.Models.Mapping;


namespace CsvJsonMapper.Forms.Dialogs
{
    public partial class AddEditRelationDialog : Form
    {
        private List<CsvSourceFile> _files;
        private CsvSourceFile _selectedParent => cmbParentFile.SelectedItem as CsvSourceFile;
        private CsvSourceFile _selectedChild => cmbChildFile.SelectedItem as CsvSourceFile;

        public Relation Relation { get; private set; }

        public AddEditRelationDialog(List<CsvSourceFile> files, Relation relationToEdit = null)
        {
            InitializeComponent();
            _files = files;
            Relation = relationToEdit ?? new Relation();
        }

        private void AddEditRelationDialog_Load(object sender, EventArgs e)
        {
            cmbParentFile.Items.AddRange(_files.ToArray());
            cmbChildFile.Items.AddRange(_files.ToArray());
            cmbRelationType.Items.AddRange(Enum.GetNames(typeof(RelationType)));

            if (!string.IsNullOrEmpty(Relation.Name))
            {
                txtRelationName.Text = Relation.Name;
                cmbParentFile.SelectedItem = _files.FirstOrDefault(f => f.FileName == Relation.ParentFileId);
                cmbChildFile.SelectedItem = _files.FirstOrDefault(f => f.FileName == Relation.ChildFileId);
                cmbRelationType.SelectedItem = Relation.Type.ToString();

                PopulateKeys(lbParentKey, _selectedParent, Relation.ParentKeyColumns);
                PopulateKeys(lbChildKey, _selectedChild, Relation.ChildKeyColumns);
            }
            else
            {
                cmbRelationType.SelectedIndex = 0;
            }
        }

        private void PopulateKeys(ListBox listBox, CsvSourceFile file, List<string> selectedKeys)
        {
            if (file == null) return;
            listBox.Items.Clear();
            listBox.Items.AddRange(file.Headers.ToArray());
            foreach (string key in selectedKeys)
            {
                int index = listBox.Items.IndexOf(key);
                if (index >= 0)
                {
                    listBox.SetSelected(index, true);
                }
            }
        }

        private void cmbParentFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_selectedParent == null)
            {
                lbParentKey.Items.Clear();
                return;
            }
            PopulateKeys(lbParentKey, _selectedParent, new List<string>());
            UpdateDefaultRelationName();
            ValidateKeyCounts();
        }

        private void cmbChildFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_selectedChild == null)
            {
                lbChildKey.Items.Clear();
                return;
            }
            PopulateKeys(lbChildKey, _selectedChild, new List<string>());
            UpdateDefaultRelationName();
            ValidateKeyCounts();
        }

        private void UpdateDefaultRelationName()
        {
            if (string.IsNullOrWhiteSpace(txtRelationName.Text))
            {
                if (_selectedParent != null && _selectedChild != null)
                {
                    txtRelationName.Text = $"{_selectedParent.FileName}_to_{_selectedChild.FileName}";
                }
            }
        }

        private void ValidateKeyCounts()
        {
            if (_selectedParent == null)
            {
                lblChildFkWarning.Visible = false;
                return;
            }

            int pkCount = lbParentKey.SelectedItems.Count;
            int fkCount = lbChildKey.SelectedItems.Count;

            lblChildFkWarning.Visible = (pkCount > 0 && fkCount > 0 && pkCount != fkCount);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRelationName.Text))
            {
                if (_selectedParent != null && _selectedChild != null)
                {
                    Relation.Name = $"{_selectedParent.FileName}_to_{_selectedChild.FileName}";
                }
                else
                {
                    Relation.Name = $"Relacja_{Guid.NewGuid().ToString().Substring(0, 4)}";
                }
            }
            else
            {
                Relation.Name = txtRelationName.Text;
            }

            if (_selectedParent == null || _selectedChild == null)
            {
                MessageBox.Show("Plik nadrzędny i podrzędny muszą być wybrane.", "Błąd Walidacji");
                return;
            }
            if (lbParentKey.SelectedItems.Count == 0)
            {
                MessageBox.Show("Klucz nadrzędny (PK) musi być wybrany.", "Błąd Walidacji");
                return;
            }
            if (lbChildKey.SelectedItems.Count == 0)
            {
                MessageBox.Show("Klucz obcy (FK) musi być wybrany.", "Błąd Walidacji");
                return;
            }
            if (lbParentKey.SelectedItems.Count != lbChildKey.SelectedItems.Count)
            {
                MessageBox.Show("Liczba kolumn klucza nadrzędnego i obcego musi być taka sama.", "Błąd Walidacji");
                return;
            }

            Relation.ParentFileId = _selectedParent.FileName;
            Relation.ParentKeyColumns = lbParentKey.SelectedItems.Cast<string>().ToList();
            Relation.ChildFileId = _selectedChild.FileName;
            Relation.ChildKeyColumns = lbChildKey.SelectedItems.Cast<string>().ToList();
            Relation.Type = (RelationType)Enum.Parse(typeof(RelationType), cmbRelationType.SelectedItem.ToString());

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void lbParentKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateKeyCounts();
        }

        private void lbChildKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateKeyCounts();
        }
    }
}
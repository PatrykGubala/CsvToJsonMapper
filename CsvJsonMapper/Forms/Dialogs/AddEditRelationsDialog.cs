using CsvJsonMapper.Models;
using CsvJsonMapper.Models.Mapping;
using System.Data;


namespace CsvJsonMapper.Forms.Dialogs
{
    public partial class AddEditRelationDialog : Form
    {
        private List<CsvSourceFile> _files;
        private CsvSourceFile _selectedParent => cmbParentFile.SelectedItem as CsvSourceFile;
        private CsvSourceFile _selectedChild => cmbChildFile.SelectedItem as CsvSourceFile;
        private CsvSourceFile _rootFile;

        public Relation Relation { get; private set; }

        public AddEditRelationDialog(List<CsvSourceFile> files, CsvSourceFile rootFile, Relation relationToEdit = null)
        {
            InitializeComponent();
            _files = files;
            _rootFile = rootFile;
            Relation = relationToEdit ?? new Relation { Id = Guid.NewGuid() };
        }

        private void AddEditRelationDialog_Load(object sender, EventArgs e)
        {
            cmbParentFile.Items.AddRange(_files.ToArray());
            cmbChildFile.Items.AddRange(_files.ToArray());
            cmbRelationType.Items.AddRange(Enum.GetNames(typeof(RelationType)));

            if (Relation.Id != Guid.Empty && !string.IsNullOrEmpty(Relation.Name))
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
                cmbRelationType.SelectedIndex = 1; 
                
                if (_rootFile != null)
                {
                    cmbParentFile.SelectedItem = _rootFile;
                }
            }
            cmbRelationType.Enabled = false;
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
            ValidateSelections();
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
            ValidateSelections();
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

        private bool AreKeysUnique(CsvSourceFile file, List<string> keyColumns)
        {
            if (file == null || keyColumns.Count == 0) return true;

            var keyHashSet = new HashSet<string>();
            foreach (DataRow row in file.ProcessedData.Rows)
            {
                string compositeKey = string.Join("|", keyColumns.Select(col => row[col]?.ToString() ?? ""));
                if (!keyHashSet.Add(compositeKey))
                {
                    return false;
                }
            }
            return true;
        }

        private void ValidateSelections()
        {
            if (_selectedParent == null)
            {
                lblChildFkWarning.Visible = false;
                lblParentPkWarning.Visible = false;
                return;
            }

            int pkCount = lbParentKey.SelectedItems.Count;
            int fkCount = lbChildKey.SelectedItems.Count;

            lblChildFkWarning.Visible = (pkCount > 0 && fkCount > 0 && pkCount != fkCount);

            if (pkCount > 0)
            {
                var parentKeys = lbParentKey.SelectedItems.Cast<string>().ToList();
                bool areUnique = AreKeysUnique(_selectedParent, parentKeys);
                lblParentPkWarning.Visible = !areUnique;
            }
            else
            {
                lblParentPkWarning.Visible = false;
            }

            if (fkCount > 0 && _selectedChild != null)
            {
                var childKeys = lbChildKey.SelectedItems.Cast<string>().ToList();
                bool isChildKeyUnique = AreKeysUnique(_selectedChild, childKeys);

                if (isChildKeyUnique)
                {
                    cmbRelationType.SelectedItem = RelationType.OneToOne.ToString();
                }
                else
                {
                    cmbRelationType.SelectedItem = RelationType.OneToMany.ToString();
                }
            }
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
                    Relation.Name = $"Relacja_{Relation.Id.ToString().Substring(0, 4)}";
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

            var parentKeys = lbParentKey.SelectedItems.Cast<string>().ToList();
            if (parentKeys.Count == 0)
            {
                MessageBox.Show("Klucz nadrzędny (PK) musi być wybrany.", "Błąd Walidacji");
                return;
            }

            var childKeys = lbChildKey.SelectedItems.Cast<string>().ToList();
            if (childKeys.Count == 0)
            {
                MessageBox.Show("Klucz obcy FK musi być wybrany.", "Błąd Walidacji");
                return;
            }
            if (parentKeys.Count != childKeys.Count)
            {
                MessageBox.Show("Liczba kolumn klucza nadrzędnego i obcego musi być taka sama.", "Błąd Walidacji");
                return;
            }

            Relation.ParentFileId = _selectedParent.FileName;
            Relation.ParentKeyColumns = parentKeys;
            Relation.ChildFileId = _selectedChild.FileName;
            Relation.ChildKeyColumns = childKeys;
            Relation.Type = (RelationType)Enum.Parse(typeof(RelationType), cmbRelationType.SelectedItem.ToString());

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void lbParentKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateSelections();
        }

        private void lbChildKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateSelections();
        }
    }
}
using CsvJsonMapper.Models;
using CsvJsonMapper.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CsvJsonMapper.Forms.Dialogs
{
    public partial class ImportConfigurationDialog : Form
    {
        private Dictionary<string, FileConfig> _fileConfigs;
        private Dictionary<string, CsvSourceFile> _rawFiles;
        private string[] _filePaths;
        private CsvParsingService _parsingService;
        private CsvSourceFile _currentFile;
        private bool _isUpdatingTypes = false;

        public string RootFilePath { get; private set; }
        public Dictionary<string, FileConfig> Configurations { get; private set; }

        public ImportConfigurationDialog(string[] filePaths, CsvParsingService parsingService)
        {
            InitializeComponent();
            this.dgvColumnTypes.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvColumnTypes_CurrentCellDirtyStateChanged);
            this.dgvColumnTypes.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvColumnTypes_CellClick);
            this.dgvPreview.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dgvPreview_SortCompare);

            _filePaths = filePaths;
            _parsingService = parsingService;
            _fileConfigs = new Dictionary<string, FileConfig>();
            _rawFiles = new Dictionary<string, CsvSourceFile>();

            foreach (var path in filePaths)
            {
                string fileName = Path.GetFileName(path);
                _fileConfigs.Add(path, new FileConfig());
                
                try
                {
                    var rawFile = _parsingService.LoadRawCsv(path);
                    _rawFiles.Add(path, rawFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd wczytywania pliku {fileName} do podglądu: {ex.Message}");
                    continue;
                }

                lbFiles.Items.Add(fileName);
                cmbRootFile.Items.Add(fileName);
            }

            if (lbFiles.Items.Count > 0)
            {
                lbFiles.SelectedIndex = 0;
                cmbRootFile.SelectedIndex = 0;
            }
        }

        private void lbFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbFiles.SelectedIndex == -1) return;

            string selectedFileName = lbFiles.SelectedItem.ToString();
            string selectedPath = _filePaths.First(p => Path.GetFileName(p) == selectedFileName);

            if (!_rawFiles.ContainsKey(selectedPath)) return;

            _currentFile = _rawFiles[selectedPath];
            var config = _fileConfigs[selectedPath];
            
            numHeaderRow.Value = config.HeaderRow;
            txtMetadataRows.Text = config.MetadataRows;
            lblSelectedFile.Text = selectedFileName;

            dgvPreview.DataSource = _currentFile.RawData;
            foreach (DataGridViewColumn col in dgvPreview.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Programmatic;
            }
            
            UpdateFileProcessing();
        }

        private void UpdateFileProcessing()
        {
            if (_currentFile == null) return;

            var config = GetCurrentFileConfig();
            _currentFile.HeaderRowIndex = config.HeaderRow - 1;
            _currentFile.MetadataRowIndices = ParseMetadataRows(config.MetadataRows);

            _parsingService.ProcessData(_currentFile);

            PopulateColumnTypesGrid(config);
            dgvPreview.Invalidate();
        }
        
        private void PopulateColumnTypesGrid(FileConfig config)
        {
            _isUpdatingTypes = true;
            dgvColumnTypes.Rows.Clear();
            dgvColumnTypes.Columns.Clear();

            var colName = new DataGridViewTextBoxColumn
            {
                Name = "ColumnName",
                HeaderText = "Kolumna",
                ReadOnly = true,
                FillWeight = 60
            };

            var colType = new DataGridViewComboBoxColumn
            {
                Name = "ColumnType",
                HeaderText = "Typ Danych",
                DataSource = new[] { "string", "int", "double" },
                FillWeight = 40
            };

            dgvColumnTypes.Columns.AddRange(colName, colType);

            if (_currentFile == null) 
            {
                _isUpdatingTypes = false;
                return;
            }

            foreach(var header in _currentFile.Headers)
            {
                string detectedType = _currentFile.DetectedColumnTypes.ContainsKey(header) ? _currentFile.DetectedColumnTypes[header] : "string";
                string overrideType = config.ColumnTypeOverrides.ContainsKey(header) ? config.ColumnTypeOverrides[header] : detectedType;
                
                dgvColumnTypes.Rows.Add(header, overrideType);
            }
            _isUpdatingTypes = false;
        }

        private void SaveCurrentFileConfig()
        {
            if (lbFiles.SelectedIndex == -1) return;

            string selectedFileName = lbFiles.SelectedItem.ToString();
            string selectedPath = _filePaths.First(p => Path.GetFileName(p) == selectedFileName);

            var config = _fileConfigs[selectedPath];
            config.HeaderRow = (int)numHeaderRow.Value;
            config.MetadataRows = txtMetadataRows.Text;
        }
        
        private FileConfig GetCurrentFileConfig()
        {
            if (lbFiles.SelectedIndex == -1) return null;
            string selectedFileName = lbFiles.SelectedItem.ToString();
            string selectedPath = _filePaths.First(p => Path.GetFileName(p) == selectedFileName);
            return _fileConfigs[selectedPath];
        }

        private List<int> ParseMetadataRows(string metadataText)
        {
            var indices = new List<int>();
            if (string.IsNullOrWhiteSpace(metadataText))
            {
                return indices;
            }

            try
            {
                var parts = metadataText.Split(',');
                foreach (var part in parts)
                {
                    if (part.Contains('-'))
                    {
                        var range = part.Split('-');
                        int start = int.Parse(range[0].Trim()) - 1;
                        int end = int.Parse(range[1].Trim()) - 1;
                        indices.AddRange(Enumerable.Range(start, end - start + 1));
                    }
                    else
                    {
                        indices.Add(int.Parse(part.Trim()) - 1);
                    }
                }
                return indices.Distinct().OrderBy(i => i).ToList();
            }
            catch
            {
                return new List<int>();
            }
        }

        private void numHeaderRow_ValueChanged(object sender, EventArgs e)
        {
            SaveCurrentFileConfig();
            UpdateFileProcessing();
        }

        private void txtMetadataRows_TextChanged(object sender, EventArgs e)
        {
            SaveCurrentFileConfig();
            UpdateFileProcessing();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SaveCurrentFileConfig();

            if (cmbRootFile.SelectedIndex == -1)
            {
                MessageBox.Show("Musisz wybrać plik główny (Root).", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string rootFileName = cmbRootFile.SelectedItem.ToString();
            RootFilePath = _filePaths.First(p => Path.GetFileName(p) == rootFileName);
            Configurations = _fileConfigs;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void dgvPreview_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (_currentFile == null) return;

            if (e.RowIndex == _currentFile.HeaderRowIndex)
            {
                e.CellStyle.BackColor = Color.LightSkyBlue;
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
            }
            else if (_currentFile.MetadataRowIndices.Contains(e.RowIndex))
            {
                e.CellStyle.BackColor = Color.LightGray;
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Italic);
            }
            else
            {
                e.CellStyle.BackColor = SystemColors.Window;
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Regular);
            }
        }

        private void dgvColumnTypes_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (_isUpdatingTypes || e.RowIndex < 0 || e.ColumnIndex != 1) return;

            var config = GetCurrentFileConfig();
            if (config == null) return;

            string columnName = dgvColumnTypes.Rows[e.RowIndex].Cells[0].Value.ToString();
            string newType = dgvColumnTypes.Rows[e.RowIndex].Cells[1].Value.ToString();

            config.ColumnTypeOverrides[columnName] = newType;
        }

        private void dgvColumnTypes_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvColumnTypes.IsCurrentCellDirty)
            {
                dgvColumnTypes.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvPreview_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (_currentFile == null || _currentFile.Headers == null || e.Column.Index >= _currentFile.Headers.Count)
            {
                e.SortResult = string.Compare(e.CellValue1?.ToString(), e.CellValue2?.ToString());
                e.Handled = true;
                return;
            }

            try
            {
                string headerName = _currentFile.Headers[e.Column.Index];
                if (!_currentFile.DetectedColumnTypes.ContainsKey(headerName))
                {
                    e.SortResult = string.Compare(e.CellValue1?.ToString(), e.CellValue2?.ToString());
                    e.Handled = true;
                    return;
                }

                string type = _currentFile.DetectedColumnTypes[headerName];
                e.SortResult = SortCompareHelper.Compare(e.CellValue1, e.CellValue2, type);
            }
            catch
            {
                e.SortResult = string.Compare(e.CellValue1?.ToString(), e.CellValue2?.ToString());
            }

            e.Handled = true;
        }

        private void dgvColumnTypes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 1)
            {
                dgvColumnTypes.BeginEdit(true);
                if (dgvColumnTypes.EditingControl is DataGridViewComboBoxEditingControl editingControl)
                {
                    editingControl.DroppedDown = true;
                }
            }
        }
    }
}
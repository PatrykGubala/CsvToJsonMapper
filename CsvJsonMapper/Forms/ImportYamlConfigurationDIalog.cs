using CsvJsonMapper.Models.Configuration;
using CsvJsonMapper.Services;


namespace CsvJsonMapper.Forms.Dialogs
{
    public partial class ImportYamlConfigDialog : Form
    {
        private ProjectConfiguration _config;
        private CsvParsingService _parsingService = new CsvParsingService();

        public ImportYamlConfigDialog(ProjectConfiguration config)
        {
            InitializeComponent();
            _config = config;
            InitializeGridColumns();
            LoadFilesToGrid();
            
            _dgvFiles.CellContentClick += DgvFiles_CellContentClick;
        }

        private void InitializeGridColumns()
        {
            var colFileName = new DataGridViewTextBoxColumn 
            { 
                HeaderText = "Nazwa w konfiguracji (ID)", 
                ReadOnly = true, 
                FillWeight = 30 
            };
            var colFilePath = new DataGridViewTextBoxColumn 
            { 
                HeaderText = "Ścieżka do pliku", 
                ReadOnly = true, 
                FillWeight = 60 
            };
            var colBtn = new DataGridViewButtonColumn 
            { 
                HeaderText = "Zmień", 
                Text = "...", 
                UseColumnTextForButtonValue = true, 
                FillWeight = 10 
            };

            _dgvFiles.Columns.AddRange(colFileName, colFilePath, colBtn);
        }

        private void LoadFilesToGrid()
        {
            if (_config.Files == null) return;

            foreach (var fileDef in _config.Files)
            {
                int rowIndex = _dgvFiles.Rows.Add(fileDef.FileName, fileDef.FilePath);
                
                if (!File.Exists(fileDef.FilePath))
                {
                    _dgvFiles.Rows[rowIndex].Cells[1].Style.BackColor = Color.LightPink;
                    _dgvFiles.Rows[rowIndex].Cells[1].ToolTipText = "Plik nie istnieje!";
                }
            }
        }

        private void DgvFiles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 2)
            {
                var fileDef = _config.Files[e.RowIndex];
                
                using (var ofd = new OpenFileDialog())
                {
                    ofd.Title = $"Wybierz plik dla: {fileDef.FileName}";
                    ofd.Filter = "Pliki CSV (*.csv)|*.csv|Wszystkie pliki (*.*)|*.*";
                    
                    if (!string.IsNullOrEmpty(fileDef.FilePath))
                    {
                        try 
                        { 
                            string dir = Path.GetDirectoryName(fileDef.FilePath);
                            if (Directory.Exists(dir)) ofd.InitialDirectory = dir;
                        } catch { }
                    }

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        fileDef.FilePath = ofd.FileName;
                        _dgvFiles.Rows[e.RowIndex].Cells[1].Value = ofd.FileName;
                        _dgvFiles.Rows[e.RowIndex].Cells[1].Style.BackColor = Color.LightGreen;
                        _dgvFiles.Rows[e.RowIndex].Cells[1].ToolTipText = "";
                    }
                }
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            foreach (var fileDef in _config.Files)
            {
                if (!File.Exists(fileDef.FilePath))
                {
                    MessageBox.Show($"Plik nie istnieje: {fileDef.FilePath}\nProszę wskazać poprawną ścieżkę.", "Błąd walidacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.None; 
                    return;
                }

                try
                {
                    _parsingService.ValidateFileStructure(fileDef.FilePath, fileDef.HeaderRowIndex);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd walidacji pliku {fileDef.FileName}:\n{ex.Message}", "Błąd walidacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.None;
                    return;
                }
            }
        }
    }
}
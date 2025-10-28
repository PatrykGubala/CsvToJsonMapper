using CsvJsonMapper.Forms.Dialogs;
using CsvJsonMapper.Models;
using CsvJsonMapper.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CsvJsonMapper.Forms
{
    public partial class MainForm : Form
    {
        private List<CsvSourceFile> _loadedFiles;
        private CsvParsingService _parsingService;
        private Font _rootNodeFont;

        public MainForm()
        {
            InitializeComponent();
            _loadedFiles = new List<CsvSourceFile>();
            _parsingService = new CsvParsingService();
            _rootNodeFont = new Font(tvSourceFiles.Font, FontStyle.Bold);
        }

        private void openCsvToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog
            {
                Filter = "Pliki CSV (*.csv)|*.csv|Wszystkie pliki (*.*)|*.*",
                Title = "Wybierz pliki CSV do załadowania",
                Multiselect = true
            })
            {
                if (ofd.ShowDialog() != DialogResult.OK) return;

                using (var dialog = new ImportConfigurationDialog(ofd.FileNames))
                {
                    if (dialog.ShowDialog() != DialogResult.OK) return;

                    _loadedFiles.Clear();
                    foreach (string filePath in ofd.FileNames)
                    {
                        try
                        {
                            var config = dialog.Configurations[filePath];
                            var csvFile = _parsingService.LoadRawCsv(filePath);
                            
                            csvFile.HeaderRowIndex = config.HeaderRow - 1;
                            csvFile.MetadataRowIndices = ParseMetadataRows(config.MetadataRows);
                            csvFile.IsRootFile = (filePath == dialog.RootFilePath);

                            _parsingService.ProcessData(csvFile);

                            _loadedFiles.Add(csvFile);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Błąd podczas wczytywania pliku {filePath}: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    UpdateSourceTreeView();
                    UpdateCsvViewsTabControl();
                    UpdateJsonPreview();
                    lblStatus.Text = $"Wczytano {_loadedFiles.Count} plików.";
                }
            }
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
                MessageBox.Show($"Nieprawidłowy format wierszy metadanych: {metadataText}. Zostaną zignorowane.", "Ostrzeżenie");
                return new List<int>();
            }
        }

        private void UpdateSourceTreeView()
        {
            tvSourceFiles.Nodes.Clear();
            tvJsonStructure.Nodes.Clear();
            var rootJsonNode = tvJsonStructure.Nodes.Add("Root (Obiekt)");

            foreach (var file in _loadedFiles)
            {
                var fileNode = new TreeNode(file.FileName) { Tag = file };
                if (file.IsRootFile)
                {
                    fileNode.NodeFont = _rootNodeFont;
                    fileNode.Text += " (Root)";
                }
                
                foreach (var header in file.Headers)
                {
                    var headerNode = fileNode.Nodes.Add(header);
                    if (file.IsRootFile)
                    {
                        rootJsonNode.Nodes.Add(header, header);
                    }
                }
                tvSourceFiles.Nodes.Add(fileNode);
            }
            tvSourceFiles.ExpandAll();
            tvJsonStructure.ExpandAll();
        }

        private void UpdateCsvViewsTabControl()
        {
            tabControlCsvViews.TabPages.Clear();
            foreach (var file in _loadedFiles)
            {
                var tabPage = new TabPage(file.FileName) { Tag = file };
                var dgv = new DataGridView
                {
                    DataSource = file.RawData,
                    Dock = DockStyle.Fill,
                    ReadOnly = true,
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    AllowUserToResizeRows = false,
                    RowHeadersVisible = true,
                    SelectionMode = DataGridViewSelectionMode.RowHeaderSelect,
                    Tag = file
                };

                dgv.DataBindingComplete += Dgv_DataBindingComplete;
                tabPage.Controls.Add(dgv);
                tabControlCsvViews.TabPages.Add(tabPage);
            }
        }

        private void Dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (dgv == null) return;
            var file = dgv.Tag as CsvSourceFile;
            if (file == null) return;
            
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;

                if (row.Index == file.HeaderRowIndex)
                {
                    row.DefaultCellStyle.BackColor = Color.LightSkyBlue;
                }
                else if (file.MetadataRowIndices.Contains(row.Index))
                {
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                }
            }
        }

        private void UpdateJsonPreview()
        {
            var rootFile = _loadedFiles.FirstOrDefault(f => f.IsRootFile);
            if (rootFile == null)
            {
                rtbJsonPreview.Text = string.Empty;
                return;
            }
            
            string json = JsonConvert.SerializeObject(rootFile.ProcessedData, Formatting.Indented);
            rtbJsonPreview.Text = json;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
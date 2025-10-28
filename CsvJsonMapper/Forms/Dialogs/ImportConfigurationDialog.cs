using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CsvJsonMapper.Forms.Dialogs
{
    public class FileConfig
    {
        public int HeaderRow { get; set; } = 1;
        public string MetadataRows { get; set; } = "";
    }

    public partial class ImportConfigurationDialog : Form
    {
        private Dictionary<string, FileConfig> _fileConfigs;
        private string[] _filePaths;

        public string RootFilePath { get; private set; }
        public Dictionary<string, FileConfig> Configurations { get; private set; }

        public ImportConfigurationDialog(string[] filePaths)
        {
            InitializeComponent();
            _filePaths = filePaths;
            _fileConfigs = new Dictionary<string, FileConfig>();

            foreach (var path in filePaths)
            {
                string fileName = Path.GetFileName(path);
                _fileConfigs.Add(path, new FileConfig());
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

            var config = _fileConfigs[selectedPath];
            numHeaderRow.Value = config.HeaderRow;
            txtMetadataRows.Text = config.MetadataRows;
            lblSelectedFile.Text = selectedFileName;
        }

        private void SaveCurrentFileConfig()
        {
            if (lbFiles.SelectedIndex == -1) return;

            string selectedFileName = lbFiles.SelectedItem.ToString();
            string selectedPath = _filePaths.First(p => Path.GetFileName(p) == selectedFileName);

            _fileConfigs[selectedPath].HeaderRow = (int)numHeaderRow.Value;
            _fileConfigs[selectedPath].MetadataRows = txtMetadataRows.Text;
        }

        private void numHeaderRow_ValueChanged(object sender, EventArgs e)
        {
            SaveCurrentFileConfig();
        }

        private void txtMetadataRows_TextChanged(object sender, EventArgs e)
        {
            SaveCurrentFileConfig();
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
    }
}
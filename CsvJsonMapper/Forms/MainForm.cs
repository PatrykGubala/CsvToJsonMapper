using CsvJsonMapper.Forms.Dialogs;
using CsvJsonMapper.Models;
using CsvJsonMapper.Models.Mapping;
using CsvJsonMapper.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

        private MappingNode _rootMappingNode;
        private ContextMenuStrip _jsonStructureContextMenu;
        private int _newNodeCounter = 1;

        private ToolStripMenuItem _menuAddObject;
        private ToolStripMenuItem _menuAddArray;
        private ToolStripMenuItem _menuRenameNode;
        private ToolStripMenuItem _menuDeleteNode;

        public MainForm()
        {
            InitializeComponent();
            _loadedFiles = new List<CsvSourceFile>();
            _parsingService = new CsvParsingService();
            _rootNodeFont = new Font(tvSourceFiles.Font, FontStyle.Bold);

            InitializeDragDropAndContextMenus();
            InitializeJsonCreator();
        }

        private void InitializeDragDropAndContextMenus()
        {
            _jsonStructureContextMenu = new ContextMenuStrip();
            _menuAddObject = new ToolStripMenuItem("Dodaj Obiekt ({} )");
            _menuAddArray = new ToolStripMenuItem("Dodaj Tablicę ([])");
            _menuRenameNode = new ToolStripMenuItem("Zmień nazwę");
            _menuDeleteNode = new ToolStripMenuItem("Usuń");

            _menuAddObject.Click += menuAddObject_Click;
            _menuAddArray.Click += menuAddArray_Click;
            _menuRenameNode.Click += menuRenameNode_Click;
            _menuDeleteNode.Click += menuDeleteNode_Click;

            _jsonStructureContextMenu.Items.AddRange(new ToolStripItem[] {
                _menuAddObject,
                _menuAddArray,
                new ToolStripSeparator(),
                _menuRenameNode,
                _menuDeleteNode
            });
            
            tvJsonStructure.ContextMenuStrip = _jsonStructureContextMenu;
            tvJsonStructure.LabelEdit = true;

            _jsonStructureContextMenu.Opening += _jsonStructureContextMenu_Opening;
            tvJsonStructure.NodeMouseClick += (s, e) => {
                if (e.Button == MouseButtons.Right) tvJsonStructure.SelectedNode = e.Node;
            };
            tvJsonStructure.BeforeLabelEdit += tvJsonStructure_BeforeLabelEdit;
            tvJsonStructure.AfterLabelEdit += tvJsonStructure_AfterLabelEdit;
            tvJsonStructure.KeyDown += tvJsonStructure_KeyDown;

            tvSourceFiles.ItemDrag += tvSourceFiles_ItemDrag;
            tvJsonStructure.AllowDrop = true;
            tvJsonStructure.DragEnter += tvJsonStructure_DragEnter;
            tvJsonStructure.DragDrop += tvJsonStructure_DragDrop;
        }

        private string GetNodeText(MappingNode node)
        {
            if (node is MappingObject) return $"{node.Name} (obiekt)";
            if (node is MappingArray) return $"{node.Name} (tablica)";
            if (node is MappingField field) return $"{field.SourceColumnName} ({field.SourceColumnType}): \"{field.Name}\"";
            return node.Name;
        }

        private void InitializeJsonCreator()
        {
            _rootMappingNode = new MappingObject { Name = "root" };
            _newNodeCounter = 1;

            tvJsonStructure.Nodes.Clear();
            var rootTvNode = tvJsonStructure.Nodes.Add(GetNodeText(_rootMappingNode));
            rootTvNode.Tag = _rootMappingNode;

            var rootFile = _loadedFiles.FirstOrDefault(f => f.IsRootFile);
            if (rootFile != null)
            {
                var container = (IMappingContainer)_rootMappingNode;
                foreach (var header in rootFile.Headers)
                {
                    string type = rootFile.DetectedColumnTypes.ContainsKey(header) ? rootFile.DetectedColumnTypes[header] : "string";
                    var newFieldModel = new MappingField
                    {
                        Name = header,
                        SourceFileId = rootFile.FileName,
                        SourceColumnName = header,
                        SourceColumnType = type
                    };
                    container.Children.Add(newFieldModel);

                    var newTvNode = rootTvNode.Nodes.Add(GetNodeText(newFieldModel));
                    newTvNode.Tag = newFieldModel;
                }
                rootTvNode.Expand();
            }
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
                    InitializeJsonCreator();
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
                    string type = file.DetectedColumnTypes.ContainsKey(header) ? file.DetectedColumnTypes[header] : "string";
                    fileNode.Nodes.Add($"{header} ({type})").Tag = header;
                }
                tvSourceFiles.Nodes.Add(fileNode);
            }
            tvSourceFiles.ExpandAll();
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
            var file = dgv?.Tag as CsvSourceFile;
            if (file == null) return;

            if (dgv == null) return;
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
            string json = GeneratePreviewJson();
            rtbJsonStructurePreview.Text = json;
            rtbJsonPreview.Text = "Podgląd finalnego pliku JSON (z danymi) pojawi się tutaj po implementacji generowania.";
        }

        private string GeneratePreviewJson()
        {
            try
            {
                JToken token = BuildJsonNode(_rootMappingNode);
                return token.ToString(Formatting.Indented);
            }
            catch (Exception ex)
            {
                return $"Błąd podczas generowania podglądu: {ex.Message}";
            }
        }

        private JToken BuildJsonNode(MappingNode node)
        {
            if (node == null) return null;

            if (node is MappingObject obj)
            {
                var jObj = new JObject();
                foreach (var child in obj.Children)
                {
                    jObj.Add(child.Name, BuildJsonNode(child));
                }
                return jObj;
            }

            if (node is MappingArray arr)
            {
                var jArr = new JArray();
                var templateNode = arr.Children.FirstOrDefault();
                if (templateNode != null)
                {
                    jArr.Add(BuildJsonNode(templateNode));
                }
                return jArr;
            }

            if (node is MappingField field)
            {
                return new JValue($"<{field.SourceFileId}::{field.SourceColumnName}>");
            }

            return null;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void _jsonStructureContextMenu_Opening(object sender, CancelEventArgs e)
        {
            var selectedNode = tvJsonStructure.SelectedNode;
            bool isRoot = (selectedNode == null || selectedNode.Parent == null);

            _menuRenameNode.Enabled = !isRoot;
            _menuDeleteNode.Enabled = !isRoot;
        }

        private void menuAddObject_Click(object sender, EventArgs e)
        {
            var selectedNode = tvJsonStructure.SelectedNode;
            if (selectedNode == null || !(selectedNode.Tag is IMappingContainer container))
            {
                selectedNode = tvJsonStructure.Nodes[0];
                container = (IMappingContainer)_rootMappingNode;
            }

            string name = $"nowyObiekt{_newNodeCounter++}";
            var newObjectModel = new MappingObject { Name = name };
            container.Children.Add(newObjectModel);

            var newTvNode = selectedNode.Nodes.Add(GetNodeText(newObjectModel));
            newTvNode.Tag = newObjectModel;
            selectedNode.Expand();

            UpdateJsonPreview();
        }

        private void menuAddArray_Click(object sender, EventArgs e)
        {
            var selectedNode = tvJsonStructure.SelectedNode;
            if (selectedNode == null || !(selectedNode.Tag is IMappingContainer container))
            {
                selectedNode = tvJsonStructure.Nodes[0];
                container = (IMappingContainer)_rootMappingNode;
            }

            string name = $"nowaTablica{_newNodeCounter++}";
            var newArrayModel = new MappingArray { Name = name };
            container.Children.Add(newArrayModel);

            var newTvNode = selectedNode.Nodes.Add(GetNodeText(newArrayModel));
            newTvNode.Tag = newArrayModel;
            selectedNode.Expand();

            UpdateJsonPreview();
        }

        private void menuRenameNode_Click(object sender, EventArgs e)
        {
            var selectedNode = tvJsonStructure.SelectedNode;
            if (selectedNode == null || selectedNode.Parent == null) return;

            if (selectedNode.Tag is MappingField field)
            {
                string value = field.Name;
                using var dialog = new InputBoxDialog("Zmień nazwę", "Wprowadź nową nazwę wyjściową JSON:", value);
                
                if (dialog.ShowDialog() != DialogResult.OK) return;
                
                field.Name = dialog.Value;
                selectedNode.Text = GetNodeText(field);
                UpdateJsonPreview();
            }
            else if (selectedNode.Tag is MappingObject or MappingArray)
            {
                selectedNode.BeginEdit();
            }
        }

        private void tvJsonStructure_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node is { Tag: MappingField })
            {
                e.CancelEdit = true;
            }
        }

        private void tvJsonStructure_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label == null || e.Node == null)
            {
                e.CancelEdit = true;
                return;
            }

            if (e.Node.Tag is MappingObject or MappingArray)
            {
                var modelNode = (MappingNode)e.Node.Tag;
                modelNode.Name = e.Label;
                e.Node.Text = GetNodeText(modelNode);
                UpdateJsonPreview();
            }
            else if (e.Node.Tag is MappingField tag)
            {
                e.CancelEdit = true;
                e.Node.Text = GetNodeText(tag);
            }
        }

        private void menuDeleteNode_Click(object sender, EventArgs e)
        {
            var selectedNode = tvJsonStructure.SelectedNode;
            if (selectedNode == null || selectedNode.Parent == null) return;

            var parentTvNode = selectedNode.Parent;
            if (parentTvNode.Tag is IMappingContainer parentContainer && selectedNode.Tag is MappingNode nodeToRemove)
            {
                parentContainer.Children.Remove(nodeToRemove);
                selectedNode.Remove();
                UpdateJsonPreview();
            }
        }


        private void tvSourceFiles_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Item is TreeNode node && node.Parent != null)
            {
                DoDragDrop(e.Item, DragDropEffects.Copy);
            }
        }

        private void tvJsonStructure_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data != null && e.Data.GetDataPresent(
                typeof(TreeNode)) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void tvJsonStructure_DragDrop(object sender, DragEventArgs e)
        {
            Point targetPoint = tvJsonStructure.PointToClient(new Point(e.X, e.Y));
            TreeNode targetNode = tvJsonStructure.GetNodeAt(targetPoint);
            TreeNode sourceNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

            if (targetNode == null || sourceNode == null) return;
            if (sourceNode.Parent == null) return; 
            if (!(targetNode.Tag is IMappingContainer container)) return;

            CsvSourceFile sourceFile = (CsvSourceFile)sourceNode.Parent.Tag;
            string columnName = (string)sourceNode.Tag;
            string type = sourceFile.DetectedColumnTypes[columnName];

            var newFieldModel = new MappingField
            {
                Name = columnName,
                SourceFileId = sourceFile.FileName,
                SourceColumnName = columnName,
                SourceColumnType = type
            };
            container.Children.Add(newFieldModel);

            var newTvNode = targetNode.Nodes.Add(GetNodeText(newFieldModel));
            newTvNode.Tag = newFieldModel;
            targetNode.Expand();

            UpdateJsonPreview();
        }

        private void tvJsonStructure_KeyDown(object sender, KeyEventArgs e)
        {
            if (tvJsonStructure.SelectedNode == null) return;
            e.Handled = true;
            menuRenameNode_Click(sender, EventArgs.Empty);
        }
    }
}


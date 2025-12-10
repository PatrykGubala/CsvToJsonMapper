using CsvJsonMapper.Forms.Dialogs;
using CsvJsonMapper.Models;
using CsvJsonMapper.Models.Mapping;
using CsvJsonMapper.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Data;

namespace CsvJsonMapper.Forms
{
    public partial class MainForm : Form
    {
        private List<CsvSourceFile> _loadedFiles;
        private CsvParsingService _parsingService;
        private JsonGenerationService _jsonGenerationService;
        private JsonExportService _jsonExportService;
        private YamlConfigurationService _yamlConfigService;
        private List<Relation> _relations;
        private Font _rootNodeFont;
        private Font _potentialKeyFont;

        private MappingNode _rootMappingNode;
        private ContextMenuStrip _jsonStructureContextMenu;
        private ContextMenuStrip _sourceTreeContextMenu;
        private int _newNodeCounter = 1;

        private ToolStripMenuItem _menuAddObject;
        private ToolStripMenuItem _menuAddArray;
        private ToolStripMenuItem _menuRenameNode;
        private ToolStripMenuItem _menuDeleteNode;

        public MainForm()
        {
            InitializeComponent();
            _loadedFiles = new List<CsvSourceFile>();
            _relations = new List<Relation>();
            _parsingService = new CsvParsingService();
            _jsonGenerationService = new JsonGenerationService();
            _jsonExportService = new JsonExportService();
            _yamlConfigService = new YamlConfigurationService(_parsingService);
            _rootNodeFont = new Font(tvSourceFiles.Font, FontStyle.Bold);
            _potentialKeyFont = new Font(tvSourceFiles.Font, FontStyle.Italic);

            InitializeDragDropAndContextMenus();
            InitializeJsonCreator();
            SetupTemplateMenu();
        }

        private void SetupTemplateMenu()
        {
            var saveItem = new ToolStripMenuItem("Zapisz Szablon (YAML)...", null, SaveTemplate_Click);
            var loadItem = new ToolStripMenuItem("Wczytaj Szablon (YAML)...", null, LoadTemplate_Click);

            fileToolStripMenuItem.DropDownItems.Insert(2, new ToolStripSeparator());
            fileToolStripMenuItem.DropDownItems.Insert(3, saveItem);
            fileToolStripMenuItem.DropDownItems.Insert(4, loadItem);
        }

        private void SaveTemplate_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Pliki YAML (*.yaml)|*.yaml|Wszystkie pliki (*.*)|*.*";
                sfd.Title = "Zapisz konfigurację projektu";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _yamlConfigService.SaveConfiguration(sfd.FileName, _loadedFiles, _relations, _rootMappingNode);
                        MessageBox.Show("Szablon został zapisany pomyślnie.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Błąd podczas zapisywania szablonu: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void LoadTemplate_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Pliki YAML (*.yaml)|*.yaml|Wszystkie pliki (*.*)|*.*";
                ofd.Title = "Wczytaj konfigurację projektu";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var config = _yamlConfigService.ReadConfiguration(ofd.FileName);

                        using (var dialog = new ImportYamlConfigDialog(config))
                        {
                            if (dialog.ShowDialog() == DialogResult.OK)
                            {
                                var validationErrors = _yamlConfigService.ValidateConfigurationIntegrity(config);
                                if (validationErrors.Count > 0)
                                {
                                    string message = "Wykryto błędy w konfiguracji:\n\n" + string.Join("\n", validationErrors.Take(10));
                                    if (validationErrors.Count > 10) message += "\n...i więcej.";
                                    
                                    MessageBox.Show(message, "Błąd Walidacji Konfiguracji", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                var result = _yamlConfigService.ProcessConfiguration(config);

                                _loadedFiles = result.Files;
                                _relations = result.Relations;
                                _rootMappingNode = result.RootNode;

                                UpdateSourceTreeView();
                                UpdateCsvViewsTabControl();
                                RebuildJsonStructureTree();
                                UpdateJsonPreview();

                                lblStatus.Text = $"Wczytano konfigurację z pliku: {Path.GetFileName(ofd.FileName)}";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Błąd podczas wczytywania szablonu: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void exportJsonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_loadedFiles.Count == 0 || !_loadedFiles.Any(f => f.IsRootFile))
            {
                MessageBox.Show("Brak załadowanych plików lub nie wybrano pliku głównego (Root).", "Błąd eksportu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Plik JSON (*.json)|*.json";
                sfd.Title = "Eksportuj do JSON";
                sfd.FileName = "output.json";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        this.Cursor = Cursors.WaitCursor;
                        lblStatus.Text = "Trwa eksportowanie danych...";
                        
                        _jsonExportService.ExportJson(sfd.FileName, _rootMappingNode, _loadedFiles, _relations);
                        
                        lblStatus.Text = "Eksport zakończony pomyślnie.";
                        MessageBox.Show("Eksport zakończony pomyślnie!", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = "Błąd eksportu.";
                        MessageBox.Show($"Wystąpił błąd podczas eksportu: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
            }
        }

        private void InitializeDragDropAndContextMenus()
        {
            _jsonStructureContextMenu = new ContextMenuStrip();
            _menuAddObject = new ToolStripMenuItem("Dodaj Obiekt ({} )");
            _menuAddArray = new ToolStripMenuItem("Dodaj Tablicę ([])");
            _menuRenameNode = new ToolStripMenuItem("Zmień nazwę");
            _menuDeleteNode = new ToolStripMenuItem("Usuń");

            var menuAddEmptyObject = new ToolStripMenuItem("Dodaj Pusty Obiekt", null, menuAddObject_Click);
            var menuAddObjectFromRelation = new ToolStripMenuItem("Dodaj Obiekt z Relacji (1:1)", null, menuAddFromRelation_Click);
            _menuAddObject.DropDownItems.AddRange(new ToolStripItem[] { menuAddEmptyObject, menuAddObjectFromRelation });

            var menuAddEmptyArray = new ToolStripMenuItem("Dodaj Pustą Tablicę", null, menuAddArray_Click);
            var menuAddArrayFromRelation = new ToolStripMenuItem("Dodaj Tablicę z Relacji (1:N)", null, menuAddFromRelation_Click);
            _menuAddArray.DropDownItems.AddRange(new ToolStripItem[] { menuAddEmptyArray, menuAddArrayFromRelation });

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
            
            _sourceTreeContextMenu = new ContextMenuStrip();
            var setRootItem = new ToolStripMenuItem("Ustaw jako plik główny (Root)", null, OnSourceNode_SetAsRoot);
            _sourceTreeContextMenu.Items.Add(setRootItem);
            tvSourceFiles.ContextMenuStrip = _sourceTreeContextMenu;
            tvSourceFiles.NodeMouseClick += tvSourceFiles_NodeMouseClick;
            _sourceTreeContextMenu.Opening += OnSourceTreeContextMenu_Opening;
        }

        private void tvSourceFiles_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                tvSourceFiles.SelectedNode = e.Node;
            }
        }

        private void OnSourceTreeContextMenu_Opening(object sender, CancelEventArgs e)
        {
            var node = tvSourceFiles.SelectedNode;
            if (node != null && node.Parent == null && node.Tag is CsvSourceFile file)
            {
                var setRootItem = _sourceTreeContextMenu.Items[0];
                setRootItem.Enabled = !file.IsRootFile;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void OnSourceNode_SetAsRoot(object sender, EventArgs e)
        {
            var node = tvSourceFiles.SelectedNode;
            if (node == null || !(node.Tag is CsvSourceFile newRootFile)) return;

            var oldRootFile = _loadedFiles.FirstOrDefault(f => f.IsRootFile);
            if (oldRootFile != null)
            {
                oldRootFile.IsRootFile = false;
            }

            newRootFile.IsRootFile = true;

            UpdateSourceTreeView();
            InitializeJsonCreator();
            UpdateJsonPreview();
        }

        private string GetNodeText(MappingNode node)
        {
            if (node is MappingObject obj)
            {
                var rel = _relations.FirstOrDefault(r => r.Id == obj.RelationId);
                string relInfo = rel != null ? $" (Rel: {rel.Name})" : "";
                return $"{node.Name} (obiekt){relInfo}";
            }
            if (node is MappingArray arr)
            {
                var rel = _relations.FirstOrDefault(r => r.Id == arr.RelationId);
                string relInfo = rel != null ? $" (Rel: {rel.Name})" : "";
                return $"{node.Name} (tablica){relInfo}";
            }
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

        private void RebuildJsonStructureTree()
        {
            tvJsonStructure.Nodes.Clear();
            if (_rootMappingNode == null) return;

            var rootTvNode = tvJsonStructure.Nodes.Add(GetNodeText(_rootMappingNode));
            rootTvNode.Tag = _rootMappingNode;

            if (_rootMappingNode is IMappingContainer container)
            {
                foreach (var child in container.Children)
                {
                    BuildTreeRecursive(rootTvNode, child);
                }
            }
            rootTvNode.ExpandAll();
        }

        private void BuildTreeRecursive(TreeNode parentTvNode, MappingNode mappingNode)
        {
            var tvNode = parentTvNode.Nodes.Add(GetNodeText(mappingNode));
            tvNode.Tag = mappingNode;

            if (mappingNode is IMappingContainer container)
            {
                foreach (var child in container.Children)
                {
                    BuildTreeRecursive(tvNode, child);
                }
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

                foreach (var file in ofd.FileNames)
                {
                    try
                    {
                        _parsingService.ValidateFileStructure(file, 0); 
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Błąd walidacji pliku {Path.GetFileName(file)}:\n{ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                using (var dialog = new ImportConfigurationDialog(ofd.FileNames, _parsingService))
                {
                    if (dialog.ShowDialog() != DialogResult.OK) return;

                    _loadedFiles.Clear();
                    _relations.Clear();
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

                            foreach (var typeOverride in config.ColumnTypeOverrides)
                            {
                                if (csvFile.DetectedColumnTypes.ContainsKey(typeOverride.Key))
                                {
                                    csvFile.DetectedColumnTypes[typeOverride.Key] = typeOverride.Value;
                                }
                            }

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
                    var columnNode = fileNode.Nodes.Add($"{header} ({type})");
                    columnNode.Tag = header;

                    if (type == "int")
                    {
                        columnNode.NodeFont = _potentialKeyFont;
                    }
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
            string jsonStructure = GeneratePreviewJsonStructure();
            rtbJsonStructurePreview.Text = jsonStructure;

            try
            {
                string jsonData = _jsonGenerationService.GeneratePreviewJson(
                    _rootMappingNode,
                    _loadedFiles,
                    _relations
                );
                rtbJsonPreview.Text = jsonData;
                
                rtbYamlPreview.Text = _yamlConfigService.GetConfigurationYaml(_loadedFiles, _relations, _rootMappingNode);
            }
            catch (Exception ex)
            {
                rtbJsonPreview.Text = $"Błąd podczas generowania podglądu danych: {ex.Message}";
                rtbYamlPreview.Text = $"Błąd podczas generowania podglądu YAML: {ex.Message}";
            }
        }

        private string GeneratePreviewJsonStructure()
        {
            try
            {
                JToken token = BuildJsonStructureNode(_rootMappingNode);
                return token.ToString(Formatting.Indented);
            }
            catch (Exception ex)
            {
                return $"Błąd podczas generowania podglądu: {ex.Message}";
            }
        }

        private JToken BuildJsonStructureNode(MappingNode node)
        {
            if (node == null) return null;

            if (node is MappingObject obj)
            {
                var jObj = new JObject();
                foreach (var child in obj.Children)
                {
                    jObj.Add(child.Name, BuildJsonStructureNode(child));
                }
                return jObj;
            }

            if (node is MappingArray arr)
            {
                var jArr = new JArray();
                var templateNode = arr.Children.FirstOrDefault();
                if (templateNode != null)
                {
                    jArr.Add(BuildJsonStructureNode(templateNode));
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

            bool canAddRelation = (selectedNode != null && selectedNode.Tag is IMappingContainer);
            _menuAddObject.DropDownItems[1].Enabled = canAddRelation && _relations.Any(r => r.Type == RelationType.OneToOne);
            _menuAddArray.DropDownItems[1].Enabled = canAddRelation && _relations.Any(r => r.Type == RelationType.OneToMany);
        }

        private void menuAddObject_Click(object sender, EventArgs e)
        {
            AddMappingNode(new MappingObject { Name = $"nowyObiekt{_newNodeCounter++}" });
        }

        private void menuAddArray_Click(object sender, EventArgs e)
        {
            AddMappingNode(new MappingArray { Name = $"nowaTablica{_newNodeCounter++}" });
        }

        private void menuAddFromRelation_Click(object sender, EventArgs e)
        {
            bool isArray = (sender as ToolStripItem)?.OwnerItem == _menuAddArray;
            var type = isArray ? RelationType.OneToMany : RelationType.OneToOne;

            using (var dialog = new SelectRelationDialog(_relations, type))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var relation = dialog.SelectedRelation;
                    if (isArray)
                    {
                        AddMappingNode(new MappingArray
                        {
                            Name = relation.Name,
                            RelationId = relation.Id
                        });
                    }
                    else
                    {
                        AddMappingNode(new MappingObject
                        {
                            Name = relation.Name,
                            RelationId = relation.Id
                        });
                    }
                }
            }
        }

        private void AddMappingNode(MappingNode node)
        {
            var selectedNode = tvJsonStructure.SelectedNode;
            IMappingContainer container;

            if (selectedNode == null || !(selectedNode.Tag is IMappingContainer))
            {
                selectedNode = tvJsonStructure.Nodes[0];
                container = (IMappingContainer)_rootMappingNode;
            }
            else
            {
                container = (IMappingContainer)selectedNode.Tag;
            }

            container.Children.Add(node);
            var newTvNode = selectedNode.Nodes.Add(GetNodeText(node));
            newTvNode.Tag = node;
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
                using (var dialog = new InputBoxDialog("Zmień nazwę", "Wprowadź nową nazwę wyjściową JSON:", value))
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        field.Name = dialog.Value;
                        selectedNode.Text = GetNodeText(field);
                        UpdateJsonPreview();
                    }
                }
            }
            else if (selectedNode.Tag is MappingObject || selectedNode.Tag is MappingArray)
            {
                selectedNode.BeginEdit();
            }
        }

        private void tvJsonStructure_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node.Tag is MappingField)
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

            if (e.Node.Tag is MappingObject || e.Node.Tag is MappingArray)
            {
                var modelNode = (MappingNode)e.Node.Tag;
                modelNode.Name = e.Label;
                e.Node.Text = GetNodeText(modelNode);
                UpdateJsonPreview();
            }
            else if (e.Node.Tag is MappingField)
            {
                e.CancelEdit = true;
                e.Node.Text = GetNodeText((MappingNode)e.Node.Tag);
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
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
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
            if (e.KeyCode == Keys.F2 && tvJsonStructure.SelectedNode != null)
            {
                e.Handled = true;
                menuRenameNode_Click(sender, EventArgs.Empty);
            }
        }

        private void manageRelationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new ManageRelationsDialog(_relations, _loadedFiles))
            {
                dialog.ShowDialog();
                UpdateJsonPreview(); 
            }
        }
    }
}
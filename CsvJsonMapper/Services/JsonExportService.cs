using CsvHelper;
using CsvHelper.Configuration;
using CsvJsonMapper.Models;
using CsvJsonMapper.Models.Mapping;
using Newtonsoft.Json;
using System.Data;
using System.Globalization;

namespace CsvJsonMapper.Services
{
    public class JsonExportService
    {
        public void ExportJson(string outputPath, MappingNode rootNode, List<CsvSourceFile> files, List<Relation> relations, bool includeNullValues)
        {
            var rootFile = files.FirstOrDefault(f => f.IsRootFile);
            if (rootFile == null) throw new Exception("Nie zdefiniowano pliku głównego (Root).");
            if (rootNode == null) throw new Exception("Struktura JSON nie została zdefiniowana.");

            var relationIndexes = BuildRelationIndexes(files, relations);
            var fileConfigs = files.ToDictionary(f => f.FileName, f => f);

            using (var fileStream = File.CreateText(outputPath))
            using (var jsonWriter = new JsonTextWriter(fileStream))
            {
                jsonWriter.Formatting = Formatting.Indented;
                jsonWriter.WriteStartArray();

                var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = rootFile.Delimiter,
                    HasHeaderRecord = true,
                    BadDataFound = null
                };

                using (var reader = new StreamReader(rootFile.FilePath))
                using (var csv = new CsvReader(reader, csvConfig))
                {
                    ReadPastHeader(csv, rootFile);

                    while (csv.Read())
                    {
                        if (rootFile.MetadataRowIndices.Contains(csv.Context.Parser.Row - 1)) continue;

                        var rootRowDict = GetRowDictionary(csv, rootFile);
                        var dataRow = CreateDataRow(rootRowDict, rootFile);
                        
                        WriteNode(jsonWriter, rootNode, dataRow, rootFile, fileConfigs, relations, relationIndexes, includeNullValues);
                    }
                }

                jsonWriter.WriteEndArray();
            }
        }

        private DataRow CreateDataRow(Dictionary<string, string> rowDict, CsvSourceFile file)
        {
            var table = file.ProcessedData.Clone(); 
            var row = table.NewRow();
            foreach (var kvp in rowDict)
            {
                if (table.Columns.Contains(kvp.Key))
                {
                    row[kvp.Key] = kvp.Value;
                }
            }
            return row;
        }

        private Dictionary<Guid, ILookup<string, DataRow>> BuildRelationIndexes(List<CsvSourceFile> files, List<Relation> relations)
        {
            var indexes = new Dictionary<Guid, ILookup<string, DataRow>>();
            var filesMap = files.ToDictionary(f => f.FileName, f => f);

            foreach (var relation in relations)
            {
                if (!filesMap.TryGetValue(relation.ChildFileId, out var childFile)) continue;

                var childRows = new List<DataRow>();
                
                var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = childFile.Delimiter,
                    HasHeaderRecord = true,
                    BadDataFound = null
                };

                using (var reader = new StreamReader(childFile.FilePath))
                using (var csv = new CsvReader(reader, csvConfig))
                {
                    ReadPastHeader(csv, childFile);

                    while (csv.Read())
                    {
                        if (childFile.MetadataRowIndices.Contains(csv.Context.Parser.Row - 1)) continue;
                        
                        var dict = GetRowDictionary(csv, childFile);
                        childRows.Add(CreateDataRow(dict, childFile));
                    }
                }

                var lookup = childRows.ToLookup(row =>
                {
                    var keyParts = new List<string>();
                    foreach (var col in relation.ChildKeyColumns)
                    {
                        keyParts.Add(row.Table.Columns.Contains(col) ? row[col]?.ToString() : "");
                    }
                    return string.Join("|", keyParts);
                });

                indexes.Add(relation.Id, lookup);
            }

            return indexes;
        }

        private void WriteNode(
            JsonTextWriter writer, 
            MappingNode node, 
            DataRow currentRow, 
            CsvSourceFile currentFile,
            Dictionary<string, CsvSourceFile> filesMap,
            List<Relation> relations,
            Dictionary<Guid, ILookup<string, DataRow>> indexes,
            bool includeNullValues,
            string propertyName = null)
        {
            if (node is MappingField field)
            {
                object value = TransformationHelper.ProcessValue(field, currentRow);
                
                if (value == null && !includeNullValues && propertyName != null)
                {
                    return;
                }

                if (propertyName != null)
                {
                    writer.WritePropertyName(propertyName);
                }
                writer.WriteValue(value);
            }
            else if (node is MappingObject obj)
            {
                if (propertyName != null)
                {
                    writer.WritePropertyName(propertyName);
                }

                writer.WriteStartObject();
                
                DataRow contextRow = currentRow;
                CsvSourceFile contextFile = currentFile;
                bool skipChildren = false;

                if (obj.RelationId.HasValue)
                {
                    if (indexes.TryGetValue(obj.RelationId.Value, out var lookup))
                    {
                        var relation = relations.FirstOrDefault(r => r.Id == obj.RelationId.Value);
                        if (relation != null)
                        {
                            string parentKey = GetCompositeKey(currentRow, relation.ParentKeyColumns);
                            var childRows = lookup[parentKey];
                            var childRow = childRows.FirstOrDefault();

                            if (childRow != null)
                            {
                                contextRow = childRow;
                                contextFile = filesMap[relation.ChildFileId];
                            }
                            else
                            {
                                skipChildren = true;
                            }
                        }
                    }
                }

                if (!skipChildren)
                {
                    foreach (var child in obj.Children)
                    {
                        WriteNode(writer, child, contextRow, contextFile, filesMap, relations, indexes, includeNullValues, child.Name);
                    }
                }

                writer.WriteEndObject();
            }
            else if (node is MappingArray arr)
            {
                if (propertyName != null)
                {
                    writer.WritePropertyName(propertyName);
                }

                writer.WriteStartArray();
                
                if (arr.Children.Count > 0 && arr.RelationId.HasValue)
                {
                    if (indexes.TryGetValue(arr.RelationId.Value, out var lookup))
                    {
                        var relation = relations.FirstOrDefault(r => r.Id == arr.RelationId.Value);
                        if (relation != null)
                        {
                            string parentKey = GetCompositeKey(currentRow, relation.ParentKeyColumns);
                            var childRows = lookup[parentKey];
                            var childFile = filesMap[relation.ChildFileId];

                            foreach (var childRow in childRows)
                            {
                                if (arr.Children.Count == 1)
                                {
                                    WriteNode(writer, arr.Children[0], childRow, childFile, filesMap, relations, indexes, includeNullValues, null);
                                }
                                else
                                {
                                    writer.WriteStartObject();
                                    foreach(var child in arr.Children)
                                    {
                                        WriteNode(writer, child, childRow, childFile, filesMap, relations, indexes, includeNullValues, child.Name);
                                    }
                                    writer.WriteEndObject();
                                }
                            }
                        }
                    }
                }

                writer.WriteEndArray();
            }
        }

        private void ReadPastHeader(CsvReader csv, CsvSourceFile file)
        {
            int currentRow = 0;
            while (currentRow < file.HeaderRowIndex)
            {
                csv.Read();
                currentRow++;
            }
            csv.Read(); 
            csv.ReadHeader();
        }

        private Dictionary<string, string> GetRowDictionary(CsvReader csv, CsvSourceFile file)
        {
            var dict = new Dictionary<string, string>();
            foreach (var header in file.Headers)
            {
                try
                {
                    dict[header] = csv.GetField(header);
                }
                catch
                {
                    dict[header] = null;
                }
            }
            return dict;
        }

        private string GetCompositeKey(DataRow row, List<string> columns)
        {
            var parts = new List<string>();
            foreach(var col in columns)
            {
                parts.Add(row.Table.Columns.Contains(col) ? row[col]?.ToString() : "");
            }
            return string.Join("|", parts);
        }
    }
}
using CsvHelper;
using CsvHelper.Configuration;
using CsvJsonMapper.Models;
using CsvJsonMapper.Models.Mapping;
using Newtonsoft.Json;
using System.Globalization;


namespace CsvJsonMapper.Services
{
    public class JsonExportService
    {
        public void ExportJson(string outputPath, MappingNode rootNode, List<CsvSourceFile> files, List<Relation> relations)
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
                        WriteNode(jsonWriter, rootNode, rootRowDict, rootFile, fileConfigs, relations, relationIndexes);
                    }
                }

                jsonWriter.WriteEndArray();
            }
        }

        private Dictionary<Guid, ILookup<string, Dictionary<string, string>>> BuildRelationIndexes(List<CsvSourceFile> files, List<Relation> relations)
        {
            var indexes = new Dictionary<Guid, ILookup<string, Dictionary<string, string>>>();
            var filesMap = files.ToDictionary(f => f.FileName, f => f);

            foreach (var relation in relations)
            {
                if (!filesMap.TryGetValue(relation.ChildFileId, out var childFile)) continue;

                var childRows = new List<Dictionary<string, string>>();
                
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
                        childRows.Add(GetRowDictionary(csv, childFile));
                    }
                }

                var lookup = childRows.ToLookup(row =>
                {
                    var keyParts = new List<string>();
                    foreach (var col in relation.ChildKeyColumns)
                    {
                        keyParts.Add(row.ContainsKey(col) ? row[col] : "");
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
            Dictionary<string, string> currentRow, 
            CsvSourceFile currentFile,
            Dictionary<string, CsvSourceFile> filesMap,
            List<Relation> relations,
            Dictionary<Guid, ILookup<string, Dictionary<string, string>>> indexes)
        {
            if (node is MappingField field)
            {
                string value = currentRow.ContainsKey(field.SourceColumnName) ? currentRow[field.SourceColumnName] : null;
                WriteValue(writer, value, field.SourceColumnType);
            }
            else if (node is MappingObject obj)
            {
                writer.WriteStartObject();
                
                Dictionary<string, string> contextRow = currentRow;
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
                        writer.WritePropertyName(child.Name);
                        WriteNode(writer, child, contextRow, contextFile, filesMap, relations, indexes);
                    }
                }

                writer.WriteEndObject();
            }
            else if (node is MappingArray arr)
            {
                writer.WriteStartArray();
                var templateNode = arr.Children.FirstOrDefault();

                if (templateNode != null && arr.RelationId.HasValue)
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
                                WriteNode(writer, templateNode, childRow, childFile, filesMap, relations, indexes);
                            }
                        }
                    }
                }

                writer.WriteEndArray();
            }
        }

        private void WriteValue(JsonTextWriter writer, string value, string type)
        {
            if (string.IsNullOrEmpty(value))
            {
                writer.WriteNull();
                return;
            }

            switch (type)
            {
                case "int":
                    if (int.TryParse(value, out int iVal)) writer.WriteValue(iVal);
                    else writer.WriteNull();
                    break;
                case "double":
                    if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double dVal)) writer.WriteValue(dVal);
                    else writer.WriteNull();
                    break;
                default:
                    writer.WriteValue(value);
                    break;
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

        private string GetCompositeKey(Dictionary<string, string> row, List<string> columns)
        {
            var parts = new List<string>();
            foreach(var col in columns)
            {
                parts.Add(row.ContainsKey(col) ? row[col] : "");
            }
            return string.Join("|", parts);
        }
    }
}
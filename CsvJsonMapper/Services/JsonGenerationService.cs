using CsvJsonMapper.Models;
using CsvJsonMapper.Models.Mapping;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Globalization;

namespace CsvJsonMapper.Services
{
    public class JsonGenerationService
    {
        public string GeneratePreviewJson(MappingNode rootNode, List<CsvSourceFile> files, List<Relation> relations)
        {
            var rootFile = files.FirstOrDefault(f => f.IsRootFile);
            if (rootNode == null || rootFile == null || rootFile.ProcessedData.Rows.Count == 0)
            {
                return "[]";
            }

            if (!(rootNode is MappingObject rootObject))
            {
                return "Błąd: Węzeł główny (root) musi być obiektem.";
            }

            var jArray = new JArray();
            var fileMap = files.ToDictionary(f => f.FileName, f => f);
            var relationMap = relations.ToDictionary(r => r.Id, r => r);

            try
            {
                foreach (DataRow rootRow in rootFile.ProcessedData.Rows)
                {
                    JToken rowToken = BuildNode(rootObject, rootRow, rootFile, fileMap, relationMap);
                    jArray.Add(rowToken);
                }
                return jArray.ToString(Newtonsoft.Json.Formatting.Indented);
            }
            catch (Exception ex)
            {
                return $"Błąd podczas generowania podglądu danych JSON: {ex.Message}\n{ex.StackTrace}";
            }
        }

        private JToken BuildNode(MappingNode node, DataRow currentRow, CsvSourceFile currentFile, Dictionary<string, CsvSourceFile> fileMap, Dictionary<Guid, Relation> relationMap)
        {
            if (node == null)
            {
                return null;
            }

            if (node is MappingField field)
            {
                if (!currentRow.Table.Columns.Contains(field.SourceColumnName))
                {
                    throw new Exception($"Błąd mapowania: Kolumna '{field.SourceColumnName}' nie istnieje w pliku '{currentFile.FileName}'.");
                }
                string value = currentRow[field.SourceColumnName]?.ToString();
                return ConvertValue(value, field.SourceColumnType);
            }

            if (node is MappingObject obj)
            {
                var jObj = new JObject();
                DataRow contextRow = currentRow;
                CsvSourceFile contextFile = currentFile;

                if (obj.RelationId.HasValue)
                {
                    if (!relationMap.TryGetValue(obj.RelationId.Value, out var relation))
                    {
                        throw new Exception($"Nie znaleziono relacji o ID: {obj.RelationId.Value}");
                    }
                    if (!fileMap.TryGetValue(relation.ChildFileId, out var childFile))
                    {
                        throw new Exception($"Nie znaleziono pliku podrzędnego '{relation.ChildFileId}' dla relacji '{relation.Name}'");
                    }

                    DataRow childRow = FindMatchingRow(currentRow, currentFile, childFile, relation);
                    if (childRow != null)
                    {
                        contextRow = childRow;
                        contextFile = childFile;
                    }
                    else
                    {
                        return null;
                    }
                }

                foreach (var child in obj.Children)
                {
                    jObj.Add(child.Name, BuildNode(child, contextRow, contextFile, fileMap, relationMap));
                }
                return jObj;
            }

            if (node is MappingArray arr)
            {
                if (!arr.RelationId.HasValue)
                {
                    return new JArray();
                }
                if (!relationMap.TryGetValue(arr.RelationId.Value, out var relation))
                {
                    throw new Exception($"Nie znaleziono relacji o ID: {arr.RelationId.Value}");
                }
                if (!fileMap.TryGetValue(relation.ChildFileId, out var childFile))
                {
                    throw new Exception($"Nie znaleziono pliku podrzędnego '{relation.ChildFileId}' dla relacji '{relation.Name}'");
                }

                var jArr = new JArray();
                var templateNode = arr.Children.FirstOrDefault();
                if (templateNode == null)
                {
                    return jArr;
                }

                List<DataRow> childRows = FindMatchingRows(currentRow, currentFile, childFile, relation);
                foreach (DataRow childRow in childRows)
                {
                    jArr.Add(BuildNode(templateNode, childRow, childFile, fileMap, relationMap));
                }
                return jArr;
            }

            return null;
        }

        private JToken ConvertValue(string value, string type)
        {
            if (string.IsNullOrEmpty(value))
            {
                return JValue.CreateNull();
            }

            switch (type)
            {
                case "int":
                    if (int.TryParse(value, out int intVal)) return new JValue(intVal);
                    return JValue.CreateNull();
                case "double":
                    if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double doubleVal)) return new JValue(doubleVal);
                    return JValue.CreateNull();
                case "string":
                default:
                    return new JValue(value);
            }
        }

        private DataRow FindMatchingRow(DataRow parentRow, CsvSourceFile parentFile, CsvSourceFile childFile, Relation relation)
        {
            return childFile.ProcessedData.AsEnumerable()
                .FirstOrDefault(childRow =>
                {
                    for (int i = 0; i < relation.ParentKeyColumns.Count; i++)
                    {
                        string pkCol = relation.ParentKeyColumns[i];
                        string fkCol = relation.ChildKeyColumns[i];

                        string pkValue = parentRow[pkCol]?.ToString().Trim();
                        string fkValue = childRow[fkCol]?.ToString().Trim();

                        if (pkValue != fkValue)
                        {
                            return false;
                        }
                    }
                    return true;
                });
        }

        private List<DataRow> FindMatchingRows(DataRow parentRow, CsvSourceFile parentFile, CsvSourceFile childFile, Relation relation)
        {
            return childFile.ProcessedData.AsEnumerable()
                .Where(childRow =>
                {
                    for (int i = 0; i < relation.ParentKeyColumns.Count; i++)
                    {
                        string pkCol = relation.ParentKeyColumns[i];
                        string fkCol = relation.ChildKeyColumns[i];

                        string pkValue = parentRow[pkCol]?.ToString().Trim();
                        string fkValue = childRow[fkCol]?.ToString().Trim();

                        if (pkValue != fkValue)
                        {
                            return false;
                        }
                    }
                    return true;
                })
                .ToList();
        }
    }
}
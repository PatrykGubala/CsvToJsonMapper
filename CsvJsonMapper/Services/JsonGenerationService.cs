using CsvJsonMapper.Models;
using CsvJsonMapper.Models.Mapping;
using Newtonsoft.Json.Linq;
using System.Data;

namespace CsvJsonMapper.Services
{
    public class JsonGenerationService
    {
        public string GeneratePreviewJson(MappingNode rootNode, List<CsvSourceFile> files, List<Relation> relations, bool includeNullValues)
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
                    JToken rowToken = BuildNode(rootObject, rootRow, rootFile, fileMap, relationMap, includeNullValues);
                    if (rowToken != null)
                    {
                        jArray.Add(rowToken);
                    }
                }
                return jArray.ToString(Newtonsoft.Json.Formatting.Indented);
            }
            catch (Exception ex)
            {
                return $"Błąd podczas generowania podglądu danych JSON: {ex.Message}\n{ex.StackTrace}";
            }
        }

        private JToken BuildNode(MappingNode node, DataRow currentRow, CsvSourceFile currentFile, Dictionary<string, CsvSourceFile> fileMap, Dictionary<Guid, Relation> relationMap, bool includeNullValues)
        {
            if (node == null)
            {
                return null;
            }

            if (node is MappingField field)
            {
                object value = TransformationHelper.ProcessValue(field, currentRow);
                return value == null ? JValue.CreateNull() : new JValue(value);
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
                    var childToken = BuildNode(child, contextRow, contextFile, fileMap, relationMap, includeNullValues);
                    
                    if (!includeNullValues && (childToken == null || childToken.Type == JTokenType.Null))
                    {
                        continue;
                    }

                    jObj.Add(child.Name, childToken);
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
                
                if (arr.Children.Count == 0) return jArr;

                List<DataRow> childRows = FindMatchingRows(currentRow, currentFile, childFile, relation);
                
                foreach (DataRow childRow in childRows)
                {
                    if (arr.Children.Count == 1)
                    {
                        jArr.Add(BuildNode(arr.Children[0], childRow, childFile, fileMap, relationMap, includeNullValues));
                    }
                    else
                    {
                        var compositeObj = new JObject();
                        foreach (var child in arr.Children)
                        {
                            var token = BuildNode(child, childRow, childFile, fileMap, relationMap, includeNullValues);
                            
                            if (!includeNullValues && (token == null || token.Type == JTokenType.Null))
                            {
                                continue;
                            }
                            
                            compositeObj.Add(child.Name, token);
                        }
                        jArr.Add(compositeObj);
                    }
                }
                return jArr;
            }

            return null;
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

                        if (!string.Equals(pkValue, fkValue, StringComparison.OrdinalIgnoreCase))
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

                        if (!string.Equals(pkValue, fkValue, StringComparison.OrdinalIgnoreCase))
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
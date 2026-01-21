using CsvHelper;
using CsvHelper.Configuration;
using CsvJsonMapper.Models;
using CsvJsonMapper.Models.Configuration;
using CsvJsonMapper.Models.Mapping;
using System.Globalization;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace CsvJsonMapper.Services
{
    public class YamlConfigurationService
    {
        private readonly CsvParsingService _parsingService;

        public YamlConfigurationService(CsvParsingService parsingService)
        {
            _parsingService = parsingService;
        }

        public string GetConfigurationYaml(List<CsvSourceFile> files, List<Relation> relations, MappingNode rootNode, bool includeNullValues)
        {
            var config = new ProjectConfiguration
            {
                Relations = relations,
                RootNode = rootNode,
                IncludeNullValues = includeNullValues
            };

            if (files != null)
            {
                foreach (var file in files)
                {
                    config.Files.Add(new FileSourceDefinition
                    {
                        FilePath = file.FilePath,
                        FileName = file.FileName,
                        IsRootFile = file.IsRootFile,
                        HeaderRowIndex = file.HeaderRowIndex,
                        MetadataRowIndices = file.MetadataRowIndices,
                        ColumnTypeOverrides = file.DetectedColumnTypes,
                        PrimaryKeyColumns = file.PrimaryKeyColumns
                    });
                }
            }

            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .WithTagMapping("!field", typeof(MappingField))
                .WithTagMapping("!object", typeof(MappingObject))
                .WithTagMapping("!array", typeof(MappingArray))
                .Build();

            return serializer.Serialize(config);
        }

        public void SaveConfiguration(string filePath, List<CsvSourceFile> files, List<Relation> relations, MappingNode rootNode, bool includeNullValues)
        {
            string yaml = GetConfigurationYaml(files, relations, rootNode, includeNullValues);
            File.WriteAllText(filePath, yaml);
        }

        public ProjectConfiguration ReadConfiguration(string filePath)
        {
            string yaml = File.ReadAllText(filePath);

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .IgnoreUnmatchedProperties()
                .WithTagMapping("!field", typeof(MappingField))
                .WithTagMapping("!object", typeof(MappingObject))
                .WithTagMapping("!array", typeof(MappingArray))
                .Build();

            return deserializer.Deserialize<ProjectConfiguration>(yaml);
        }

        public List<string> ValidateConfigurationIntegrity(ProjectConfiguration config)
        {
            var errors = new List<string>();
            var fileHeaders = new Dictionary<string, List<string>>();

            if (config.Files == null || config.Files.Count == 0)
            {
                errors.Add("Konfiguracja nie zawiera definicji żadnych plików źródłowych.");
                return errors;
            }

            foreach (var fileDef in config.Files)
            {
                if (!File.Exists(fileDef.FilePath))
                {
                    errors.Add($"Plik źródłowy nie istnieje: {fileDef.FilePath} (ID: {fileDef.FileName})");
                    continue;
                }

                try
                {
                    var headers = GetFileHeaders(fileDef);
                    fileHeaders[fileDef.FileName] = headers;
                }
                catch (Exception ex)
                {
                    errors.Add($"Nie można odczytać nagłówków z pliku {fileDef.FileName}: {ex.Message}");
                }
            }

            if (config.Relations != null)
            {
                foreach (var rel in config.Relations)
                {
                    if (!fileHeaders.ContainsKey(rel.ParentFileId))
                        errors.Add($"Relacja '{rel.Name}': Plik nadrzędny '{rel.ParentFileId}' nie jest zdefiniowany.");
                    else
                    {
                        foreach (var col in rel.ParentKeyColumns)
                        {
                            if (!fileHeaders[rel.ParentFileId].Contains(col))
                                errors.Add($"Relacja '{rel.Name}': Kolumna klucza '{col}' nie istnieje w pliku '{rel.ParentFileId}'.");
                        }
                    }

                    if (!fileHeaders.ContainsKey(rel.ChildFileId))
                        errors.Add($"Relacja '{rel.Name}': Plik podrzędny '{rel.ChildFileId}' nie jest zdefiniowany.");
                    else
                    {
                        foreach (var col in rel.ChildKeyColumns)
                        {
                            if (!fileHeaders[rel.ChildFileId].Contains(col))
                                errors.Add($"Relacja '{rel.Name}': Kolumna klucza '{col}' nie istnieje w pliku '{rel.ChildFileId}'.");
                        }
                    }
                }
            }

            if (config.RootNode != null)
            {
                ValidateNodeRecursively(config.RootNode, fileHeaders, config.Relations ?? new List<Relation>(), errors);
            }

            return errors;
        }

        private void ValidateNodeRecursively(MappingNode node, Dictionary<string, List<string>> fileHeaders, List<Relation> relations, List<string> errors)
        {
            if (node is MappingField field)
            {
                if (!fileHeaders.ContainsKey(field.SourceFileId))
                {
                    errors.Add($"Pole '{field.Name}': Odwołuje się do nieznanego pliku '{field.SourceFileId}'.");
                }
                else
                {
                    if (field.TransformationType != TransformationType.CombineFields)
                    {
                        if (!fileHeaders[field.SourceFileId].Contains(field.SourceColumnName))
                        {
                            errors.Add($"Pole '{field.Name}': Kolumna '{field.SourceColumnName}' nie istnieje w pliku '{field.SourceFileId}'.");
                        }
                    }
                }
            }
            else if (node is IMappingContainer container)
            {
                if (node is MappingObject obj && obj.RelationId.HasValue)
                {
                    if (!relations.Any(r => r.Id == obj.RelationId.Value))
                        errors.Add($"Obiekt '{obj.Name}': Odwołuje się do nieistniejącej relacji ID {obj.RelationId}.");
                }
                else if (node is MappingArray arr && arr.RelationId.HasValue)
                {
                    if (!relations.Any(r => r.Id == arr.RelationId.Value))
                        errors.Add($"Tablica '{arr.Name}': Odwołuje się do nieistniejącej relacji ID {arr.RelationId}.");
                }

                foreach (var child in container.Children)
                {
                    ValidateNodeRecursively(child, fileHeaders, relations, errors);
                }
            }
        }

        private List<string> GetFileHeaders(FileSourceDefinition fileDef)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = DetectDelimiter(fileDef.FilePath),
                HasHeaderRecord = false 
            };

            using (var reader = new StreamReader(fileDef.FilePath))
            using (var csv = new CsvReader(reader, config))
            {
                for (int i = 0; i <= fileDef.HeaderRowIndex; i++)
                {
                    if (!csv.Read()) throw new Exception("Plik jest zbyt krótki.");
                }

                var record = csv.Context.Parser.Record;
                return record.ToList();
            }
        }

        private string DetectDelimiter(string filePath)
        {
            var delimiters = new[] { ",", ";", "\t", "|" };
            string firstLine = File.ReadLines(filePath).FirstOrDefault();
            if (string.IsNullOrEmpty(firstLine)) return ",";

            return delimiters
                .OrderByDescending(d => firstLine.Count(c => c.ToString() == d))
                .FirstOrDefault();
        }

        public (List<CsvSourceFile> Files, List<Relation> Relations, MappingNode RootNode, bool IncludeNullValues) ProcessConfiguration(ProjectConfiguration config)
        {
            var loadedFiles = new List<CsvSourceFile>();

            if (config.Files != null)
            {
                foreach (var fileDef in config.Files)
                {
                    if (!File.Exists(fileDef.FilePath))
                    {
                        throw new FileNotFoundException($"Nie znaleziono pliku źródłowego: {fileDef.FilePath}");
                    }

                    var csvFile = _parsingService.LoadRawCsv(fileDef.FilePath);

                    csvFile.FileName = fileDef.FileName;

                    csvFile.HeaderRowIndex = fileDef.HeaderRowIndex;
                    csvFile.MetadataRowIndices = fileDef.MetadataRowIndices;
                    csvFile.IsRootFile = fileDef.IsRootFile;
                    csvFile.PrimaryKeyColumns = fileDef.PrimaryKeyColumns;

                    _parsingService.ProcessData(csvFile);

                    if (fileDef.ColumnTypeOverrides != null)
                    {
                        foreach (var kvp in fileDef.ColumnTypeOverrides)
                        {
                            if (csvFile.DetectedColumnTypes.ContainsKey(kvp.Key))
                            {
                                csvFile.DetectedColumnTypes[kvp.Key] = kvp.Value;
                            }
                        }
                    }

                    loadedFiles.Add(csvFile);
                }
            }

            var relations = config.Relations ?? new List<Relation>();
            var rootNode = config.RootNode ?? new MappingObject { Name = "root" };
            var includeNullValues = config.IncludeNullValues;

            return (loadedFiles, relations, rootNode, includeNullValues);
        }
    }
}
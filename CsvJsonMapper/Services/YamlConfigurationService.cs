using CsvJsonMapper.Models;
using CsvJsonMapper.Models.Configuration;
using CsvJsonMapper.Models.Mapping;
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

        public void SaveConfiguration(string filePath, List<CsvSourceFile> files, List<Relation> relations, MappingNode rootNode)
        {
            var config = new ProjectConfiguration
            {
                Relations = relations,
                RootNode = rootNode
            };

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

            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .WithTagMapping("!field", typeof(MappingField))
                .WithTagMapping("!object", typeof(MappingObject))
                .WithTagMapping("!array", typeof(MappingArray))
                .Build();

            string yaml = serializer.Serialize(config);
            File.WriteAllText(filePath, yaml);
        }

        public (List<CsvSourceFile> Files, List<Relation> Relations, MappingNode RootNode) LoadConfiguration(string filePath)
        {
            string yaml = File.ReadAllText(filePath);

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .IgnoreUnmatchedProperties()
                .WithTagMapping("!field", typeof(MappingField))
                .WithTagMapping("!object", typeof(MappingObject))
                .WithTagMapping("!array", typeof(MappingArray))
                .Build();

            var config = deserializer.Deserialize<ProjectConfiguration>(yaml);

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

            return (loadedFiles, relations, rootNode);
        }
    }
}
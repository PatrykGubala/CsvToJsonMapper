using CsvJsonMapper.Models.Mapping;

namespace CsvJsonMapper.Models.Configuration
{
    public class ProjectConfiguration
    {
        public List<FileSourceDefinition> Files { get; set; } = new List<FileSourceDefinition>();
        public List<Relation> Relations { get; set; } = new List<Relation>();
        public MappingNode RootNode { get; set; }
        public bool IncludeNullValues { get; set; }
 
    }

    public class FileSourceDefinition
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public bool IsRootFile { get; set; }
        public int HeaderRowIndex { get; set; }
        public List<int> MetadataRowIndices { get; set; }
        public Dictionary<string, string> ColumnTypeOverrides { get; set; } = new Dictionary<string, string>();
        public List<string> PrimaryKeyColumns { get; set; } = new List<string>();
    }
}
namespace CsvJsonMapper.Models.Mapping
{
    public class MappingField : MappingNode
    {
        public string SourceFileId { get; set; }
        public string SourceColumnName { get; set; }
        public string SourceColumnType { get; set; } = "string";
    }
}
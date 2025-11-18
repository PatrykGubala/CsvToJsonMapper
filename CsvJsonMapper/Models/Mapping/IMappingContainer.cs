namespace CsvJsonMapper.Models.Mapping
{
    public interface IMappingContainer
    {
        List<MappingNode> Children { get; set; }
    }
}
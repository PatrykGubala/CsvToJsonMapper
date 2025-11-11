namespace CsvJsonMapper.Models.Mapping
{
    public class MappingArray : MappingNode, IMappingContainer
    {
        public List<MappingNode> Children { get; set; }
        public Guid? RelationId { get; set; }

        public MappingArray()
        {
            Children = new List<MappingNode>();
        }
    }
}
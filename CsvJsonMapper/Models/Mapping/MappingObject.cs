namespace CsvJsonMapper.Models.Mapping
{
    public class MappingObject : MappingNode, IMappingContainer
    {
        public List<MappingNode> Children { get; set; }
        public Guid? RelationId { get; set; }

        public MappingObject()
        {
            Children = new List<MappingNode>();
        }
    }
}
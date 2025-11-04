using System.Collections.Generic;

namespace CsvJsonMapper.Models.Mapping
{
    public class MappingArray : MappingNode, IMappingContainer
    {
        public List<MappingNode> Children { get; set; }

        public MappingArray()
        {
            Children = new List<MappingNode>();
        }
    }
}
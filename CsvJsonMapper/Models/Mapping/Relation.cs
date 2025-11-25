using System.ComponentModel;
using YamlDotNet.Serialization;

namespace CsvJsonMapper.Models.Mapping
{
    public class Relation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ParentFileId { get; set; }
        public string ChildFileId { get; set; }
        public List<string> ParentKeyColumns { get; set; }
        public List<string> ChildKeyColumns { get; set; }
        public RelationType Type { get; set; }

        public Relation()
        {
            Id = Guid.NewGuid();
            ParentKeyColumns = new List<string>();
            ChildKeyColumns = new List<string>();
        }

        public override string ToString()
        {
            return Name;
        }

        [Browsable(false)]
        [YamlIgnore]
        public string ParentKeyDisplay => string.Join(", ", ParentKeyColumns);

        [Browsable(false)]
        [YamlIgnore]
        public string ChildKeyDisplay => string.Join(", ", ChildKeyColumns);

        [YamlIgnore]
        public string Description => $"{ParentFileId} ({ParentKeyDisplay}) -> {ChildFileId} ({ChildKeyDisplay}) [{Type}]";
    }
}
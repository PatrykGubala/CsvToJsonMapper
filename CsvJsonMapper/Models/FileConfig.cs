namespace CsvJsonMapper.Models
{
    public class FileConfig
    {
        public int HeaderRow { get; set; } = 1;
        public string MetadataRows { get; set; } = "";
        public Dictionary<string, string> ColumnTypeOverrides { get; set; }

        public FileConfig()
        {
            ColumnTypeOverrides = new Dictionary<string, string>();
        }
    }
}
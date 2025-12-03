using System.Data;

namespace CsvJsonMapper.Models
{
    public class CsvSourceFile
    {
        public string FilePath { get; set; }
        
        private string _fileNameOverride;
        public string FileName 
        { 
            get => _fileNameOverride ?? Path.GetFileName(FilePath);
            set => _fileNameOverride = value;
        }

        public string Delimiter { get; set; }
        
        public bool IsRootFile { get; set; }
        public int HeaderRowIndex { get; set; }
        public List<int> MetadataRowIndices { get; set; }
        
        public DataTable RawData { get; set; }
        public DataTable ProcessedData { get; set; }
        public List<string> Headers { get; set; }
        public Dictionary<string, string> DetectedColumnTypes { get; set; }
        public List<string> PrimaryKeyColumns { get; set; }

        public CsvSourceFile()
        {
            RawData = new DataTable();
            ProcessedData = new DataTable();
            Headers = new List<string>();
            MetadataRowIndices = new List<int>();
            DetectedColumnTypes = new Dictionary<string, string>();
            PrimaryKeyColumns = new List<string>();
            HeaderRowIndex = 0;
            IsRootFile = false;
        }

        public override string ToString()
        {
            return FileName;
        }
    }
}
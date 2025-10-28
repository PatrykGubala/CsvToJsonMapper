using System.Collections.Generic;
using System.Data;
using System.IO;

namespace CsvJsonMapper.Models
{
    public class CsvSourceFile
    {
        public string FilePath { get; set; }
        public string FileName => Path.GetFileName(FilePath);
        public string Delimiter { get; set; }
        
        public bool IsRootFile { get; set; }
        public int HeaderRowIndex { get; set; }
        public List<int> MetadataRowIndices { get; set; }
        
        public DataTable RawData { get; set; }
        public DataTable ProcessedData { get; set; }
        public List<string> Headers { get; set; }

        public CsvSourceFile()
        {
            RawData = new DataTable();
            ProcessedData = new DataTable();
            Headers = new List<string>();
            MetadataRowIndices = new List<int>();
            HeaderRowIndex = 0;
            IsRootFile = false;
        }
    }
}
using CsvHelper;
using CsvHelper.Configuration;
using CsvJsonMapper.Models;
using System.Data;
using System.Globalization;

namespace CsvJsonMapper.Services
{
    public class CsvParsingService
    {
        private const int PREVIEW_ROW_LIMIT = 50;

        public void ValidateFileStructure(string filePath, int headerRowIndex, string delimiter = null)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Plik nie istnieje: {filePath}");

            if (new FileInfo(filePath).Length == 0)
                throw new Exception($"Plik jest pusty: {filePath}");

            string actualDelimiter = delimiter ?? DetectDelimiter(filePath);
            if (string.IsNullOrEmpty(actualDelimiter))
                throw new Exception($"Nie udało się wykryć separatora w pliku: {filePath}");

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = actualDelimiter,
                HasHeaderRecord = false
            };

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                int rowCount = 0;
                while (rowCount <= headerRowIndex)
                {
                    if (!csv.Read())
                        throw new Exception($"Plik {Path.GetFileName(filePath)} ma zbyt mało wierszy, aby znaleźć nagłówek w wierszu {headerRowIndex + 1}.");
                    rowCount++;
                }

                if (csv.Context.Parser.Record == null || csv.Context.Parser.Record.Length == 0)
                    throw new Exception($"Wiersz nagłówkowy w pliku {Path.GetFileName(filePath)} jest pusty.");
            }
        }

        public CsvSourceFile LoadRawCsv(string filePath)
        {
            var sourceFile = new CsvSourceFile { FilePath = filePath };

            sourceFile.Delimiter = DetectDelimiter(filePath);
            if (string.IsNullOrEmpty(sourceFile.Delimiter))
            {
                throw new Exception($"Nie można automatycznie wykryć separatora dla pliku: {sourceFile.FileName}");
            }

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = sourceFile.Delimiter,
                HasHeaderRecord = false
            };

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                if (!csv.Read())
                {
                    return sourceFile;
                }

                var firstRecord = csv.Context.Parser.Record;
                if (firstRecord == null) return sourceFile;

                int columnCount = firstRecord.Length;
                for (int i = 0; i < columnCount; i++)
                {
                    sourceFile.RawData.Columns.Add($"Column{i + 1}");
                }

                var row = sourceFile.RawData.NewRow();
                row.ItemArray = firstRecord;
                sourceFile.RawData.Rows.Add(row);

                int rowsRead = 1;
                while (csv.Read() && rowsRead < PREVIEW_ROW_LIMIT)
                {
                    var nextRow = sourceFile.RawData.NewRow();
                    var currentRecord = csv.Context.Parser.Record;
                    
                    var targetArray = new object[columnCount];
                    Array.Copy(currentRecord, targetArray, Math.Min(currentRecord.Length, targetArray.Length));
                    
                    nextRow.ItemArray = targetArray;
                    sourceFile.RawData.Rows.Add(nextRow);
                    rowsRead++;
                }
            }
            return sourceFile;
        }

        public void ProcessData(CsvSourceFile file)
        {
            file.ProcessedData = new DataTable();
            file.Headers.Clear();

            var rawTable = file.RawData;
            if (rawTable.Rows.Count == 0 || file.HeaderRowIndex >= rawTable.Rows.Count) return;

            DataRow headerRow = rawTable.Rows[file.HeaderRowIndex];
            var headers = new List<string>();
            foreach (var item in headerRow.ItemArray)
            {
                string header = item.ToString();
                if (string.IsNullOrWhiteSpace(header)) header = "EmptyHeader";
                
                string uniqueHeader = header;
                int suffix = 1;
                while (headers.Contains(uniqueHeader))
                {
                    uniqueHeader = $"{header}_{suffix++}";
                }
                headers.Add(uniqueHeader);
                file.ProcessedData.Columns.Add(uniqueHeader, typeof(string));
            }
            file.Headers.AddRange(headers);

            for (int i = 0; i < rawTable.Rows.Count; i++)
            {
                if (i == file.HeaderRowIndex || file.MetadataRowIndices.Contains(i))
                {
                    continue;
                }

                var rawRow = rawTable.Rows[i];
                var newRow = file.ProcessedData.NewRow();
                
                var sourceArray = rawRow.ItemArray;
                var targetArray = new object[file.ProcessedData.Columns.Count];
                
                Array.Copy(sourceArray, targetArray, Math.Min(sourceArray.Length, targetArray.Length));
                
                newRow.ItemArray = targetArray;
                file.ProcessedData.Rows.Add(newRow);
            }

            DetectColumnTypes(file);
        }

        private void DetectColumnTypes(CsvSourceFile file)
        {
            file.DetectedColumnTypes.Clear();
            foreach (DataColumn column in file.ProcessedData.Columns)
            {
                bool isInt = true;
                bool isDouble = true;

                foreach (DataRow row in file.ProcessedData.Rows)
                {
                    string value = row[column] as string;
                    if (string.IsNullOrEmpty(value)) continue;

                    if (isInt && !int.TryParse(value, out _))
                    {
                        isInt = false;
                    }
                    if (isDouble && !double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                    {
                        isDouble = false;
                    }
                    if (!isInt && !isDouble) break;
                }

                if (isInt)
                {
                    file.DetectedColumnTypes[column.ColumnName] = "int";
                }
                else if (isDouble)
                {
                    file.DetectedColumnTypes[column.ColumnName] = "double";
                }
                else
                {
                    file.DetectedColumnTypes[column.ColumnName] = "string";
                }
            }
        }

        private string DetectDelimiter(string filePath)
        {
            var delimiters = new[] { ",", ";", "\t", "|" };
            string firstLine = File.ReadLines(filePath).FirstOrDefault();

            if (string.IsNullOrEmpty(firstLine))
            {
                return null;
            }

            var bestDelimiter = delimiters
                .Select(d => new { Delimiter = d, Count = firstLine.Count(c => c.ToString() == d) })
                .OrderByDescending(x => x.Count)
                .FirstOrDefault();

            return bestDelimiter?.Count > 0 ? bestDelimiter.Delimiter : null;
        }
    }
}
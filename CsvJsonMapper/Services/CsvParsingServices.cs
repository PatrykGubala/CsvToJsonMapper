using CsvHelper;
using CsvHelper.Configuration;
using CsvJsonMapper.Models;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;

namespace CsvJsonMapper.Services
{
    public class CsvParsingService
    {
        public CsvSourceFile LoadRawCsv(string filePath)
        {
            var sourceFile = new CsvSourceFile { FilePath = filePath };

            sourceFile.Delimiter = DetectDelimiter(filePath);
            if (string.IsNullOrEmpty(sourceFile.Delimiter))
            {
                throw new System.Exception($"Nie można automatycznie wykryć separatora dla pliku: {sourceFile.FileName}");
            }

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = sourceFile.Delimiter,
                HasHeaderRecord = false
            };

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                using (var dr = new CsvDataReader(csv))
                {
                    sourceFile.RawData.Load(dr);
                }
            }
            return sourceFile;
        }

        public void ProcessData(CsvSourceFile file)
        {
            file.ProcessedData = new DataTable();
            file.Headers.Clear();

            var rawTable = file.RawData;
            if (rawTable.Rows.Count == 0) return;

            DataRow headerRow = rawTable.Rows[file.HeaderRowIndex];
            var headers = new List<string>();
            foreach (var item in headerRow.ItemArray)
            {
                string header = item.ToString();
                string uniqueHeader = header;
                int suffix = 1;
                while (headers.Contains(uniqueHeader))
                {
                    uniqueHeader = $"{header}_{suffix++}";
                }
                headers.Add(uniqueHeader);
                file.ProcessedData.Columns.Add(uniqueHeader);
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
                newRow.ItemArray = rawRow.ItemArray;
                file.ProcessedData.Rows.Add(newRow);
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
using CsvJsonMapper.Models.Mapping;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CsvJsonMapper.Services
{
    public static class TransformationHelper
    {
        public static object ProcessValue(MappingField field, DataRow row)
        {
            string rawValue = null;
            
            if (!string.IsNullOrEmpty(field.SourceColumnName) && row.Table.Columns.Contains(field.SourceColumnName))
            {
                rawValue = row[field.SourceColumnName]?.ToString();
            }

            string processedString = ApplyTransformation(rawValue, field, row);
            return ConvertValue(processedString, field.SourceColumnType);
        }

        public static string ApplyTransformation(string value, MappingField field, DataRow row)
        {
            if (field.TransformationType == TransformationType.CombineFields)
            {
                if (string.IsNullOrEmpty(field.TransformationPattern)) return value;
                
                return Regex.Replace(field.TransformationPattern, @"\{(.+?)\}", m =>
                {
                    string colName = m.Groups[1].Value;
                    if (row.Table.Columns.Contains(colName))
                    {
                        return row[colName]?.ToString() ?? "";
                    }
                    return m.Value;
                });
            }

            if (string.IsNullOrEmpty(value)) return null;

            switch (field.TransformationType)
            {
                case TransformationType.Trim:
                    return value.Trim();
                
                case TransformationType.ToUpper:
                    return value.ToUpper();
                
                case TransformationType.ToLower:
                    return value.ToLower();

                case TransformationType.SplitBySeparator:
                    if (!string.IsNullOrEmpty(field.TransformationPattern))
                    {
                        var parts = value.Split(new[] { field.TransformationPattern }, StringSplitOptions.None);
                        if (field.SplitIndex >= 0 && field.SplitIndex < parts.Length)
                        {
                            return parts[field.SplitIndex].Trim();
                        }
                        return null;
                    }
                    break;

                case TransformationType.SplitByRegex:
                    if (!string.IsNullOrEmpty(field.TransformationPattern))
                    {
                        var match = Regex.Match(value, field.TransformationPattern);
                        if (match.Success)
                        {
                            if (field.SplitIndex < match.Groups.Count)
                            {
                                return match.Groups[field.SplitIndex].Value.Trim();
                            }
                        }
                        return null; 
                    }
                    break;
            }

            return value;
        }

        private static object ConvertValue(string value, string type)
        {
            if (value == null) return null;

            switch (type?.ToLower())
            {
                case "int":
                case "integer":
                    if (int.TryParse(value, out int intVal)) return intVal;
                    return null;
                case "double":
                case "float":
                case "decimal":
                    if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double doubleVal)) return doubleVal;
                    return null;
                case "bool":
                case "boolean":
                    if (bool.TryParse(value, out bool boolVal)) return boolVal;
                    return null;
                default:
                    return value;
            }
        }
    }
}
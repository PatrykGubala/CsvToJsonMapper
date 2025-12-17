using System.ComponentModel;

namespace CsvJsonMapper.Models.Mapping
{
    public enum TransformationType
    {
        None,
        SplitBySeparator,
        SplitByRegex,
        CombineFields,
        ToUpper,
        ToLower,
        Trim
    }

    public class MappingField : MappingNode
    {
        [Category("Dane Źródłowe")]
        [DisplayName("ID Pliku")]
        [ReadOnly(true)]
        public string SourceFileId { get; set; }

        [Category("Dane Źródłowe")]
        [DisplayName("Główna Kolumna")]
        [Description("Dla Split: kolumna do podziału. Dla Combine: pole może być puste (użyj Wzorca).")]
        [ReadOnly(false)] 
        public string SourceColumnName { get; set; }

        [Category("Dane Źródłowe")]
        [DisplayName("Typ Danych")]
        [Description("Typ danych (string, int, double, boolean).")]
        [TypeConverter(typeof(StringConverter))]
        public string SourceColumnType { get; set; } = "string";

        [Category("Transformacje")]
        [DisplayName("Rodzaj Transformacji")]
        [Description("Wybierz metodę obróbki danych.")]
        [DefaultValue(TransformationType.None)]
        public TransformationType TransformationType { get; set; } = TransformationType.None;

        [Category("Transformacje")]
        [DisplayName("Wzorzec Transformacji")]
        [Description("SplitBySeparator: separator (np. '-'). SplitByRegex: regex (np. '^(.+) (.+)$'). CombineFields: szablon (np. '{Marka} {Model}').")]
        public string TransformationPattern { get; set; }

        [Category("Transformacje")]
        [DisplayName("Indeks / Grupa")]
        [Description("Dla SplitBySeparator: numer elementu (0, 1...). Dla SplitByRegex: numer grupy regex.")]
        [DefaultValue(0)]
        public int SplitIndex { get; set; } = 0;
    }
}
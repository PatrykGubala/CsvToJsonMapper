using System.Globalization;

namespace CsvJsonMapper.Services
{
    public static class SortCompareHelper
    {
        public static int Compare(object valA, object valB, string type)
        {
            string a = valA as string ?? "";
            string b = valB as string ?? "";

            if (type == "int")
            {
                bool isAInt = int.TryParse(a, out int intA);
                bool isBInt = int.TryParse(b, out int intB);
                if (isAInt && isBInt) return intA.CompareTo(intB);
                if (isAInt) return 1; 
                if (isBInt) return -1;
            }
            else if (type == "double")
            {
                bool isADouble = double.TryParse(a, NumberStyles.Any, CultureInfo.InvariantCulture, out double doubleA);
                bool isBDouble = double.TryParse(b, NumberStyles.Any, CultureInfo.InvariantCulture, out double doubleB);
                if (isADouble && isBDouble) return doubleA.CompareTo(doubleB);
                if (isADouble) return 1;
                if (isBDouble) return -1;
            }

            return string.Compare(a, b, StringComparison.Ordinal);
        }
    }
}
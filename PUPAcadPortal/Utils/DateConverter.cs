using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Globalization;

namespace PUPAcadPortal.Utils
{
    public class DateConverter : DefaultTypeConverter
    {
        private static readonly string[] CsvDateFormats = {
            "yyyy-MM-dd", "M/d/yyyy", "M-d-yyyy", "M/d/yy",
            "M-d-yy", "dd/MM/yyyy", "dd-MM-yyyy", "dd-MMMM-yyyy",
            "d-MMM-yy", "dd-MMM-yy", "d-MMM-yyyy", "dd-MMM-yyyy"
        };

        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrWhiteSpace(text)) return DateTime.MinValue;

            if (DateTime.TryParseExact(text, CsvDateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                return parsedDate;
            }
            throw new FormatException($"'{text}' is not a valid date format.");
        }
    }
}

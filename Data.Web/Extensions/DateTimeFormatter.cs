using System.Globalization;

namespace Cantin.Data.Extensions
{
    public static class DateTimeFormatter
    {
        public static string FormatForTr(DateTime? dateTime) {
            CultureInfo culture = new CultureInfo("tr-TR");
            string formattedDateTime = "";
            if (dateTime.HasValue) {
                DateTime x = dateTime.Value;
                formattedDateTime = x.ToString("dd.MM.yyyy HH:mm:ss", culture);
            }
            return formattedDateTime;

        }
    }
}

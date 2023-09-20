using System.Globalization;

namespace Cantin.Core.Helpers
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
        public static DateTime? GetDateTimeForFiltersForStartDate(string? filterDateTime)
        {
            if (filterDateTime == null || filterDateTime == string.Empty)
            {
                return null;
            }
            if (filterDateTime == "today")
            {
                return DateTime.Now.Date;
            }
            if (filterDateTime == "thisWeek")
            {
                return DateTime.Now.AddDays(-7);
            }
            if (filterDateTime == "thisMonth")
            {
                return DateTime.Now.AddMonths(-1);
            }
            if (filterDateTime == "thisYear")
            {
                return DateTime.Now.AddYears(-1);
            }
            return Convert.ToDateTime(filterDateTime);
        }
            //bugün /bu hafta/ bu ay/ bu yıl /hepsi frontendden null olarak gelecek

            public static DateTime? GetDateTimeForFiltersForFinishDate(string? filterDateTime)
            {
                if (filterDateTime == null || filterDateTime == string.Empty || filterDateTime == "today")
                {
                    return null;
                }
                if (filterDateTime == "thisWeek")
                {
                    return DateTime.Now;
                }
                if (filterDateTime == "thisMonth")
                {
                    return DateTime.Now;
                }
                if (filterDateTime == "thisYear")
                {
                    return DateTime.Now;
                }
                return Convert.ToDateTime(filterDateTime);
                //bugün /bu hafta/ bu ay/ bu yıl /hepsi frontendden null olarak gelecek
            }
        }
}

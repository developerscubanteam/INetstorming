using System.Globalization;

namespace System
{
    public static class DateTimeExtension
    {
        public static bool TryParseApiFormat(this string value)
        {
            DateTime dateValue = DateTime.UtcNow;
            return DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue);
        }

        public static string ToFormat_yyyyMMdd(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        public static string ToApplicationPatternFormat(this DateTime date, string pattern)
        {
            return date.ToString(pattern);
        }
        public static string ToHourFormat_HHmm(this DateTime date)
        {
            return date.ToString("HH:mm");
        }

        public static string ToFormat_yyyyMMdd_HHmm(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm");
        }

        public static string ToFormat_yyyyMMdd_HHmmssfff(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        public static string ToHourFormat_HHmm(this TimeSpan time)
        {
            return string.Format("{0:hh\\:mm}", time);
        }

        public static string ToTimesStamp(this DateTime date)
        {
            return date.ToString("yyyyMMddHHmmss");
        }

        public static string GetTimeStamp()
        {
            return ToTimesStamp(DateTime.Now);
        }
    }
}

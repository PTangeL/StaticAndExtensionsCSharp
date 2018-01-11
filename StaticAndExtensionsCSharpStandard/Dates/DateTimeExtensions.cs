using System;
using System.Globalization;

namespace StaticAndExtensionsCSharpStandard.Dates
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Toes the date time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultvalue">The defaultvalue.</param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string value, DateTime? defaultvalue = null) => 
            DateTime.TryParse(value, out var result) ? result : defaultvalue;

        #region Elapsed extension
        /// <summary>
        /// Elapseds the time.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <returns>TimeSpan</returns>
        public static TimeSpan Elapsed(this DateTime datetime) => DateTime.Now - datetime;
        #endregion

        #region Week of year
        /// <summary>
        /// Weeks the of year.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="weekrule">The weekrule.</param>
        /// <param name="firstDayOfWeek">The first day of week.</param>
        /// <returns></returns>
        public static int WeekOfYear(this DateTime datetime, CalendarWeekRule weekrule, DayOfWeek firstDayOfWeek)
        {
            var ciCurr = CultureInfo.CurrentCulture;
            return ciCurr.Calendar.GetWeekOfYear(datetime, weekrule, firstDayOfWeek);
        }
        /// <summary>
        /// Weeks the of year.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="firstDayOfWeek">The first day of week.</param>
        /// <returns></returns>
        public static int WeekOfYear(this DateTime datetime, DayOfWeek firstDayOfWeek)
        {
            var dateinf = new DateTimeFormatInfo();
            var weekrule = dateinf.CalendarWeekRule;
            return WeekOfYear(datetime, weekrule, firstDayOfWeek);
        }
        /// <summary>
        /// Weeks the of year.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="weekrule">The weekrule.</param>
        /// <returns></returns>
        public static int WeekOfYear(this DateTime datetime, CalendarWeekRule weekrule)
        {
            var dateinf = new DateTimeFormatInfo();
            var firstDayOfWeek = dateinf.FirstDayOfWeek;
            return WeekOfYear(datetime, weekrule, firstDayOfWeek);
        }

        /// <summary>
        /// Weeks the of year.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <returns></returns>
        public static int WeekOfYear(this DateTime datetime)
        {
            var dateinf = new DateTimeFormatInfo();
            var weekrule = dateinf.CalendarWeekRule;
            var firstDayOfWeek = dateinf.FirstDayOfWeek;
            return WeekOfYear(datetime, weekrule, firstDayOfWeek);
        }
        #endregion

        #region Get Datetime for Day of Week
        /// <summary>
        /// Gets the date time for day of week.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="day">The day.</param>
        /// <param name="firstDayOfWeek">The first day of week.</param>
        /// <returns></returns>
        public static DateTime GetDateTimeForDayOfWeek(this DateTime datetime, DayOfWeek day, 
            DayOfWeek firstDayOfWeek)
        {
            var current = DateTimeUtils.DaysFromFirstDayOfWeek(datetime.DayOfWeek, firstDayOfWeek);
            var resultday = DateTimeUtils.DaysFromFirstDayOfWeek(day, firstDayOfWeek);
            return datetime.AddDays(resultday - current);
        }
        public static DateTime GetDateTimeForDayOfWeek(this DateTime datetime, DayOfWeek day)
        {
            var dateinf = new DateTimeFormatInfo();
            var firstDayOfWeek = dateinf.FirstDayOfWeek;
            return GetDateTimeForDayOfWeek(datetime, day, firstDayOfWeek);
        }
        /// <summary>
        /// Firsts the date time of week.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <returns></returns>
        public static DateTime FirstDateTimeOfWeek(this DateTime datetime)
        {
            var dateinf = new DateTimeFormatInfo();
            var firstDayOfWeek = dateinf.FirstDayOfWeek;
            return FirstDateTimeOfWeek(datetime, firstDayOfWeek);
        }
        /// <summary>
        /// Firsts the date time of week.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="firstDayOfWeek">The first day of week.</param>
        /// <returns></returns>
        public static DateTime FirstDateTimeOfWeek(this DateTime datetime, DayOfWeek firstDayOfWeek) => 
            datetime.AddDays(-DateTimeUtils.DaysFromFirstDayOfWeek(datetime.DayOfWeek, firstDayOfWeek));
        #endregion

        public static string GetValueOrDefaultToString(this DateTime? datetime, 
            string defaulVvalue, string format = "") =>
            datetime == null ? defaulVvalue : datetime.Value.ToString(format);
    }
}
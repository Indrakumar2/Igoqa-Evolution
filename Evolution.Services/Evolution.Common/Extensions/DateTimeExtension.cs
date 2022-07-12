using Evolution.Common.Helpers;
using System;

namespace Evolution.Common.Extensions
{
    public static class DateTimeExtension
    {
        public static long ConvertToUnixTimestamp(this DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return(long) Math.Floor(diff.TotalSeconds);
        }

        /// <summary>
        /// Returns TimeZone adjusted time for a given from a Utc or local time.
        /// Date is first converted to UTC then adjusted.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeZoneId"></param>
        /// <returns></returns>
        public static DateTime ToTimeZoneTime(this DateTime time, string timeZoneId)
        {
            TimeZoneInfo tzi = DateTimeHelper.ValidateTimeZoneAndReturnDefault(timeZoneId);
            return time.ToTimeZoneTime(tzi);
        }

        /// <summary>
        /// Returns TimeZone adjusted time for a given from a Utc or local time.
        /// Date is first converted to UTC then adjusted.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeZoneId"></param>
        /// <returns></returns>
        public static DateTime ToTimeZoneTime(this DateTime time, TimeZoneInfo tzi)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(time, tzi);
        }

        public static string ToDateFormat(this DateTime date,string format)
        {
            return date.ToString(format);
        }

        public static int GetQuarter(this DateTime date)
        {
            return (date.Month + 2) / 3;
        }

        public static int GetPreviousQuarter(this DateTime date)
        {
            return (int)Math.Ceiling((double)date.AddMonths(-3).Month / (double)3);
        }
    }
}

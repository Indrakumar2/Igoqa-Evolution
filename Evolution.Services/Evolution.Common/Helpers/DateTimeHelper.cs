using System;

namespace Evolution.Common.Helpers
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// If passed timezone is invalid then will return default time zone
        /// </summary>
        /// <param name="timeZoneName"></param>
        public static TimeZoneInfo ValidateTimeZoneAndReturnDefault(string timeZoneName)
        {
            TimeZoneInfo timeZone = null;
            try
            {
                timeZone = string.IsNullOrEmpty(timeZoneName) ? null : TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            if (timeZone == null)
                timeZone = TimeZoneInfo.Local;

            return timeZone;
        }
    }
}

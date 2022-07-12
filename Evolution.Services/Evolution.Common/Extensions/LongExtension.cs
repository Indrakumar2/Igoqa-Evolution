using System;

namespace Evolution.Common.Extensions
{
    public static class LongExtension
    {
        public static DateTime ConvertFromUnixTimestamp(this long timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return origin.AddSeconds(timestamp);
        }
    }
}

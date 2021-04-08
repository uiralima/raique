using System;

namespace Raique.Library
{
    public static class DateUtilities
    {
        private static TimeZoneInfo _timeZoneInfo;
        public static DateTime Now
        {
            get
            {
                if (_timeZoneInfo == null)
                    ConfigureTimeZoneInfo();
                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZoneInfo);
            }
        }
        private static void ConfigureTimeZoneInfo()
        {
            _timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
        }
        public static DateTime FromDateStr(string dateStr)
        {
            //Esperado formato yyyyMMdd
            dateStr = StringUtilities.OnlyNumbersStr(dateStr);
            if (dateStr.Length < 8)
            {
                throw new Exception("Data em um formato inválido");
            }
            int year = Convert.ToInt32(dateStr.Substring(0, 4));
            int mont = Convert.ToInt32(dateStr.Substring(4, 2));
            int day = Convert.ToInt32(dateStr.Substring(6, 2));
            return new DateTime(year, mont, day);
        }
        public static DateTime FromDateTimeStr(string dateStr)
        {
            //Esperado formato yyyyMMddHHmmss
            dateStr = StringUtilities.OnlyNumbersStr(dateStr);
            if (dateStr.Length < 14)
            {
                throw new Exception("Data em um formato inválido");
            }
            int year = Convert.ToInt32(dateStr.Substring(0, 4));
            int mont = Convert.ToInt32(dateStr.Substring(4, 2));
            int day = Convert.ToInt32(dateStr.Substring(6, 2));
            int hour = Convert.ToInt32(dateStr.Substring(8, 2));
            int minute = Convert.ToInt32(dateStr.Substring(10, 2));
            int second = Convert.ToInt32(dateStr.Substring(12, 2));
            return new DateTime(year, mont, day, hour, minute, second);
        }
    }
}

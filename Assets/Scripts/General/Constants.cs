using System.Collections.Generic;

namespace General
{
    public class Constants
    {
        public static string TIMEAPI_REQUEST = "https://timeapi.io/api/time/current/zone?timeZone=Europe%2FMoscow";
        public static string JSONTEST_REQUEST = "http://time.jsontest.com/";
        
        public static List<string> KEYS = new() { "hour", "minute", "seconds", "hours", "minutes", "milliseconds_since_epoch" };

        public static string ALARM_STORAGE_NAME_HOURS = "alarm_hours";
        public static string ALARM_STORAGE_NAME_MINUTES = "alarm_minutes";
    }
}
using System;

namespace PingdomAlertShared.PingdomModel
{
    public class State
    {
        public string status { get; set; }
        public int timefrom { get; set; }
        public int timeto { get; set; }

        public DateTime TimeFromDate
        {
            get { return DateTimeConverter.UnixTimeStampToDateTime(timefrom); }
        }

        public string TimeFromDateFormatted
        {
            get { return TimeFromDate.ToString("MM/dd"); }
        }

        public int Value
        {
            get
            {
                return status == "up" ? 1 : 0;
            }
        }
    }
}
using System;

namespace PingdomAlertShared.PingdomModel
{
    public class Day
    {
        public int avgresponse { get; set; }
        public int starttime { get; set; }

        public DateTime StartTimeResponse
        {
            get { return DateTimeConverter.UnixTimeStampToDateTime(starttime); }
        }

        public string StartTimeResponseFormatted
        {
            get { return StartTimeResponse.ToString("MM/dd"); }
        }
    }
}
namespace PingdomAlertShared.PingdomModel
{
    public class ResponseTime
    {
        public int from { get; set; }
        public int to { get; set; }
        public int avgresponse { get; set; }

        public string AvgResponseFormatted
        {
            get { return string.Format("{0} ms", avgresponse); }
        }
    }
}
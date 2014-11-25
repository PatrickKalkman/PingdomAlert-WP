namespace PingdomAlertShared.PingdomModel
{
    public class Banner
    {
        public string id { get; set; }
        public string name { get; set; }
        public int checkid { get; set; }
        public bool auto { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public int fromyear { get; set; }
        public int frommonth { get; set; }
        public int fromday { get; set; }
        public int toyear { get; set; }
        public int tomonth { get; set; }
        public int today { get; set; }
    }
}
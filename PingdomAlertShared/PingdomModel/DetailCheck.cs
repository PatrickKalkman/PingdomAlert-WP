namespace PingdomAlertShared.PingdomModel
{
    public class DetailCheck
    {
        public int id { get; set; }
        public string name { get; set; }
        public int resolution { get; set; }
        public bool sendtoemail { get; set; }
        public bool sendtosms { get; set; }
        public bool sendtotwitter { get; set; }
        public bool sendtoiphone { get; set; }
        public int sendnotificationwhendown { get; set; }
        public int notifyagainevery { get; set; }
        public bool notifywhenbackup { get; set; }
        public int created { get; set; }
        public string hostname { get; set; }
        public string status { get; set; }
        public int lasterrortime { get; set; }
        public int lasttesttime { get; set; }

        public string FormattedLastErrorTime
        {
            get { return DateTimeConverter.UnixTimeStampToDateTime(lasterrortime).ToString(); }
        }

        public string FormattedLastTestTime
        {
            get { return DateTimeConverter.UnixTimeStampToDateTime(lasttesttime).ToString(); }
        }

        public string FormattedCreated
        {
            get { return DateTimeConverter.UnixTimeStampToDateTime(created).ToString(); }
        }
    }
}
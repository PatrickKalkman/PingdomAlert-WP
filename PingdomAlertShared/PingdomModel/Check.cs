namespace PingdomAlertShared.PingdomModel
{
    public class Check
    {
        public string hostname { get; set; }
        public int id { get; set; }
        public int lasterrortime { get; set; }
        public int lastresponsetime { get; set; }
        public int lasttesttime { get; set; }
        public string name { get; set; }
        public int resolution { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string Image
        {
            get
            {
                return status == "up" ? "../Assets/ServerStatusUp.png" : "../Assets/ServerStatusDown.png";
            }
        }

        public string StatusFormatted
        {
            get { return string.Format("( {0} )", status); }
        }

        public string SpecialStatusFormatted
        {
            get { return string.Format("{0} ( {1} )", hostname, status); }
        }


        public string LastErrorTimeFormatted
        {
            get
            {
                if (lasterrortime != 0)
                {
                    return DateTimeConverter.UnixTimeStampToDateTime(lasterrortime).ToString("g");
                }
                return "<none>";
            }
        }

        public string LastResponseTimeFormatted
        {
            get
            {
                if (lastresponsetime != 0)
                {
                    return string.Format("{0} ms", lastresponsetime);
                }
                return "<none>";
            }
        }

        public string LastTestTimeFormatted
        {
            get
            {
                if (lasttesttime != 0)
                {
                    return DateTimeConverter.UnixTimeStampToDateTime(lasttesttime).ToString("g");
                }
                return "<none>";
            }
        }
    }
}
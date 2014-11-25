namespace PingdomAlertShared.PingdomModel
{
    public class ResponseStatus
    {
        public int totalup { get; set; }
        public int totaldown { get; set; }
        public int totalunknown { get; set; }

        public string TotalupFormatted
        {
            get
            {
                double percentage = 1.0 - ((double)totaldown / (double)totalup);
                return string.Format("{0:##.##}%", percentage * 100);
            }
        }

        public string TotaldownFormatted
        {
            get
            {
                int hours = (int)(totaldown/3600);
                int minutes = (int) ((totaldown - (hours*3600))/60);

                return string.Format("{0}h {1}m", hours, minutes);
            }
        }
    }

}
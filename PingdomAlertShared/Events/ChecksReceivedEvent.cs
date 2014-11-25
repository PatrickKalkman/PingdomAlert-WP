using PingdomAlertShared.PingdomModel;

namespace PingdomAlertShared.Events
{
    public class ChecksReceivedEvent
    {
        public Checks Checks { get; set; }

        public bool HasError { get; set; }
    }
}
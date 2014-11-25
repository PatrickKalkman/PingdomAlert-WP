using PingdomAlertShared.PingdomModel;

namespace PingdomAlertShared.Events
{
    public class CheckDetailReceivedEvent
    {
        public DetailCheck DetailCheck { get; set; }
    }
}
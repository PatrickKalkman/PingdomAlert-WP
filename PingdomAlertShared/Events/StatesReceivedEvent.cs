using PingdomAlertShared.PingdomModel;

namespace PingdomAlertShared.Events
{
    public class StatesReceivedEvent
    {
        public StateRoot States { get; set; }
    }
}
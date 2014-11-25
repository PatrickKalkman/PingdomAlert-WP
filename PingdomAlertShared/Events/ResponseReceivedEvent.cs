using PingdomAlertShared.PingdomModel;

namespace PingdomAlertShared.Events
{
    public class ResponseReceivedEvent
    {
        public ResponseRoot Response { get; set; }
    }
}
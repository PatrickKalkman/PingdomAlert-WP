using PingdomAlertShared.PingdomModel;

namespace PingdomAlertShared.Events
{
    public class PerformanceReceivedEvent
    {
        public DayPerformance Performance { get; set; }
    }
}
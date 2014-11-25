namespace PingdomAlertShared.Events
{
    public class SettingsUpdateEvent
    {
        public PingdomSettings Settings { get; set; }

        public bool SettingsAreAvailable
        {
            get { return Settings != null; }
        }
    }
}
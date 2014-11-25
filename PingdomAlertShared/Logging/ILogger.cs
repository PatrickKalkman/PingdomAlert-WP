using System;

namespace PingdomAlertShared.Logging
{
    public interface ILogging
    {
        void Error(string message, Exception error);

        void Error(string message);
    }
}
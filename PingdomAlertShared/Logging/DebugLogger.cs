using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PingdomAlertShared.Logging
{
    public class DebugLogger : ILogging
    {
        public void Error(string message, Exception error)
        {
        }

        public void Error(string message)
        {
        }
    }
}

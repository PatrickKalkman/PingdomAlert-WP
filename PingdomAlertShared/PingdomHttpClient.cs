using System;
using System.IO;
using System.Net;
using System.Threading;

namespace PingdomAlertShared
{
    public class PingdomHttpClient
    {
        private HttpWebRequest httpRequest;
        private PingdomSettings pingdomSettings;

        public void GetResponse(string method, AsyncCallback methodToCall)
        {
            Uri requestUri = new Uri(pingdomSettings.PingdomBaseUriString + pingdomSettings.PingdomApiUriString + method);
            httpRequest = (HttpWebRequest)HttpWebRequest.Create(requestUri);
            httpRequest.Credentials = new NetworkCredential(pingdomSettings.Username, pingdomSettings.Password);
            httpRequest.Headers["App-Key"] = pingdomSettings.ApplicationKey;
            httpRequest.BeginGetResponse(req =>
            {
                var httpWebRequest = (HttpWebRequest)req.AsyncState;
                try
                {
                    using (var webResponse = httpWebRequest.EndGetResponse(req))
                    {
                        using (var reader = new StreamReader(webResponse.GetResponseStream()))
                        {
                            string response = reader.ReadToEnd();
                            methodToCall(new PingdomHttpClientResult { Response = response });
                        }
                    }
                }
                catch (Exception e)
                {
                    methodToCall(new PingdomHttpClientResult() { Error = e.Message });
                }

            }, httpRequest);
        }

        public void SetSettings(PingdomSettings settings)
        {
            pingdomSettings = settings;
        }
    }

    public class PingdomHttpClientResult : IAsyncResult
    {
        public string Response { get; set; }
        public string Error { get; set; }

        public bool IsCompleted { get; private set; }
        public WaitHandle AsyncWaitHandle { get; private set; }
        public object AsyncState { get; private set; }
        public bool CompletedSynchronously { get; private set; }
    }

}
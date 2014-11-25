using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using PingdomAlertShared.Events;
using PingdomAlertShared.Logging;
using PingdomAlertShared.PingdomModel;

namespace PingdomAlertShared
{
    public class PingdomManager
    {
        private readonly PingdomHttpClient pingdomHttpClient;
        private readonly PingdomSettingsManager pingdomSettingsManager;
        private readonly ILogging logger;

        public PingdomManager(PingdomHttpClient pingdomHttpClient, PingdomSettingsManager pingdomSettingsManager, ILogging logger)
        {
            this.pingdomHttpClient = pingdomHttpClient;
            this.pingdomSettingsManager = pingdomSettingsManager;
            this.logger = logger;
        }

        public void GetAllChecks()
        {
            try
            {
                const string GetAllChecksMethod = "checks";
                pingdomHttpClient.GetResponse(GetAllChecksMethod,
                    r =>
                    {
                        try
                        {
                            var result = (PingdomHttpClientResult)r;
                            Checks checks = JsonConvert.DeserializeObject<Checks>(result.Response);
                            Messenger.Default.Send(new ChecksReceivedEvent { Checks = checks });
                        }
                        catch (Exception error)
                        {
                            Messenger.Default.Send(new ChecksReceivedEvent { Checks = null, HasError = true });
                            logger.Error("GetAllChecks failed", error);
                        }
                    });
            }
            catch (Exception error)
            {
                logger.Error("GetAllChecks failed", error);
            }
        }

        public void GetCheck(int checkId)
        {
            try
            {
                const string GetCheckDetailMethod = "checks/{0}";
                pingdomHttpClient.GetResponse(string.Format(GetCheckDetailMethod, checkId),
                    r =>
                    {
                        var result = (PingdomHttpClientResult)r;
                        DetailCheckRoot detailCheck = JsonConvert.DeserializeObject<DetailCheckRoot>(result.Response);
                        Messenger.Default.Send(new CheckDetailReceivedEvent { DetailCheck = detailCheck.check });
                    });
            }
            catch (Exception error)
            {
                logger.Error(string.Format("GetCheck failed {0}", checkId), error);
            }
        }

        public void GetSharedReport()
        {
            try
            {
                const string ReportsMethod = "reports.shared";
                pingdomHttpClient.GetResponse(ReportsMethod,
                    r =>
                    {
                        var result = (PingdomHttpClientResult)r;
                        BannersRoot banners = JsonConvert.DeserializeObject<BannersRoot>(result.Response);
                        Messenger.Default.Send(new BannersReceivedEvent { Banners = banners });
                    });
            }
            catch (Exception error)
            {
                logger.Error("GetSharedReports", error);
            }
        }

        public void GetPerformanceReport(int checkId)
        {
            try
            {
                int from = (int)DateTimeConverter.DateTimeToUnixTimeStamp(DateTime.Now.AddDays(-30));
                const string ReportsMethod = "summary.performance/{0}?from={1}&resolution=day";
                pingdomHttpClient.GetResponse(string.Format(ReportsMethod, checkId, from),
                    r =>
                    {
                        var result = (PingdomHttpClientResult)r;
                        DayPerformance performance = JsonConvert.DeserializeObject<DayPerformance>(result.Response);
                        Messenger.Default.Send(new PerformanceReceivedEvent { Performance = performance });
                    });
            }
            catch (Exception error)
            {
                logger.Error(string.Format("GetPerformanceReports failed {0}", checkId), error);
            }
        }

        public void GetOutagesReport(int checkId)
        {
            try
            {
                int from = (int)DateTimeConverter.DateTimeToUnixTimeStamp(DateTime.Now.AddDays(-30));
                const string ReportsMethod = "summary.outage/{0}?from={1}";
                pingdomHttpClient.GetResponse(string.Format(ReportsMethod, checkId, from),
                    r =>
                    {
                        var result = (PingdomHttpClientResult)r;
                        StateRoot states = JsonConvert.DeserializeObject<StateRoot>(result.Response);
                        EnrichDataWithDays(states);
                        Messenger.Default.Send(new StatesReceivedEvent { States = states });
                    });
            }
            catch (Exception error)
            {
                logger.Error(string.Format("GetOutagesReport failed {0}", checkId), error);
            }
        }

        public void GetAverageReport(int checkId)
        {
            try
            {
                int from = (int)DateTimeConverter.DateTimeToUnixTimeStamp(DateTime.Now.AddDays(-30));
                const string ReportsMethod = "summary.average/{0}?includeuptime=true&from={1}";
                pingdomHttpClient.GetResponse(string.Format(ReportsMethod, checkId, from),
                    r =>
                    {
                        var result = (PingdomHttpClientResult)r;
                        ResponseRoot response = JsonConvert.DeserializeObject<ResponseRoot>(result.Response);
                        Messenger.Default.Send(new ResponseReceivedEvent { Response = response });
                    });
            }
            catch (Exception error)
            {
                logger.Error(string.Format("GetAverageReport failed {0}", checkId), error);
            }
        }

        //public bool AreAllSystemsUp()
        //{
        //    try
        //    {
        //        Checks checks = GetAllChecksDirect();
        //        if (checks.checks.Any(check => check.status == "down"))
        //        {
        //            return false;
        //        }
        //        return true;
        //    }
        //    catch (Exception error)
        //    {
        //        logger.Error("AreAllSystemsUp failed", error);
        //        return false;
        //    }
        //}

        private void EnrichDataWithDays(StateRoot states)
        {
            var statesToAdd = new List<State>();

            for (int stateCounter = 0; stateCounter < states.summary.states.Count - 1; stateCounter += 2)
            {
                DateTime firstDate = states.summary.states[stateCounter].TimeFromDate;
                DateTime secondDate = states.summary.states[stateCounter + 1].TimeFromDate;
                string firstState = states.summary.states[stateCounter].status;

                for (DateTime dateCounter = firstDate.AddDays(1); dateCounter < secondDate; dateCounter = dateCounter.AddDays(1))
                {
                    statesToAdd.Add(new State { status = firstState, timefrom = (int)DateTimeConverter.DateTimeToUnixTimeStamp(dateCounter) });
                }
            }

            states.summary.states.AddRange(statesToAdd);
        }

        public void CheckValidLogin()
        {
            try
            {
                const string GetAllChecksMethod = "checks";
                pingdomHttpClient.GetResponse(GetAllChecksMethod,
                    r =>
                    {
                        var result = (PingdomHttpClientResult)r;
                        try
                        {
                            JsonConvert.DeserializeObject<Checks>(result.Response);
                            Messenger.Default.Send(new CheckValidLoginEvent { IsValid = true });
                        }
                        catch (Exception error)
                        {
                            logger.Error("IsValidLogin failed", error);
                            Messenger.Default.Send(new CheckValidLoginEvent { IsValid = false });
                        }
                    });
            }
            catch (Exception error)
            {
                logger.Error("IsValidLogin failed", error);
                Messenger.Default.Send(new CheckValidLoginEvent { IsValid = false });
            }
        }

        public void SetSettings(PingdomSettings settings)
        {
            pingdomHttpClient.SetSettings(settings);
        }

        public PingdomSettings LoadSettings()
        {
            return pingdomSettingsManager.GetSettings();
        }
    }
}
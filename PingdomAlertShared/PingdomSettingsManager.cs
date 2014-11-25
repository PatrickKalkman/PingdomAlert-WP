using System;
using System.Security.Cryptography;
using System.Text;
using GalaSoft.MvvmLight.Messaging;
using PingdomAlertShared.Events;
using PingdomAlertShared.Logging;

namespace PingdomAlertShared
{
    public class PingdomSettingsManager
    {
        private readonly ILogging logger;

        public PingdomSettingsManager(ILogging logger)
        {
            this.logger = logger;
        }

        public PingdomSettings GetSettings()
        {
            var objectStorageHelper = new ObjectStorageHelper<PingdomSettings>();
            PingdomSettings pingdomSettings = objectStorageHelper.Load();
            if (pingdomSettings != null)
            {
                PingdomSettings copyOfSettings = CreateSettingsCopy(pingdomSettings);
                copyOfSettings.Username = Decrypt(copyOfSettings.Username);
                copyOfSettings.Password = Decrypt(copyOfSettings.Password);
                return copyOfSettings;
            }
            return null;
        }

        public bool SaveSettings(string username, string password)
        {
            var objectStorageHelper = new ObjectStorageHelper<PingdomSettings>();
            PingdomSettings settings = CreateSettings(username, password);
            objectStorageHelper.Save(settings);
            PingdomSettings settingsToSend = CreateSettingsCopy(settings);
            settingsToSend.Username = Decrypt(settingsToSend.Username);
            settingsToSend.Password = Decrypt(settingsToSend.Password);
            Messenger.Default.Send(new SettingsUpdateEvent { Settings = settingsToSend });
            return true;
        }

        private PingdomSettings CreateSettingsCopy(PingdomSettings settings)
        {
            var settingsToSend = new PingdomSettings();
            settingsToSend.ApplicationKey = settings.ApplicationKey;
            settingsToSend.Username = settings.Username;
            settingsToSend.Password = settings.Password;
            settingsToSend.PingdomApiUriString = settings.PingdomApiUriString;
            settingsToSend.PingdomBaseUriString = settings.PingdomBaseUriString;
            return settingsToSend;
        }

        public PingdomSettings CreateSettings(string username, string password)
        {
            var pingdomSettings = new PingdomSettings();
            pingdomSettings.ApplicationKey = "Get Your Own";
            pingdomSettings.PingdomBaseUriString = "https://api.pingdom.com";
            pingdomSettings.PingdomApiUriString = "/api/2.0/";
            pingdomSettings.Username = Encrypt(username);
            pingdomSettings.Password = Encrypt(password);
            return pingdomSettings;
        }

        public string Encrypt(string value)
        {
            try
            {
                byte[] pinByte = Encoding.UTF8.GetBytes(value);
                byte[] protectedBytes = ProtectedData.Protect(pinByte, null);
                return Convert.ToBase64String(protectedBytes);
            }
            catch (Exception ex)
            {
                logger.Error("Encrypt method failed", ex);
                return string.Empty;
            }
        }

        public string Decrypt(string value)
        {
            try
            {
                byte[] protectedPinByte = System.Convert.FromBase64String(value);
                byte[] pinByte = ProtectedData.Unprotect(protectedPinByte, null);
                return Encoding.UTF8.GetString(pinByte, 0, pinByte.Length);
            }
            catch (Exception ex)
            {
                logger.Error("Decrypt method failed", ex);
                return string.Empty;
            }
        }
    }
}
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using PingdomAlertShared;
using PingdomAlertShared.Logging;
using PingdomAlertShared.Navigation;

namespace PingdomAlert.WP8.ViewModel
{
    public class SettingsViewModel : ViewModelBase, INavigable
    {
        private readonly ILogging logger;
        private readonly PingdomSettingsManager pingdomSettingsManager;
        private readonly BackgroundImageBrush backgroundImageBrush;
        private const string TestResultString = "The given credentials are: {0}";
        private PingdomSettings loadedPingdomSettings;

        public SettingsViewModel(
            ILogging logger, 
            PingdomSettingsManager pingdomSettingsManager, 
            BackgroundImageBrush backgroundImageBrush)
        {
            this.pingdomSettingsManager = pingdomSettingsManager;
            this.backgroundImageBrush = backgroundImageBrush;
            TestCommand = new RelayCommand(OnTestCommand);
            SaveCommand = new RelayCommand(OnSaveCommand);
            LoadCommand = new RelayCommand(OnLoadCommand);
            NavigateCommand = new RelayCommand(OnNavigateCommand);
            this.logger = logger;
            Messenger.Default.Register<CheckValidLoginEvent>(this, OnCheckValidLoginEventReceived);
        }

        private void OnNavigateCommand()
        {
            var task = new Microsoft.Phone.Tasks.WebBrowserTask
            {
                Uri = new Uri("https://www.pingdom.com/signup/#freemodal")
            };

            task.Show();
        }

        private void OnCheckValidLoginEventReceived(CheckValidLoginEvent checkValidLoginEvent)
        {
            string toAdd = checkValidLoginEvent.IsValid ? "valid" : "invalid";
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    TestResults = string.Format(TestResultString, toAdd);
                    ShowProgress = false;
                });
        }

        private void OnLoadCommand()
        {
            loadedPingdomSettings = pingdomSettingsManager.GetSettings();
            if (loadedPingdomSettings != null)
            {
                UserName = loadedPingdomSettings.Username;
                Password = loadedPingdomSettings.Password;
            }
        }

        private void OnSaveCommand()
        {
            pingdomSettingsManager.SaveSettings(UserName, Password);
            NavigationService.Back();
        }

        private void OnTestCommand()
        {
            ShowProgress = true;
            var httpClient = new PingdomHttpClient();
            var manager = new PingdomManager(httpClient, pingdomSettingsManager, logger);
            var settings = pingdomSettingsManager.CreateSettings(UserName, Password);
            settings.Username = UserName;
            settings.Password = Password;
            httpClient.SetSettings(settings);
            manager.CheckValidLogin();
        }

        private bool showProgress;

        public bool ShowProgress
        {
            get { return showProgress; }
            set
            {
                showProgress = value;
                RaisePropertyChanged(() => ShowProgress);
            }
        }

        public ImageBrush BackgroundImageBrush
        {
            get { return backgroundImageBrush.GetBackground(); }
        }

        public ICommand TestCommand
        {
            get; private set;
        }

        public ICommand SaveCommand
        {
            get; private set;
        }

        public ICommand LoadCommand
        {
            get;
            private set;
        }

        private string password;

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                this.RaisePropertyChanged(() => this.Password);
            }
        }

        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; this.RaisePropertyChanged(() => this.UserName); }
        }

        private string testResults;

        public string TestResults
        {
            get { return testResults; }
            set { testResults = value; this.RaisePropertyChanged(() => this.TestResults); }
        }

        public INavigationService NavigationService { get; set; }

        public ICommand NavigateCommand
        {
            get; private set;
        }
    }
}
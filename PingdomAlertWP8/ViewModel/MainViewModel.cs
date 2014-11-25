using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using PingdomAlertShared;
using PingdomAlertShared.Events;
using PingdomAlertShared.Navigation;
using PingdomAlertShared.PingdomModel;

namespace PingdomAlert.WP8.ViewModel
{
    public class MainViewModel : ViewModelBase, INavigable
    {
        private readonly PingdomManager pingdomManager;
        private readonly BackgroundImageBrush backgroundImageBrush;
        private bool settingsLoaded;

        public MainViewModel(PingdomManager pingdomManager, BackgroundImageBrush backgroundImageBrush)
        {
            this.pingdomManager = pingdomManager;
            this.backgroundImageBrush = backgroundImageBrush;
            Messenger.Default.Register<ChecksReceivedEvent>(this, OnChecksRecieved);
            Messenger.Default.Register<SettingsUpdateEvent>(this, OnSettingsUpdateEventReceived);

            LoadCommand = new RelayCommand(Load);
            SettingsCommand = new RelayCommand(OnSettingsCommand);
            AboutCommand = new RelayCommand(OnAboutCommand);
            PrivacyCommand = new RelayCommand(OnPrivacyCommand);
            ShowProgress = true;
            isNavigationAllowed = false;
        }

        private bool isNavigationAllowed;

        private void OnPrivacyCommand()
        {
            this.NavigationService.Navigate("/PrivacyView.xaml");
        }

        private void OnSettingsCommand()
        {
            this.NavigationService.Navigate("/SettingsView.xaml");
        }

        private void OnSettingsUpdateEventReceived(SettingsUpdateEvent settingsUpdateEvent)
        {
            ShowProgress = true;
            ShowAccount = false;
            settingsLoaded = true;
            isNavigationAllowed = false;
            pingdomManager.SetSettings(settingsUpdateEvent.Settings);
            pingdomManager.GetAllChecks();
        }

        private void OnChecksRecieved(ChecksReceivedEvent checksReceivedEvent)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    ShowProgress = false;
                    if (!checksReceivedEvent.HasError)
                    {
                        PingdomChecks =
                            new ObservableCollection<Check>(
                                checksReceivedEvent.Checks.checks);
                        isNavigationAllowed = true;
                    }
                    else
                    {
                        PingdomChecks = null;
                    }
                });
        }

        private void Load()
        {
            isNavigationAllowed = false;
            if (!settingsLoaded)
            {
                PingdomSettings settings = pingdomManager.LoadSettings();
                if (settings != null)
                {
                    ShowAccount = false;
                    pingdomManager.SetSettings(settings);
                    pingdomManager.GetAllChecks();
                }
                else
                {
                    ShowAccount = true;
                }
            }
        }

        public INavigationService NavigationService { get; set; }

        public RelayCommand LoadCommand
        {
            get;
            private set;
        }

        private ObservableCollection<Check> pingdomChecks;

        public ObservableCollection<Check> PingdomChecks
        {
            get
            {

                return pingdomChecks;
            }
            set
            {
                pingdomChecks = value;
                RaisePropertyChanged(() => PingdomChecks);
            }
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

        private Check selectedCheck;

        public Check SelectedCheck
        {
            get
            {
                return selectedCheck;
            }
            set 
            {
                if (isNavigationAllowed)
                {
                    selectedCheck = value;
                    RaisePropertyChanged(() => SelectedCheck);
                    Messenger.Default.Send(new CheckSelectedEvent {SelectedCheck = SelectedCheck});
                    NavigationService.Navigate("/CheckDetail.xaml");
                }
            }
        }

        private void OnAboutCommand()
        {
            NavigationService.Navigate("/YourLastAboutDialog;component/AboutPage.xaml");
        }

        public ImageBrush BackgroundImageBrush
        {
            get { return backgroundImageBrush.GetBackground(); }
        }

        public RelayCommand SettingsCommand
        {
            get;
            private set;
        }

        public RelayCommand AboutCommand
        {
            get;
            private set;
        }

        public RelayCommand PrivacyCommand
        {
            get; 
            private set;
        }

        public RelayCommand OpenAccountCommand
        {
            get; 
            private set;
        }

        private bool showAccount;

        public bool ShowAccount
        {
            get { return showAccount; }
            set
            {
                showAccount = value;
                RaisePropertyChanged(() => ShowAccount);
            }
        }
    }
}
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using PingdomAlertShared;
using PingdomAlertShared.Events;
using PingdomAlertShared.Navigation;
using PingdomAlertShared.PingdomModel;

namespace PingdomAlert.WP8.ViewModel
{
    public class CheckDetailViewModel : ViewModelBase, INavigable
    {
        private readonly PingdomManager pingdomManager;
        private readonly BackgroundImageBrush backgroundImageBrush;

        public CheckDetailViewModel(PingdomManager pingdomManager, BackgroundImageBrush backgroundImageBrush)
        {
            this.pingdomManager = pingdomManager;
            this.backgroundImageBrush = backgroundImageBrush;
            Messenger.Default.Register<CheckSelectedEvent>(this, OnSelectedCheckEventReceived);
            Messenger.Default.Register<ResponseReceivedEvent>(this, OnResponseReceived);
            Messenger.Default.Register<PerformanceReceivedEvent>(this, OnPerformanceReceived);
            Messenger.Default.Register<StatesReceivedEvent>(this, OnStatesReceived);

            if (IsInDesignMode)
            {
                SelectedCheck = new Check() { hostname = "www.semanticarchitecture", name = "Semanticarchitecture" };
            }
        }

        private void OnStatesReceived(StatesReceivedEvent statesReceivedEvent)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                                          {
                                                              this.StateData =
                                                                  new ObservableCollection<State>(
                                                                      statesReceivedEvent.States.summary.states);
                                                              OutagesProgressVisible = false;
                                                          });
        }

        private void OnPerformanceReceived(PerformanceReceivedEvent performanceReceivedEvent)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                                          {
                                                              this.PerformanceData =
                                                                  new ObservableCollection<Day>(
                                                                      performanceReceivedEvent.Performance.summary.days);
                                                              PerformanceProgressVisible = false;
                                                          });
        }

        private void OnResponseReceived(ResponseReceivedEvent responseReceivedEvent)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    ResponseData = responseReceivedEvent.Response.summary;
                    ShowProgress = false;
                });
        }

        private void OnSelectedCheckEventReceived(CheckSelectedEvent checkSelectedEvent)
        {
            ShowProgress = true;
            OutagesProgressVisible = true;
            PerformanceProgressVisible = true;
            SelectedCheck = null;
            ResponseData = null;
            if (checkSelectedEvent.SelectedCheck != null)
            {
                SelectedCheck = checkSelectedEvent.SelectedCheck;
                pingdomManager.GetAverageReport(selectedCheck.id);
                pingdomManager.GetPerformanceReport(selectedCheck.id);
                pingdomManager.GetOutagesReport(selectedCheck.id);
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

        private ObservableCollection<State> stateData;

        public ObservableCollection<State> StateData
        {
            get { return stateData; }
            set
            {
                stateData = value;
                RaisePropertyChanged(() => StateData);
            }
        }

        private ObservableCollection<Day> performanceData;

        public ObservableCollection<Day> PerformanceData
        {
            get { return performanceData; }
            set
            {
                performanceData = value;
                RaisePropertyChanged(() => PerformanceData);
            }
        }

        private ResponseSummary responseSummary;

        public ResponseSummary ResponseData
        {
            get { return responseSummary; }
            set
            {
                responseSummary = value;
                RaisePropertyChanged(() => ResponseData);
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
                selectedCheck = value;
                this.RaisePropertyChanged(() => SelectedCheck);
            }
        }

        public INavigationService NavigationService { get; set; }

        private string performanceBannerImageUrl;

        public string PerformanceBannerImageUrl
        {
            get { return performanceBannerImageUrl; }
            set 
            { 
                performanceBannerImageUrl = value;
                RaisePropertyChanged(() => PerformanceBannerImageUrl);
            }
        }

        private bool performanceProgressVisible;

        public bool PerformanceProgressVisible
        {
            get { return performanceProgressVisible; }
            set
            {
                performanceProgressVisible = value;
                RaisePropertyChanged(() => PerformanceProgressVisible);
            }
        }

        private bool outagesProgressVisible;

        public bool OutagesProgressVisible
        {
            get { return outagesProgressVisible; }
            set
            {
                outagesProgressVisible = value;
                RaisePropertyChanged(() => OutagesProgressVisible);
            }
        }

        public ImageBrush BackgroundImageBrush
        {
            get { return backgroundImageBrush.GetBackground(); }
        }
    }
}

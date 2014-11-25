using System;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using PingdomAlert.WP8.ViewModel;
using PingdomAlertShared.PingdomModel;

namespace PingdomAlert.WP8
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void ApplicationBarMenuItemSettings_Click(object sender, EventArgs e)
        {
            var viewModel = this.DataContext as MainViewModel;
            if (viewModel != null)
            {
                viewModel.SettingsCommand.Execute(null);
            }
        }

        private void ApplicationBarMenuItemAbout_Click(object sender, EventArgs e)
        {
            var viewModel = this.DataContext as MainViewModel;
            if (viewModel != null)
            {
                viewModel.AboutCommand.Execute(null);
            }
        }

        private void ApplicationBarMenuItemPrivacy_Click(object sender, EventArgs e)
        {
            var viewModel = this.DataContext as MainViewModel;
            if (viewModel != null)
            {
                viewModel.PrivacyCommand.Execute(null);
            }
        }
    }
}
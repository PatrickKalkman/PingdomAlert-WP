using System;

namespace PingdomAlertShared.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly System.Windows.Navigation.NavigationService navigationService;

        public NavigationService(System.Windows.Navigation.NavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public void Navigate(string url)
        {
            this.navigationService.Navigate(new Uri(url, UriKind.RelativeOrAbsolute));
        }

        public void Back()
        {
            this.navigationService.GoBack();
        }
    }
}

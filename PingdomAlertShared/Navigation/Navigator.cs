using System.Windows;
using System.Windows.Controls;

namespace PingdomAlertShared.Navigation
{
    public static class Navigator
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.RegisterAttached("Source", typeof(INavigable), typeof(Navigator), new PropertyMetadata(OnSourceChanged));
        
        public static INavigable GetSource(DependencyObject obj)
        {
            return (INavigable)obj.GetValue(SourceProperty);
        }

        public static void SetSource(DependencyObject obj, INavigable value)
        {
            obj.SetValue(SourceProperty, value);
        }

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var page = (Page)d;
            page.Loaded += PageLoaded;
        }

        private static void PageLoaded(object sender, RoutedEventArgs e)
        {
            var page = (Page)sender;

            INavigable navSource = GetSource(page);
            if (navSource != null)
            {
                navSource.NavigationService = new NavigationService(page.NavigationService);
            }
        }
    }
}

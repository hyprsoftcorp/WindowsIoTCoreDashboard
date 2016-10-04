using Microsoft.HockeyApp;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WindowsIoTDashboard.App.ViewModels;

namespace WindowsIoTDashboard.App.Pages
{
    public abstract class BasePage : Page
    {
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var vm = DataContext as IViewModel;
            if (vm != null)
                await vm.InitializeAsync();
            HockeyClient.Current.TrackEvent(GetType().Name);
        }

        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            var vm = DataContext as IViewModel;
            if (vm != null)
                await vm.UnInitializeAsync();
        }
    }
}

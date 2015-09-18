using GalaSoft.MvvmLight.Messaging;
using WindowsIoTDashboard.App.Models;
using WindowsIoTDashboard.App.Services;

namespace WindowsIoTDashboard.App.Pages
{
    public sealed partial class NetworkingPage : BasePage
    {
        public NetworkingPage()
        {
            this.InitializeComponent();
        }

        private void WifiAdapters_SelectionChanged(object sender, Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
                Messenger.Default.Send(e.AddedItems[0] as WifiAdapterModel.Interface,
                    UserInterfaceService.Commands.WifiAdapterSelectionChanged);
        }
    }
}

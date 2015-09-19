using GalaSoft.MvvmLight.Messaging;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WindowsIoTDashboard.App.Helpers;
using WindowsIoTDashboard.App.Pages;
using WindowsIoTDashboard.App.Services;

namespace WindowsIoTDashboard.App
{
    public sealed partial class MainPage : BasePage
    {
        public MainPage()
        {
            this.InitializeComponent();
            Messenger.Default.Register<Exception>(this, UserInterfaceService.Commands.HideFeedback, _ => FeedbackFlyout.Hide());
            Messenger.Default.Register<string>(this, UserInterfaceService.Commands.ShowSettings, _ => SettingsFlyout.ShowAt(SettingsButton));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            NavMenu.SelectedIndex = 0;

        }

        private void NavMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = (ListBox)sender;
            if (listBox.SelectedIndex == -1) return;
            NavStrip.IsPaneOpen = false;
            ContentFrame.Navigate(((NavItem)listBox.SelectedItem).Page);
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            NavStrip.IsPaneOpen = !NavStrip.IsPaneOpen;
        }
    }
}

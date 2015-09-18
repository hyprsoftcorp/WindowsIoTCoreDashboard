using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.System.Profile;
using Windows.UI.Popups;

namespace WindowsIoTDashboard.App.Services
{
    public interface IUserInterfaceService
    {
        Task ShowFeedbackAsync(Exception ex);
        Task HideFeedbackAsync();
        Task ShowSettingsAsync();
        Task HideSettingsAsync();
        Task<IUICommand> ShowDialogAsync(string title, string message, IEnumerable<UICommand> commands, uint defaultCommandIndex);
    }

    public class UserInterfaceService : IUserInterfaceService
    {
        public enum Commands
        {
            ShowFeedback,
            HideFeedback,
            ShowSettings,
            HideSettings,
            WifiAdapterSelectionChanged
        }

        public Task ShowFeedbackAsync(Exception ex)
        {
            Messenger.Default.Send(ex, Commands.ShowFeedback);
            return Task.FromResult(0);
        }

        public Task HideFeedbackAsync()
        {
            Messenger.Default.Send<Exception>(null, Commands.HideFeedback);
            return Task.FromResult(0);
        }

        public Task ShowSettingsAsync()
        {
            Messenger.Default.Send(String.Empty, Commands.ShowSettings);
            return Task.FromResult(0);
        }

        public Task HideSettingsAsync()
        {
            Messenger.Default.Send(String.Empty, Commands.HideSettings);
            return Task.FromResult(0);
        }

        public async Task<IUICommand> ShowDialogAsync(string title, string message, IEnumerable<UICommand> commands, uint defaultCommandIndex)
        {
            var dialog = new MessageDialog(message, title);
            // TODO: This is a known issue on Windows 10 IoT Core.
            if (AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.IoT")
            {
                foreach (var command in commands)
                    dialog.Commands.Add(command);
            }
            dialog.DefaultCommandIndex = defaultCommandIndex;
            return await dialog.ShowAsync();
        }
    }
}

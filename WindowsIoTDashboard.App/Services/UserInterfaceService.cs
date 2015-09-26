using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace WindowsIoTDashboard.App.Services
{
    public interface IUserInterfaceService
    {
        Task ShowFeedbackAsync(Exception ex);
        Task HideFeedbackAsync();
        Task ShowSettingsAsync();
        Task HideSettingsAsync();
        Task ShowBusyIndicatorAsync();
        Task HideBusyIndicatorAsync();
        Task HideRunCommandAsync();
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
            WifiAdapterSelectionChanged,
            ShowBusyIndicator,
            HideBusyIndicator,
            HideRunCommand
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

        public Task ShowBusyIndicatorAsync()
        {
            Messenger.Default.Send(String.Empty, Commands.ShowBusyIndicator);
            return Task.FromResult(0);
        }

        public Task HideBusyIndicatorAsync()
        {
            Messenger.Default.Send(String.Empty, Commands.HideBusyIndicator);
            return Task.FromResult(0);
        }

        public Task HideRunCommandAsync()
        {
            Messenger.Default.Send(String.Empty, Commands.HideRunCommand);
            return Task.FromResult(0);
        }

        public async Task<IUICommand> ShowDialogAsync(string title, string message, IEnumerable<UICommand> commands, uint defaultCommandIndex)
        {
            // TODO: This is a known issue on Windows 10 IoT Core.
            if (!App.IsRunningOnWindowsIoTDevice)
            {
                var dialog = new MessageDialog(message, title);
                foreach (var command in commands)
                    dialog.Commands.Add(command);
                dialog.DefaultCommandIndex = defaultCommandIndex;
                return await dialog.ShowAsync();
            }
            return null;
        }
    }
}

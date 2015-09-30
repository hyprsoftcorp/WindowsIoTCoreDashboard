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
        Task<IUICommand> ShowDialogAsync(string title, string message, IList<UICommand> commands, uint defaultCommandIndex);
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

        public async Task<IUICommand> ShowDialogAsync(string title, string message, IList<UICommand> commands, uint defaultCommandIndex)
        {
            if (String.IsNullOrEmpty(title))
                throw new ArgumentNullException("Title cannot be null or empty.");

            if (String.IsNullOrEmpty(message))
                throw new ArgumentNullException("Message cannot be null or empty.");

            if (commands == null)
                throw new ArgumentNullException("Commands cannot be null.");

            if (commands.Count <= 0)
                throw new ArgumentOutOfRangeException("Commands must contain at least one UICommand.");

            // Windows.UI.Popups.MessageDialog isn't currently supported on Windows 10 IoT Core so let's skip showing our confirmaion message
            // and just automatically invoke our first UICommand.
            if (!App.IsRunningOnWindowsIoTDevice)
            {
                var dialog = new MessageDialog(message, title);
                foreach (var command in commands)
                    dialog.Commands.Add(command);
                dialog.DefaultCommandIndex = defaultCommandIndex;
                return await dialog.ShowAsync();
            }
            else
            {
                commands[0].Invoked(commands[0]);
                return commands[0];
            }
        }
    }
}

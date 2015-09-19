using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using WindowsIoTDashboard.App.Services;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using System.Collections.Generic;
using System.Net.Http;

namespace WindowsIoTDashboard.App.ViewModels
{
    public class MainViewModel : ViewModelBase, IViewModel
    {
        #region Fields

        private IRestService _restService;
        private ISettingsService _settingsService;
        private IUserInterfaceService _userInterfaceService;

        #endregion

        #region Constructors

        public MainViewModel(IRestService restService, ISettingsService settingsService, IUserInterfaceService userInterfaceService)
        {
            _restService = restService;
            _settingsService = settingsService;
            _userInterfaceService = userInterfaceService;

            Messenger.Default.Register<Exception>(this, UserInterfaceService.Commands.ShowFeedback, e => ShowFeedback(e));
        }

        #endregion

        #region Properties

        public ISettingsService Settings
        {
            get { return _settingsService; }
            private set { Set(ref _settingsService, value); }
        }

        private string _feedbackText;
        public string FeedbackText
        {
            get { return _feedbackText; }
            set { Set(ref _feedbackText, value); }
        }

        private Visibility _feedbackButtonVisibility = Visibility.Collapsed;
        public Visibility FeedbackButtonVisibility
        {
            get { return _feedbackButtonVisibility; }
            set { Set(ref _feedbackButtonVisibility, value); }
        }

        private RelayCommand _rebootCommand;
        public RelayCommand RebootCommand
        {
            get
            {
                return _rebootCommand ?? (_rebootCommand = new RelayCommand(async () =>
                {
                    var commands = new List<UICommand>
                    {
                        new UICommand("Yes, let's do it", async (cmd) =>
                        {
                            try
                            {
                                _restService.TelemetryClient.TrackEvent("RebootCommand");
                                await _restService.PostAsync(new Uri("api/control/reboot", UriKind.Relative), null);
                            }
                            catch (Exception ex)
                            {
                                ShowFeedback(ex);
                            }
                        }),
                        new UICommand("No, forget it")
                    };
                    await _userInterfaceService.ShowDialogAsync("Reboot Confirmation", "Are you sure you want to reboot your Windows IoT device?", commands, 1);
                }));
            }
        }

        private RelayCommand _shutdownCommand;
        public RelayCommand ShutdownCommand
        {
            get
            {
                return _shutdownCommand ?? (_shutdownCommand = new RelayCommand(async () =>
                {
                    var commands = new List<UICommand>
                    {
                        new UICommand("Yes, put 'er down", async (cmd) =>
                        {
                            try
                            {
                                _restService.TelemetryClient.TrackEvent("ShutdownCommand");
                                await _restService.PostAsync(new Uri("api/control/shutdown", UriKind.Relative), null);
                            }
                            catch (Exception ex)
                            {
                                ShowFeedback(ex);
                            }
                        }),
                        new UICommand("No, not yet")
                    };
                    await _userInterfaceService.ShowDialogAsync("Shutdown Confirmation", "Are you sure you want to shutdown your Windows IoT device?", commands, 1);
                }));
            }
        }

        private RelayCommand _dismissFeedbackCommand;
        public RelayCommand DismissFeedbackCommand
        {
            get
            {
                return _dismissFeedbackCommand ?? (_dismissFeedbackCommand = new RelayCommand(async () =>
                {
                    await _userInterfaceService.HideFeedbackAsync();
                    FeedbackButtonVisibility = Visibility.Collapsed;
                }));
            }
        }

        private RelayCommand _showSettingsCommand;
        public RelayCommand ShowSettingsCommand
        {
            get
            {
                return _showSettingsCommand ?? (_showSettingsCommand = new RelayCommand(async () => await _userInterfaceService.ShowSettingsAsync()));
            }
        }

        #endregion

        #region Methods

        public async Task InitializeAsync()
        {
            if (_settingsService.IsFirstRun)
            {
                var commands = new List<UICommand>
                {
                    new UICommand("I'm Good"),
                    new UICommand("Open Settings", async (cmd) => await _userInterfaceService.ShowSettingsAsync())
                };
                await _userInterfaceService.ShowDialogAsync("Well Hello!", "It looks like this is the first time you've run this app so let's make sure things are setup correctly. We have preconfigured this app's connection to your Windows IoT device using the default device name (minwinpc), username (administrator), and password (p@ssw0rd). If you have changed any of those settings on your device you will need to change this app's settings too.", commands, 1);
                _settingsService.IsFirstRun = false;
            }   // first run?
        }

        public Task UnInitializeAsync()
        {
            return Task.FromResult(0);
        }

        public void ShowFeedback(Exception ex)
        {
            if (ex is HttpRequestException)
                FeedbackText = String.Format("We are unable to connect to the Windows IoT core device named '{0}'.  Please ensure your device connection settings are correct and that you have network connectivity.", _settingsService.DeviceName);
            else
            {
                _restService.TelemetryClient.TrackException(ex);
                FeedbackText = String.Format("Uh oh it looks like something bad has happened.  This is all we know: {0} {1}", ex.Message, ex.InnerException == null ? String.Empty : ex.InnerException.Message).Replace("\n", " ").Replace("\r", " ");
            }
            FeedbackButtonVisibility = String.IsNullOrEmpty(FeedbackText) ? Visibility.Collapsed : Visibility.Visible;
        }

        #endregion
    }
}
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using WindowsIoTDashboard.App.Models;
using WindowsIoTDashboard.App.Services;

namespace WindowsIoTDashboard.App.ViewModels
{
    public class HomeViewModel : ViewModelBase, IViewModel
    {
        #region Fields

        private IRestService _restService;
        private IUserInterfaceService _userInterfaceService;
        private DispatcherTimer _timer;

        #endregion

        #region Constructors

        public HomeViewModel(IRestService restService, IUserInterfaceService userInterfaceService)
        {
            _restService = restService;
            _userInterfaceService = userInterfaceService;
        }

        #endregion

        #region Properties

        private DeviceInfoModel _deviceInfoModel;
        public DeviceInfoModel DeviceInfoModel
        {
            get { return _deviceInfoModel; }
            private set { Set(ref _deviceInfoModel, value); }
        }

        private SystemPerfModel _systemPerfModel;
        public SystemPerfModel SystemPerfModel
        {
            get { return _systemPerfModel; }
            private set { Set(ref _systemPerfModel, value); }
        }

        private RelayCommand _refreshCommand;
        public RelayCommand RefreshCommand
        {
            get
            {
                return _refreshCommand ?? (_refreshCommand = new RelayCommand(async () => await InitializeAsync()));
            }
        }

        private double _totalMemory;
        public double TotalMemory
        {
            get { return _totalMemory; }
            set { Set(ref _totalMemory, value); }
        }

        private double _inUseMemory;
        public double InUseMemory
        {
            get { return _inUseMemory; }
            set { Set(ref _inUseMemory, value); }
        }

        private double _availableMemory;
        public double AvailableMemory
        {
            get { return _availableMemory; }
            set { Set(ref _availableMemory, value); }
        }

        private double _networkInCurrent;
        public double NetworkInCurrent
        {
            get { return _networkInCurrent; }
            set { Set(ref _networkInCurrent, value); }
        }

        private double _networkInMaximum;
        public double NetworkInMaximum
        {
            get { return _networkInMaximum; }
            set { Set(ref _networkInMaximum, value); }
        }

        private double _networkOutCurrent;
        public double NetworkOutCurrent
        {
            get { return _networkOutCurrent; }
            set { Set(ref _networkOutCurrent, value); }
        }

        private double _networkOutMaximum;
        public double NetworkOutMaximum
        {
            get { return _networkOutMaximum; }
            set { Set(ref _networkOutMaximum, value); }
        }

        public Visibility ResourcesVisibility
        {
            get { return App.IsRunningOnWindowsIoTDevice ? Visibility.Collapsed : Visibility.Visible; }
        }

        #endregion

        #region Methods

        public async Task InitializeAsync()
        {
            try
            {
                await _userInterfaceService.ShowBusyIndicatorAsync();
                DeviceInfoModel = await _restService.GetAsync<DeviceInfoModel>(new Uri("api/iot/device/information", UriKind.Relative));

                _timer = new DispatcherTimer();
                _timer.Tick += Timer_Tick;
                _timer.Start();
            }
            catch (Exception ex)
            {
                await _userInterfaceService.ShowFeedbackAsync(ex);
            }
        }

        public Task UnInitializeAsync()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Tick -= Timer_Tick;
            }
            DeviceInfoModel = null;
            SystemPerfModel = null;
            return Task.FromResult(0);
        }

        private async void Timer_Tick(object sender, object e)
        {
            try
            {
                _timer.Stop();
                _timer.Interval = TimeSpan.FromSeconds(1);

                SystemPerfModel = await _restService.GetAsync<SystemPerfModel>(new Uri("api/resourcemanager/systemperf", UriKind.Relative));
                TotalMemory = SystemPerfModel.TotalPages * SystemPerfModel.PageSize;
                AvailableMemory = SystemPerfModel.AvailablePages * SystemPerfModel.PageSize;
                InUseMemory = TotalMemory - AvailableMemory;

                var networkIn = SystemPerfModel.NetworkingData.NetworkInBytes;
                NetworkInMaximum = Math.Max(NetworkInMaximum, networkIn);
                NetworkInCurrent = networkIn;

                var networkOut = SystemPerfModel.NetworkingData.NetworkOutBytes;
                NetworkOutMaximum = Math.Max(NetworkOutMaximum, networkOut);
                NetworkOutCurrent = networkOut;

                _timer.Start();
                await _userInterfaceService.HideBusyIndicatorAsync();
            }
            catch (Exception ex)
            {
                await _userInterfaceService.ShowFeedbackAsync(ex);
            }
        }

        #endregion
    }
}

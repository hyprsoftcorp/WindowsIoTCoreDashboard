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

        private float _totalMemory;
        public float TotalMemory
        {
            get { return _totalMemory; }
            set { Set(ref _totalMemory, value); }
        }

        private float _inUseMemory;
        public float InUseMemory
        {
            get { return _inUseMemory; }
            set { Set(ref _inUseMemory, value); }
        }

        private float _availableMemory;
        public float AvailableMemory
        {
            get { return _availableMemory; }
            set { Set(ref _availableMemory, value); }
        }

        #endregion

        #region Methods

        public async Task InitializeAsync()
        {
            try
            {
                DeviceInfoModel = await _restService.GetAsync<DeviceInfoModel>(new Uri("api/iot/deviceinformation", UriKind.Relative));

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
                TotalMemory = (SystemPerfModel.TotalPages * SystemPerfModel.PageSize) / (float)1048576;
                AvailableMemory = (SystemPerfModel.AvailablePages * SystemPerfModel.PageSize) / (float)1048576;
                InUseMemory = TotalMemory - AvailableMemory;
                _timer.Start();
            }
            catch (Exception ex)
            {
                await _userInterfaceService.ShowFeedbackAsync(ex);
            }
        }

        #endregion
    }
}

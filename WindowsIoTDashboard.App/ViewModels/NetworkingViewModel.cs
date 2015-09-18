using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Threading.Tasks;
using WindowsIoTDashboard.App.Models;
using WindowsIoTDashboard.App.Services;

namespace WindowsIoTDashboard.App.ViewModels
{
    public class NetworkingViewModel : ViewModelBase, IViewModel
    {
        #region Fields

        private IRestService _restService;
        private IUserInterfaceService _userInterfaceService;

        #endregion

        #region Constructors

        public NetworkingViewModel(IRestService restService, IUserInterfaceService userInterfaceSerive)
        {
            _restService = restService;
            _userInterfaceService = userInterfaceSerive;
            Messenger.Default.Register<WifiAdapterModel.Interface>(this, UserInterfaceService.Commands.WifiAdapterSelectionChanged, async m =>
            {
                try
                {
                    WifiNetworksModel = await _restService.GetAsync<WifiNetworksModel>(new Uri(String.Format("api/wifi/networks?interface={0}",
                        m.GUID.ToString("D")), UriKind.Relative));
                }
                catch (Exception ex)
                {
                    await _userInterfaceService.ShowFeedbackAsync(ex);
                }
            });
        }

        #endregion

        #region Properties

        private IpConfigModel _ipConfigModel;
        public IpConfigModel IpConfigModel
        {
            get { return _ipConfigModel; }
            private set { Set(ref _ipConfigModel, value); }
        }

        private WifiAdapterModel _wifiAdapterModel;
        public WifiAdapterModel WifiAdapterModel
        {
            get { return _wifiAdapterModel; }
            private set { Set(ref _wifiAdapterModel, value); }
        }

        private WifiNetworksModel _wifiNetworksModel;
        public WifiNetworksModel WifiNetworksModel
        {
            get { return _wifiNetworksModel; }
            private set { Set(ref _wifiNetworksModel, value); }
        }

        private RelayCommand _refreshCommand;
        public RelayCommand RefreshCommand
        {
            get
            {
                return _refreshCommand ?? (_refreshCommand = new RelayCommand(async () => await InitializeAsync()));
            }
        }

        #endregion

        #region Methods

        public async Task InitializeAsync()
        {
            try
            {
                IpConfigModel = await _restService.GetAsync<IpConfigModel>(new Uri("api/networking/ipconfig", UriKind.Relative));
                WifiAdapterModel = await _restService.GetAsync<WifiAdapterModel>(new Uri("api/wifi/interfaces", UriKind.Relative));
                if (WifiAdapterModel.Interfaces.Length > 0)
                    WifiNetworksModel = await _restService.GetAsync<WifiNetworksModel>(new Uri(String.Format("api/wifi/networks?interface={0}",
                        WifiAdapterModel.Interfaces[0].GUID.ToString("D")), UriKind.Relative));
            }
            catch (Exception ex)
            {
                await _userInterfaceService.ShowFeedbackAsync(ex);
            }
        }

        public Task UnInitializeAsync()
        {
            IpConfigModel = null;
            WifiAdapterModel = null;
            WifiNetworksModel = null;
            return Task.FromResult(0);
        }

        #endregion
    }
}

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Linq;
using System.Threading.Tasks;
using WindowsIoTDashboard.App.Models;
using WindowsIoTDashboard.App.Services;

namespace WindowsIoTDashboard.App.ViewModels
{
    public class DevicesViewModel : ViewModelBase, IViewModel
    {
        #region Fields

        private IRestService _restService;
        private IUserInterfaceService _userInterfaceService;

        #endregion

        #region Constructors

        public DevicesViewModel(IRestService restService, IUserInterfaceService userInterfaceService)
        {
            _restService = restService;
            _userInterfaceService = userInterfaceService;
        }

        #endregion

        #region Properties

        private DevicesModel _model;
        public DevicesModel Model
        {
            get { return _model; }
            private set { Set(ref _model, value); }
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
                await _userInterfaceService.ShowBusyIndicatorAsync();
                Model = await _restService.GetAsync<DevicesModel>(new Uri("api/devicemanager/devices", UriKind.Relative));
                var sortedModel = new DevicesModel();
                BuildHierarchy(Model, sortedModel, Model.DeviceList.Where(d => d.ParentID == null).Select(d => d.ID).FirstOrDefault(), 0);
                Model = sortedModel;
                await _userInterfaceService.HideBusyIndicatorAsync();
            }
            catch (Exception ex)
            {
                await _userInterfaceService.ShowFeedbackAsync(ex);
            }
        }

        public Task UnInitializeAsync()
        {
            Model = null;
            return Task.FromResult(0);
        }

        private void BuildHierarchy(DevicesModel model, DevicesModel sortedModel, string parentId, int level)
        {
            foreach (var device in model.DeviceList.Where(d => d.ParentID == parentId))
            {
                device.Level = level;
                if (string.IsNullOrEmpty(device.Description))
                    device.Description = device.ID;
                if (String.IsNullOrEmpty(device.Class))
                    device.Class = "Unknown";
                sortedModel.DeviceList.Add(device);
                var children = Model.DeviceList.Where(d => d.ParentID == device.ID);
                if (children.Count() > 0)
                {
                    device.HasChildren = true;
                    BuildHierarchy(Model, sortedModel, device.ID, level + 1);
                }
            }   // for each device
        }

        #endregion
    }
}

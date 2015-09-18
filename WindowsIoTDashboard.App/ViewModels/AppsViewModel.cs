using GalaSoft.MvvmLight;
using System;
using System.Linq;
using System.Threading.Tasks;
using WindowsIoTDashboard.App.Models;
using WindowsIoTDashboard.App.Services;
using GalaSoft.MvvmLight.Command;

namespace WindowsIoTDashboard.App.ViewModels
{
    public class AppsViewModel : ViewModelBase, IViewModel
    {
        #region Fields

        private IRestService _restService;
        private IUserInterfaceService _userInterfaceService;

        #endregion

        #region Constructors

        public AppsViewModel(IRestService restService, IUserInterfaceService userInterfaceService)
        {
            _restService = restService;
            _userInterfaceService = userInterfaceService;
        }

        #endregion

        #region Properties

        private AppsModel _installedModel;
        public AppsModel InstalledModel
        {
            get { return _installedModel; }
            private set { Set(ref _installedModel, value); }
        }

        private AppsModel _runningModel;
        public AppsModel RunningModel
        {
            get { return _runningModel; }
            private set { Set(ref _runningModel, value); }
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
                InstalledModel = await _restService.GetAsync<AppsModel>(new Uri("api/appx/installed", UriKind.Relative));
                var processesModel = await _restService.GetAsync<ProcessesModel>(new Uri("api/resourcemanager/processes", UriKind.Relative));
                RunningModel = new AppsModel();
                foreach (var process in processesModel.Processes)
                {
                    var app = InstalledModel.InstalledPackages.FirstOrDefault(p => p.PackageFullName == process.PackageFullName);
                    if (app != null)
                        RunningModel.InstalledPackages.Add(app);
                }   // for each running process
            }
            catch (Exception ex)
            {
                await _userInterfaceService.ShowFeedbackAsync(ex);
            }
        }

        public Task UnInitializeAsync()
        {
            InstalledModel = null;
            RunningModel = null;
            return Task.FromResult(0);
        }

        #endregion
    }
}

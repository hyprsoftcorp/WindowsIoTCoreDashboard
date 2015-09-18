using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using WindowsIoTDashboard.App.Models;
using WindowsIoTDashboard.App.Services;
using System.Collections.ObjectModel;

namespace WindowsIoTDashboard.App.ViewModels
{
    public class ProcessesViewModel : ViewModelBase, IViewModel
    {
        #region ProcessViewModel

        public class ProcessViewModel : ViewModelBase
        {
            #region Properties

            public ProcessesViewModel Parent { get; private set; }

            private float _cpuUsage;
            public float CPUUsage
            {
                get { return _cpuUsage; }
                set { Set(ref _cpuUsage, value > 100 ? 100 : value); }
            }

            private string _imagename;
            public string ImageName
            {
                get { return _imagename; }
                set { Set(ref _imagename, value); }
            }

            private float _privateWorkingSet;
            public float PrivateWorkingSet
            {
                get { return _privateWorkingSet; }
                set { Set(ref _privateWorkingSet, value / 1048576); }
            }

            private int _processId;
            public int ProcessId
            {
                get { return _processId; }
                set { Set(ref _processId, value); }
            }

            private string _userName;
            public string UserName
            {
                get { return _userName; }
                set { Set(ref _userName, value); }
            }

            private float _virtualSize;
            public float VirtualSize
            {
                get { return _virtualSize; }
                set { Set(ref _virtualSize, value / 1048576); }
            }

            private float _workingSetSize;
            public float WorkingSetSize
            {
                get { return _workingSetSize; }
                set { Set(ref _workingSetSize, value / 1048576); }
            }

            public Visibility TerminateButtonVisibility
            {
                get { return ProcessId > 0 ? Visibility.Visible : Visibility.Collapsed; }
            }

            private RelayCommand<int> _terminateCommand;
            public RelayCommand<int> TerminateCommand
            {
                get
                {
                    return _terminateCommand ?? (_terminateCommand = new RelayCommand<int>(async processId => await Parent.TerminateProcesAsync(processId), processId => processId > 0));
                }
            }

            #endregion

            #region Methods

            public static ProcessViewModel FromProcessModel(ProcessesViewModel parent, ProcessesModel.Process process)
            {
                return new ProcessViewModel
                {
                    Parent = parent,
                    CPUUsage = process.CPUUsage,
                    ImageName = process.ImageName,
                    PrivateWorkingSet = process.PrivateWorkingSet,
                    ProcessId = process.ProcessId,
                    UserName = process.UserName,
                    VirtualSize = process.VirtualSize,
                    WorkingSetSize = process.WorkingSetSize
                };
            }

            public void Update(ProcessesModel.Process process)
            {
                CPUUsage = process.CPUUsage;
                ImageName = process.ImageName;
                PrivateWorkingSet = process.PrivateWorkingSet;
                UserName = process.UserName;
                VirtualSize = process.VirtualSize;
                WorkingSetSize = process.WorkingSetSize;
            }

            #endregion
        }

        #endregion

        #region Fields

        private IRestService _restService;
        private IUserInterfaceService _userInterfaceService;
        private DispatcherTimer _timer;

        #endregion

        #region Constructors

        public ProcessesViewModel(IRestService restService, IUserInterfaceService userInterfaceService)
        {
            _restService = restService;
            _userInterfaceService = userInterfaceService;
        }

        #endregion

        #region Properties

        private ObservableCollection<ProcessViewModel> _processes;
        public ObservableCollection<ProcessViewModel> ProcessesModel
        {
            get { return _processes; }
            private set { Set(ref _processes, value); }
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

        public async Task TerminateProcesAsync(int processId)
        {
            var process = ProcessesModel.FirstOrDefault(p => p.ProcessId == processId);
            var commands = new List<UICommand>
                    {
                        new UICommand("Yes, kill it", async (cmd) =>
                        {
                            try
                            {
                                await _restService.PostAsync(new Uri(String.Format("/api/taskmanager/stop?pid={0}", processId), UriKind.Relative), null);
                                ProcessesModel.Remove(process);
                            }
                            catch (Exception ex)
                            {
                                await _userInterfaceService.ShowFeedbackAsync(ex);
                            }
                        }),
                        new UICommand("No, be nice")
                    };
            await _userInterfaceService.ShowDialogAsync("Terminate Process Confirmation",
                String.Format("Are you sure you want to terminate the '{0}' process?", process.ImageName), commands, 1);
        }

        public Task InitializeAsync()
        {
            _timer = new DispatcherTimer();
            _timer.Tick += Timer_Tick;
            _timer.Start();
            return Task.FromResult(0);
        }

        public Task UnInitializeAsync()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Tick -= Timer_Tick;
            }
            ProcessesModel = null;
            return Task.FromResult(0);
        }

        private async void Timer_Tick(object sender, object e)
        {
            try
            {
                _timer.Stop();
                _timer.Interval = TimeSpan.FromSeconds(1);
                var processModel = await _restService.GetAsync<ProcessesModel>(new Uri("api/resourcemanager/processes", UriKind.Relative));
                if (ProcessesModel == null || ProcessesModel.Count <= 1)
                {
                    ProcessesModel = new ObservableCollection<ProcessViewModel>();
                    foreach (var process in processModel.Processes)
                        ProcessesModel.Add(ProcessViewModel.FromProcessModel(this, process));
                }
                else
                {
                    // Update displayed processes.  Ignore any processes that were starter or terminated until the next page transition.
                    foreach (var process in processModel.Processes)
                    {
                        var displayProcess = ProcessesModel.Where(p => p.ProcessId == process.ProcessId).FirstOrDefault();
                        if (displayProcess == null) continue;
                        displayProcess.Update(process);
                    }   // for each process
                }
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

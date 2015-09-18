using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using WindowsIoTDashboard.App.Services;

namespace WindowsIoTDashboard.App.ViewModels
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //if (ViewModelBase.IsInDesignModeStatic)

            // Services
            SimpleIoc.Default.Register<ISettingsService, SettingsService>();
            SimpleIoc.Default.Register<IRestService, RestService>();
            SimpleIoc.Default.Register<IUserInterfaceService, UserInterfaceService>();

            // View Models
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<HomeViewModel>();
            SimpleIoc.Default.Register<ProcessesViewModel>();
            SimpleIoc.Default.Register<DevicesViewModel>();
            SimpleIoc.Default.Register<NetworkingViewModel>();
            SimpleIoc.Default.Register<AppsViewModel>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This non-static member is needed for data binding purposes.")]
        public HomeViewModel Home
        {
            get { return ServiceLocator.Current.GetInstance<HomeViewModel>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This non-static member is needed for data binding purposes.")]
        public ProcessesViewModel Processes
        {
            get { return ServiceLocator.Current.GetInstance<ProcessesViewModel>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This non-static member is needed for data binding purposes.")]
        public DevicesViewModel Devices
        {
            get { return ServiceLocator.Current.GetInstance<DevicesViewModel>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This non-static member is needed for data binding purposes.")]
        public NetworkingViewModel Networking
        {
            get { return ServiceLocator.Current.GetInstance<NetworkingViewModel>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This non-static member is needed for data binding purposes.")]
        public AppsViewModel Apps
        {
            get { return ServiceLocator.Current.GetInstance<AppsViewModel>(); }
        }
    }
}

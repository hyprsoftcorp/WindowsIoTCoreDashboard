using System.Threading.Tasks;

namespace WindowsIoTDashboard.App.ViewModels
{
    public interface IViewModel
    {
        Task InitializeAsync();
        Task UnInitializeAsync();
    }
}

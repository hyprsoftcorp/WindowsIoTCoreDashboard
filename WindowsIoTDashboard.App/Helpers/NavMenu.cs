using System.Collections.Generic;
using WindowsIoTDashboard.App.Pages;

namespace WindowsIoTDashboard.App.Helpers
{
    public class NavMenu
    {
        public NavMenu()
        {
               MenuItems = new List<NavItem>() {
                new NavItem("\uE10F","Home", typeof(HomePage)),
                new NavItem("\uE138","Apps", typeof(AppsPage)),
                new NavItem("\uE83B","Devices", typeof(DevicesPage)),
                new NavItem("\uE704","Networking", typeof(NetworkingPage)),
                new NavItem("\uE81E","Processes", typeof(ProcessesPage))
            };
        }

        public IReadOnlyList<NavItem> MenuItems { get; private set; }
    }
}

using System.Collections.ObjectModel;

namespace WindowsIoTDashboard.App.Models
{
    public class AppsModel
    {
        public class Package
        {
            public string Name { get; set; }
            public string PackageFamilyName { get; set; }
            public string PackageFullName { get; set; }
            public string PackageRelativeId { get; set; }
            public override string ToString()
            {
                return PackageFullName;
            }
        }

        public AppsModel()
        {
            InstalledPackages = new ObservableCollection<Package>();
        }

        public ObservableCollection<Package> InstalledPackages { get; set; }
    }
}

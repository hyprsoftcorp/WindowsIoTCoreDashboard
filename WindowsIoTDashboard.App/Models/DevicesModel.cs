using System.Collections.Generic;

namespace WindowsIoTDashboard.App.Models
{
    public class DevicesModel
    {
        public class Device
        {
            public string Class { get; set; }
            public string Description { get; set; }
            public string ID { get; set; }
            public string Manufacturer { get; set; }
            public string ParentID { get; set; }
            public int ProblemCode { get; set; }
            public long StatusCode { get; set; }
            public string FriendlyName { get; set; }
            public int Level { get; set; }
            public bool HasChildren { get; set; }
            public override string ToString()
            {
                return !string.IsNullOrEmpty(Description) ? Description : ID;
            }
        }

        public DevicesModel()
        {
            DeviceList = new List<Device>();
        }

        public List<Device> DeviceList { get; set; }
    }
}

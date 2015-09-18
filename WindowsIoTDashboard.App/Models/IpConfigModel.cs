
namespace WindowsIoTDashboard.App.Models
{
    public class IpConfigModel
    {
        public class Adapter
        {
            public string Description { get; set; }
            public string HardwareAddress { get; set; }
            public int Index { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public DHCP DHCP { get; set; }
            public Gateway[] Gateways { get; set; }
            public Ipaddress[] IpAddresses { get; set; }
        }

        public class DHCP
        {
            public int LeaseExpires { get; set; }
            public int LeaseObtained { get; set; }
            public Address Address { get; set; }
        }

        public class Address
        {
            public string IpAddress { get; set; }
            public string Mask { get; set; }
        }

        public class Gateway
        {
            public string IpAddress { get; set; }
            public string Mask { get; set; }
        }

        public class Ipaddress
        {
            public string IpAddress { get; set; }
            public string Mask { get; set; }
        }

        public Adapter[] Adapters { get; set; }
    }
}

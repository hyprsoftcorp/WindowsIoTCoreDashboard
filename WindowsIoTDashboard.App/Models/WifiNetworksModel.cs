
namespace WindowsIoTDashboard.App.Models
{
    public class WifiNetworksModel
    {
        public class Availablenetwork
        {
            public bool AlreadyConnected { get; set; }
            public string AuthenticationAlgorithm { get; set; }
            public string CipherAlgorithm { get; set; }
            public int Connectable { get; set; }
            public string InfrastructureType { get; set; }
            public bool ProfileAvailable { get; set; }
            public string ProfileName { get; set; }
            public string SSID { get; set; }
            public int SecurityEnabled { get; set; }
            public int SignalQuality { get; set; }
            public string[] PhysicalTypes { get; set; }
            public override string ToString()
            {
                return SSID;
            }
        }

        public Availablenetwork[] Availablenetworks { get; set; }
    }
}

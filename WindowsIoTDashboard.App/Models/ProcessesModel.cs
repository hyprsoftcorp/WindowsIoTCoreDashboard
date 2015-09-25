using System;

namespace WindowsIoTDashboard.App.Models
{
    public class ProcessesModel
    {
        public class Process
        {
            public double CPUUsage { get; set; }
            public string ImageName { get; set; }
            public double PrivateWorkingSet { get; set; }
            public int ProcessId { get; set; }
            public int SessionId { get; set; }
            public string UserName { get; set; }
            public double VirtualSize { get; set; }
            public double WorkingSetSize { get; set; }
            public string PackageFullName { get; set; }
            public override string ToString()
            {
                return String.Format("{0} ({1})", ImageName, ProcessId);
            }
        }

        public Process[] Processes { get; set; }
    }
}
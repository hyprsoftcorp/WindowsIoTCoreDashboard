using System;

namespace WindowsIoTDashboard.App.Models
{
    public class ProcessesModel
    {
        public class Process
        {
            public float CPUUsage { get; set; }
            public string ImageName { get; set; }
            public float PrivateWorkingSet { get; set; }
            public int ProcessId { get; set; }
            public int SessionId { get; set; }
            public string UserName { get; set; }
            public float VirtualSize { get; set; }
            public float WorkingSetSize { get; set; }
            public string PackageFullName { get; set; }
            public override string ToString()
            {
                return String.Format("{0} ({1})", ImageName, ProcessId);
            }
        }

        public Process[] Processes { get; set; }
    }
}
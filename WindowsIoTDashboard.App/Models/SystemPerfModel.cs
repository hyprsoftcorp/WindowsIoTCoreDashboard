
namespace WindowsIoTDashboard.App.Models
{
    public class SystemPerfModel
    {
        public class Gpudata
        {
            public Availableadapter[] AvailableAdapters { get; set; }
        }

        public class Availableadapter
        {
            public int DedicatedMemory { get; set; }
            public int DedicatedMemoryUsed { get; set; }
            public string Description { get; set; }
            public int SystemMemory { get; set; }
            public int SystemMemoryUsed { get; set; }
        }

        public class Networkingdata
        {
            public int NetworkInBytes { get; set; }
            public int NetworkOutBytes { get; set; }
        }

        public int AvailablePages { get; set; }
        public int CommitLimit { get; set; }
        public int CommittedPages { get; set; }
        public int CpuLoad { get; set; }
        public int IOOtherSpeed { get; set; }
        public int IOReadSpeed { get; set; }
        public int IOWriteSpeed { get; set; }
        public int NonPagedPoolPages { get; set; }
        public int PageSize { get; set; }
        public int PagedPoolPages { get; set; }
        public int TotalPages { get; set; }
        public Gpudata GPUData { get; set; }
        public Networkingdata NetworkingData { get; set; }
    }
}

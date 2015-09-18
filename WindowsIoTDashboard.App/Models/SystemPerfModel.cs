
namespace WindowsIoTDashboard.App.Models
{
    public class SystemPerfModel
    {
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
    }
}

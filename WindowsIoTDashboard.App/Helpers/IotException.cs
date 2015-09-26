using System;

namespace WindowsIoTDashboard.App.Helpers
{
    public class IotException : Exception
    {
        public IotException(string message) : base(message)
        { }

        public IotException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}

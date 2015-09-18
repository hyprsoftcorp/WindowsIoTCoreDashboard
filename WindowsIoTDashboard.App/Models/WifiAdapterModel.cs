
using System;

namespace WindowsIoTDashboard.App.Models
{
    public class WifiAdapterModel
    {
        public class Interface
        {
            public string Description { get; set; }
            public Guid GUID { get; set; }
            public int Index { get; set; }
            public Profileslist[] ProfilesList { get; set; }
            public override string ToString()
            {
                return Description;
            }
        }

        public class Profileslist
        {
            public bool GroupPolicyProfile { get; set; }
            public string Name { get; set; }
            public bool PerUserProfile { get; set; }
        }

        public Interface[] Interfaces { get; set; }
    }
}

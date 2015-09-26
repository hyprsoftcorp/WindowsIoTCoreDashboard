
using System;
using Windows.ApplicationModel;

namespace WindowsIoTDashboard.App.Services
{
    public interface ISettingsService
    {
        string DeviceName { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        bool IsFirstRun { get; set; }
        string RunCommandText { get; set; }
        bool IsRunAsDefaultAccount { get; set; }
    }

    public class SettingsService : ISettingsService
    {
        public string DeviceName
        {
            get
            {
                return Windows.Storage.ApplicationData.Current.RoamingSettings.Values["DeviceName"] == null ?
                    "minwinpc" : Windows.Storage.ApplicationData.Current.RoamingSettings.Values["DeviceName"].ToString();
            }
            set
            {
                Windows.Storage.ApplicationData.Current.RoamingSettings.Values["DeviceName"] = value;
            }
        }

        public string Username
        {
            get
            {
                return Windows.Storage.ApplicationData.Current.RoamingSettings.Values["Username"] == null ?
                    "administrator" : Windows.Storage.ApplicationData.Current.RoamingSettings.Values["Username"].ToString();
            }
            set
            {
                Windows.Storage.ApplicationData.Current.RoamingSettings.Values["Username"] = value;
            }
        }

        public string Password
        {
            get
            {
                return Windows.Storage.ApplicationData.Current.RoamingSettings.Values["Password"] == null ?
                    "p@ssw0rd" : Windows.Storage.ApplicationData.Current.RoamingSettings.Values["Password"].ToString();
            }
            set
            {
                Windows.Storage.ApplicationData.Current.RoamingSettings.Values["Password"] = value;
            }
        }

        public bool IsFirstRun
        {
            get
            {
                return Windows.Storage.ApplicationData.Current.LocalSettings.Values["IsFirstRun"] == null ?
                    true : bool.Parse(Windows.Storage.ApplicationData.Current.LocalSettings.Values["IsFirstRun"].ToString());
            }
            set
            {
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["IsFirstRun"] = value;
            }
        }

        public string RunCommandText
        {
            get
            {
                return Windows.Storage.ApplicationData.Current.RoamingSettings.Values["RunCommandText"] == null ?
                    String.Empty : Windows.Storage.ApplicationData.Current.RoamingSettings.Values["RunCommandText"].ToString();
            }
            set
            {
                Windows.Storage.ApplicationData.Current.RoamingSettings.Values["RunCommandText"] = value;
            }
        }

        public bool IsRunAsDefaultAccount
        {
            get
            {
                return Windows.Storage.ApplicationData.Current.RoamingSettings.Values["IsRunAsDefaultAccount"] == null ?
                    true : bool.Parse(Windows.Storage.ApplicationData.Current.RoamingSettings.Values["IsRunAsDefaultAccount"].ToString());
            }
            set
            {
                Windows.Storage.ApplicationData.Current.RoamingSettings.Values["IsRunAsDefaultAccount"] = value;
            }
        }

        public string AppVersion
        {
            get { return String.Format(" v{0}.{1}.{2}.{3}", Package.Current.Id.Version.Major, Package.Current.Id.Version.Minor, Package.Current.Id.Version.Build, Package.Current.Id.Version.Revision); }
        }
    }
}

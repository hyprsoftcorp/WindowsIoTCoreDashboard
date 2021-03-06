﻿using Microsoft.HockeyApp;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WindowsIoTDashboard.App.Helpers;
using WindowsIoTDashboard.App.Models;

namespace WindowsIoTDashboard.App.Services
{
    public interface IRestService
    {
        IHockeyClient TelemetryClient { get; }

        Task<T> GetAsync<T>(Uri uri) where T : class;
        Task PostAsync(Uri uri);
        Task DeleteAsync(Uri uri);
    }

    public class RestService : IRestService
    {
        #region Fields

        private static readonly string SystemPerfModelErrorPrefix = "{\"Reason\" : \"";
        private static readonly string SystemPerfModelErrorPostfix = "\"}";

        private enum RequestType
        {
            Delete,
            Post
        }

        private ISettingsService _settingsService;

        #endregion

        #region Constructors

        public RestService(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        #endregion

        #region Properties

        public IHockeyClient TelemetryClient { get { return HockeyClient.Current; } }

        #endregion

        #region Methods

        public async Task<T> GetAsync<T>(Uri uri) where T : class
        {
            if (uri.IsAbsoluteUri)
                throw new UriFormatException("The uri must be a relative uri.");

            using (var client = new HttpClient())
            {
                try
                {
                    SetAuthorizationHeader(client, _settingsService);
                    var response = await client.GetAsync(new Uri(new Uri(String.Format("http://{0}:8080/", _settingsService.DeviceName)), uri));
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();

                        // This apparently is a known issue.  See https://github.com/Microsoft/WindowsDevicePortalWrapper/blob/master/WindowsDevicePortalWrapper/WindowsDevicePortalWrapper.Shared/HttpRest/RestGet.cs
                        if (typeof(T) == typeof(SystemPerfModel))
                        {
                            // Recover from an error in which SystemPerformanceInformation is returned with an incorrect prefix, postfix and the message converted into JSON a second time.
                            if (json.StartsWith(SystemPerfModelErrorPrefix, StringComparison.OrdinalIgnoreCase) && json.EndsWith(SystemPerfModelErrorPostfix, StringComparison.OrdinalIgnoreCase))
                            {
                                // Remove the incorrect prefix and postfix from the JSON message.
                                json = json.Substring(SystemPerfModelErrorPrefix.Length, json.Length - SystemPerfModelErrorPrefix.Length - SystemPerfModelErrorPostfix.Length);

                                // Undo the second JSON conversion.
                                json = Regex.Replace(json, "\\\\\"", "\"", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                                json = Regex.Replace(json, "\\\\\\\\", "\\", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                            }   // contains malformed json
                        }   // system performance  model

                        return JsonConvert.DeserializeObject<T>(json);
                    }
                    else
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        throw new HttpRequestException(String.Format("Error {0}: {1}", (int)response.StatusCode, String.IsNullOrEmpty(error) ? response.ReasonPhrase : error));
                    }
                }
                catch (HttpRequestException ex)
                {
                    if (IsConnectivityException(ex))
                        throw new IotException(String.Format("We are unable to connect to the Windows 10 IoT Core device named '{0}'.  Please ensure your device connection settings are correct and that you have network connectivity.", _settingsService.DeviceName));
                    else
                        throw;
                }
            }   // using http client
        }

        public async Task PostAsync(Uri uri)
        {
            await ExecuteRequest(uri, RequestType.Post);
        }

        public async Task DeleteAsync(Uri uri)
        {
            await ExecuteRequest(uri, RequestType.Delete);
        }

        private static void SetAuthorizationHeader(HttpClient client, ISettingsService settingsService)
        {
            var byteArray = Encoding.ASCII.GetBytes(String.Format(@"{0}:{1}", settingsService.Username, settingsService.Password));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        private async Task ExecuteRequest(Uri uri, RequestType type)
        {
            if (uri.IsAbsoluteUri)
                throw new UriFormatException("The uri must be a relative uri.");

            using (var client = new HttpClient())
            {
                try
                {
                    SetAuthorizationHeader(client, _settingsService);
                    HttpResponseMessage response;
                    if (type == RequestType.Delete)
                        response = await client.DeleteAsync(new Uri(new Uri(String.Format("http://{0}:8080/", _settingsService.DeviceName)), uri));
                    else
                        response = await client.PostAsync(new Uri(new Uri(String.Format("http://{0}:8080/", _settingsService.DeviceName)), uri), null);
                    if (!response.IsSuccessStatusCode)
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        throw new HttpRequestException(String.Format("Error ({0}): {1}", (int)response.StatusCode, String.IsNullOrEmpty(error) ? response.ReasonPhrase : error));
                    }
                }
                catch (HttpRequestException ex)
                {
                    if (IsConnectivityException(ex))
                        throw new IotException(String.Format("We are unable to connect to the Windows 10 IoT Core device named '{0}'.  Please ensure your device connection settings are correct and that you have network connectivity.", _settingsService.DeviceName));
                    else
                        throw;
                }
            }   // using http client
        }

        private bool IsConnectivityException(HttpRequestException ex)
        {
            return ex.InnerException != null && (ex.InnerException.Message.ToLower().Contains("a connection with the server could not be established") ||
                ex.InnerException.Message.ToLower().Contains("the server name or address could not be resolved"));
        }

        #endregion
    }
}

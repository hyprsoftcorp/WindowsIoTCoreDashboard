using GalaSoft.MvvmLight.Messaging;
using Microsoft.ApplicationInsights;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WindowsIoTDashboard.App.Services
{
    public interface IRestService
    {
        TelemetryClient TelemetryClient { get; }
        Task<T> GetAsync<T>(Uri uri) where T : class;
        Task PostAsync(Uri uri, HttpContent content);
    }

    public class RestService : IRestService
    {
        private ISettingsService _settingsService;

        public RestService(ISettingsService settingsService)
        {
            _settingsService = settingsService;
            TelemetryClient = new TelemetryClient();
        }

        public TelemetryClient TelemetryClient { get; private set; }

        public async Task<T> GetAsync<T>(Uri uri) where T : class
        {
            if (uri.IsAbsoluteUri)
                throw new UriFormatException("The uri must be a relative uri.");

            using (var client = new HttpClient())
            {
                SetAuthorizationHeader(client, _settingsService);
                var response = await client.GetAsync(new Uri(new Uri(String.Format("http://{0}:8080/", _settingsService.DeviceName)), uri));
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }   // using http client
        }

        public async Task PostAsync(Uri uri, HttpContent content)
        {
            if (uri.IsAbsoluteUri)
                throw new UriFormatException("The uri must be a relative uri.");

            using (var client = new HttpClient())
            {
                SetAuthorizationHeader(client, _settingsService);
                var response = await client.PostAsync(new Uri(new Uri(String.Format("http://{0}:8080/", _settingsService.DeviceName)), uri), content);
                response.EnsureSuccessStatusCode();
            }   // using http client
        }

        private static void SetAuthorizationHeader(HttpClient client, ISettingsService settingsService)
        {
            var byteArray = Encoding.ASCII.GetBytes(String.Format(@"{0}\{1}:{2}", settingsService.DeviceName, settingsService.Username, settingsService.Password));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }
    }
}

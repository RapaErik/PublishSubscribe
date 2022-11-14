using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using PublishSubscribe.Models;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading;

namespace PublishSubscribe.Services
{
    public class SenderService : ISenderService
    {
        private readonly HttpClient _httpClient;
        private readonly Logger _logger;
        public SenderService(HttpClient httpClient)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _httpClient = httpClient;
        }
        public async Task<bool> SendPostAsync(string url, string message)
        {
            using var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromMilliseconds(10000));

            try
            {
                var data = new StringContent(message, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, data, cts.Token);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return false;
            }
        }
    }
}

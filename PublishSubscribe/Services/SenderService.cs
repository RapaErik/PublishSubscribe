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
        public Task<bool> SendPing()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SendPost(string url, string message)
        {
            bool result = false;
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromMilliseconds(10000));

            try
            {
                var data = new StringContent(message, Encoding.UTF8, "application/json");
              
                var response = await _httpClient.PostAsync(url, data, cts.Token);
                if(response.IsSuccessStatusCode)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                result = false;
            }
            finally
            {
                cts.Dispose();
            }
            return result;

        }
    }
}

using Contracts;
using NLog;
using System.Text;
using System;
using Newtonsoft.Json;

namespace SubClient1.Services
{
    public class SubscriberClient : ISubscriberClient
    {
        private readonly string _brokerSubscribeUrl= "https://127.0.0.1:7014/subscribe";
        private readonly string _brokerUnubscribeUrl = "https://127.0.0.1:7014/unsubscribe";
        private readonly HttpClient _httpClient;
        private readonly Logger _logger;
        public SubscriberClient(HttpClient httpClient)
        {
            
                _logger = LogManager.GetCurrentClassLogger();
                _httpClient = httpClient;
        }

        public async Task<bool> Subscribe(SubscribeContract subscribe)
        {
            bool result = false;
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromMilliseconds(10000));

            try
            {
                var message = JsonConvert.SerializeObject(subscribe);
                var data = new StringContent(message, Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Add("Token",subscribe.Token);

                var response = await _httpClient.PostAsync(_brokerSubscribeUrl, data, cts.Token);
                if (response.IsSuccessStatusCode)
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

        public async Task<bool> Unsubscribe(SubscribeContract unsubscribe)
        {
            bool result = false;
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromMilliseconds(10000));

            try
            {
                var message = JsonConvert.SerializeObject(unsubscribe);
                var data = new StringContent(message, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_brokerUnubscribeUrl, data, cts.Token);
                if (response.IsSuccessStatusCode)
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

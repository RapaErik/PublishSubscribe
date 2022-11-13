using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class ContractReceived<T> where T : IContract
    {
        public long PublishedBy { get; set; }
        public string Topic { get; set; }
        public string Token { get; set; }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                var json = CryptoAESManager.Dencrypt(_message);
                _data = JsonConvert.DeserializeObject<T>(json);
            }
        }

        private T _data;

        public T Data
        {
            get { return _data; }
        }


    }
}

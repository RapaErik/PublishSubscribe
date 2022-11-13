using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class ContractSend<T> where T : IContract
    {
        public long PublisherId { get; set; }
        public string Topic { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public ContractSend(T data, long publisherId,string topic, string token)
        {
            PublisherId = publisherId;
            Topic = topic;  
            Token = token;
            Message = JsonConvert.SerializeObject(data);
        }
    }
}

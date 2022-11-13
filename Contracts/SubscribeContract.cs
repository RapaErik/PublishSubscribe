using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class SubscribeContract
    {
        public long SubscriberId { get; set; }
        public string? Token { get; set; }
        public string? Topic { get; set; }
        public string? ClientUrl { get; set; }
    }
}

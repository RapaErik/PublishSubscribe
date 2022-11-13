using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;

namespace PublishSubscribe.Models
{
    public class Subscriber : IEquatable<Subscriber>
    {
        public long Id { get; set; }
        public string Topic { get; set; }
        public string ResponseUrl { get; set; }

        public bool Equals(Subscriber? other)
        {
           return this.Id == other?.Id && this.Topic == other?.Topic;
        }
    }

    public class SubscriberEqualityComparer : IEqualityComparer<Subscriber>
    {
        public bool Equals(Subscriber? x, Subscriber? y)
        {
            if(x == null && y == null)
            {
                return true;
            }    
            
            if(x ==null || y == null)
            {
                return false;
            }

            return x.Equals(y);
        }

        public int GetHashCode([DisallowNull] Subscriber obj)
        {
            return JsonConvert.SerializeObject(obj).GetHashCode();
        }
    }
}

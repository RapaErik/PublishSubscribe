using Microsoft.Extensions.Options;
using PublishSubscribe.Configuration;
using System.Reflection;

namespace PublishSubscribe.Auth
{
    public class AuthContext : IAuthContext
    {
        private readonly List<string> _listOfTokens;

        public AuthContext(IOptions<TokensConfiguration> tokens)
        {
            _listOfTokens = tokens.Value.ListOfTokens;
        }
        public bool Authenticate(string token)
        {
            return _listOfTokens.Contains(token);
        }
    }
}

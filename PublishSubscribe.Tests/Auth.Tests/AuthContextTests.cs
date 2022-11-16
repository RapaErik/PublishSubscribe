using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using PublishSubscribe.Auth;
using PublishSubscribe.Configuration;
using PublishSubscribe.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PublishSubscribe.Tests.Auth.Tests
{
    public class AuthContextTests
    {
        private readonly IOptions<TokensConfiguration> _tokensConfig = Options.Create<TokensConfiguration>(new TokensConfiguration
        {
            ListOfTokens = new List<string> {
                "token1",
                "token2",
                "token3"
            }
        });

        [Fact]
        public async Task ValidToken_ReturnsTrue()
        {
            // Arrange

            string token = "token1";
            AuthContext authContext = new AuthContext(_tokensConfig);
   
            // Act

            bool res =  authContext.Authenticate(token);

            // Assert
            Assert.True(res);
        }
        [Fact]
        public async Task InvalidToken_ReturnsFalse()
        {
            // Arrange

            string token = "token4";
            AuthContext authContext = new AuthContext(_tokensConfig);

            // Act

            bool res = authContext.Authenticate(token);

            // Assert
            Assert.False(res);
        }

    }
}

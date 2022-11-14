using Microsoft.AspNetCore.Http;
using Moq;
using Moq.Protected;
using PublishSubscribe.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PublishSubscribe.Tests.Services.Tests
{
    public class SenderServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        public SenderServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        }

        [Fact]
        public async Task SendPost_ResponseOk_ReturnsTrue()
        {
            // Arrange

            string url = "https://test.com";
            string message = "dataTest";

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{ok}"),
            };


            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                                                      "SendAsync",
                                                      ItExpr.IsAny<HttpRequestMessage>(),
                                                      ItExpr.IsAny<CancellationToken>())
                                                   .ReturnsAsync(response);

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);



            var service = new SenderService(httpClient);

            // Act

            var res = await service.SendPostAsync(url, message);

            // Assert
            Assert.True(res);
        }
        [Fact]
        public async Task SendPost_Failure_Exeption_ReturnsFalse()
        {
            // Arrange

            string url = "https://test.com";
            string message = "dataTest";

            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                                                      "SendAsync",
                                                      ItExpr.IsAny<HttpRequestMessage>(),
                                                      ItExpr.IsAny<CancellationToken>())
                                                   .ThrowsAsync(new Exception());

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);


            var service = new SenderService(httpClient);

            // Act

            var res = await service.SendPostAsync(url, message);

            // Assert
            Assert.False(res);
        }
    }
}

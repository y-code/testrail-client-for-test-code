using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using Moq;

namespace Ycode.TestRailClient.V2.Test
{
    public abstract class TestBase
    {
        protected (Mock<IWebClient> , Mock<IWebClientFactory>) MockWebClientFactory(Dictionary<string, object> apiMockMap)
        {
            var webClient = new Mock<IWebClient>();
            foreach (var apiMock in apiMockMap)
            {
                var requestUrl = apiMock.Key;
                var responseData = apiMock.Value;

                webClient.Setup(c => c.OpenReadTaskAsync(It.Is<string>(address => address == requestUrl)))
                    .ReturnsAsync(() =>
                    {
                        var stream = new MemoryStream();
                        var writer = new Utf8JsonWriter(stream);
                        JsonSerializer.Serialize(writer, responseData);
                        stream.Position = 0;
                        return stream;
                    });
            }

            var webClientFactory = new Mock<IWebClientFactory>();
            webClientFactory.Setup(f => f.Create(It.IsAny<WebHeaderCollection>()))
                .Returns(webClient.Object);

            return (webClient, webClientFactory);
        }
    }
}

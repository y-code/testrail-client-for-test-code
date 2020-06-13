using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Ycode.TestRailClient.Test.ApiMock.Models;

namespace Ycode.TestRailClient.V2.Test
{
    [TestFixture]
    public class StatusTest : TestBase
    {
        const string requestUrl = "https://test:1234/index.php?/api/v2/get_statuses";

        private static readonly IEnumerable<StatusV2> data_statuses_1
            = new List<StatusV2>
            {
                new StatusV2
                {
                    Id = 1,
                    Name = "status-1",
                    Label = "STATUS",
                },
                new StatusV2
                {
                    Id = 2,
                    Name = "status-2",
                    Label = "STATUS",
                },
                new StatusV2
                {
                    Id = 3,
                    Name = "status-2",
                    Label = "STATUS 3",
                },
            }.AsReadOnly();

        private static readonly TestRailApiClientConfiguration data_config
            = new TestRailApiClientConfiguration
            {
                Url = "https://test:1234/",
                UserName = "test.user",
                AppKey = "qwerasdfzxcv",
            };

        [TestCase()]
        public void TestStatusesMaps()
        {
            var (webClient, webClientFactory) = MockWebClientFactory(
                new Dictionary<string, object>
                {
                    { requestUrl, data_statuses_1 }
                });

            var client = new TestRailApiClient(
                data_config,
                webClientFactory.Object);

            var statuses = client.Statuses;

            Assert.That(statuses, Has.Count.EqualTo(4));
            Assert.That(statuses, Does.ContainKey("status-1"));
            Assert.That(statuses, Does.ContainKey("status-2"));
            Assert.That(statuses, Does.ContainKey("STATUS"));
            Assert.That(statuses, Does.ContainKey("STATUS 3"));

            var statusesById = client.StatusesById;

            Assert.That(statusesById, Has.Count.EqualTo(3));
            Assert.That(statusesById, Does.ContainKey(1));
            Assert.That(statusesById, Does.ContainKey(2));
            Assert.That(statusesById, Does.ContainKey(3));
        }

        [TestCase()]
        public async Task TestStatusesCaching()
        {
            var (webClient, webClientFactory) = MockWebClientFactory(
                new Dictionary<string, object>
                {
                    { requestUrl, data_statuses_1 }
                });

            var client = new TestRailApiClient(
                data_config,
                webClientFactory.Object);

            var statuses1 = client.Statuses;

            webClient.Verify(
                c => c.OpenReadTaskAsync(It.Is<string>(address => address == requestUrl)),
                Times.Exactly(1));

            var statuses2 = client.Statuses;

            webClient.Verify(
                c => c.OpenReadTaskAsync(It.Is<string>(address => address == requestUrl)),
                Times.Exactly(1));

            var statusesById1 = client.StatusesById;

            webClient.Verify(
                c => c.OpenReadTaskAsync(It.Is<string>(address => address == requestUrl)),
                Times.Exactly(1));

            var statusesById2 = client.StatusesById;

            webClient.Verify(
                c => c.OpenReadTaskAsync(It.Is<string>(address => address == requestUrl)),
                Times.Exactly(1));

            var statuses = await client.GetStatusesAsync();

            webClient.Verify(
                c => c.OpenReadTaskAsync(It.Is<string>(address => address == requestUrl)),
                Times.Exactly(2));

            var statuses3 = client.Statuses;

            webClient.Verify(
                c => c.OpenReadTaskAsync(It.Is<string>(address => address == requestUrl)),
                Times.Exactly(2));

            var statusesById3 = client.StatusesById;

            webClient.Verify(
                c => c.OpenReadTaskAsync(It.Is<string>(address => address == requestUrl)),
                Times.Exactly(2));
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Ycode.TestRailClient.Test.ApiMock.Models;

namespace Ycode.TestRailClient.V2.Test
{
    [TestFixture]
    public class PriorityTest : TestBase
    {
        const string requestUrl = "https://test:1234/index.php?/api/v2/get_priorities";

        private static readonly IEnumerable<PriorityV2> data_priorities_1
            = new List<PriorityV2>
            {
                new PriorityV2
                {
                    Id = 1,
                    Name = "P1 - Don't Test",
                    ShortName = "P1",
                    Priority = 1,
                    IsDefault = false,
                },
                new PriorityV2
                {
                    Id = 2,
                    Name = "P1 - Don't Test",
                    ShortName = "P2",
                    Priority = 2,
                    IsDefault = false,
                },
                new PriorityV2
                {
                    Id = 3,
                    Name = "P3 - Test If Time",
                    ShortName = "P2",
                    Priority = 3,
                    IsDefault = false,
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
        public void TestPrioritiesMaps()
        {
            var (webClient, webClientFactory) = MockWebClientFactory(
                new Dictionary<string, object>
                {
                    { requestUrl, data_priorities_1 }
                });

            var client = new TestRailApiClient(
                data_config,
                webClientFactory.Object);

            var priorities = client.Priorities;

            Assert.That(priorities, Has.Count.EqualTo(4));
            Assert.That(priorities, Does.ContainKey("P1 - Don't Test"));
            Assert.That(priorities, Does.ContainKey("P3 - Test If Time"));
            Assert.That(priorities, Does.ContainKey("P1"));
            Assert.That(priorities, Does.ContainKey("P2"));

            var prioritiesById = client.PrioritiesById;

            Assert.That(prioritiesById, Has.Count.EqualTo(3));
            Assert.That(prioritiesById, Does.ContainKey(1));
            Assert.That(prioritiesById, Does.ContainKey(2));
            Assert.That(prioritiesById, Does.ContainKey(3));
        }

        [TestCase()]
        public async Task TestPrioritiesCaching()
        {
            var (webClient, webClientFactory) = MockWebClientFactory(
                new Dictionary<string, object>
                {
                    { requestUrl, data_priorities_1 }
                });

            var client = new TestRailApiClient(
                data_config,
                webClientFactory.Object);

            var priorities1 = client.Priorities;

            webClient.Verify(
                c => c.OpenReadTaskAsync(It.Is<string>(address => address == requestUrl)),
                Times.Exactly(1));

            var priorities2 = client.Priorities;

            webClient.Verify(
                c => c.OpenReadTaskAsync(It.Is<string>(address => address == requestUrl)),
                Times.Exactly(1));

            var prioritiesById1 = client.PrioritiesById;

            webClient.Verify(
                c => c.OpenReadTaskAsync(It.Is<string>(address => address == requestUrl)),
                Times.Exactly(1));

            var prioritiesById2 = client.PrioritiesById;

            webClient.Verify(
                c => c.OpenReadTaskAsync(It.Is<string>(address => address == requestUrl)),
                Times.Exactly(1));

            var priorities = await client.GetPrioritiesAsync();

            webClient.Verify(
                c => c.OpenReadTaskAsync(It.Is<string>(address => address == requestUrl)),
                Times.Exactly(2));

            var priorities3 = client.Priorities;

            webClient.Verify(
                c => c.OpenReadTaskAsync(It.Is<string>(address => address == requestUrl)),
                Times.Exactly(2));

            var prioritiesById3 = client.PrioritiesById;

            webClient.Verify(
                c => c.OpenReadTaskAsync(It.Is<string>(address => address == requestUrl)),
                Times.Exactly(2));
        }
    }
}

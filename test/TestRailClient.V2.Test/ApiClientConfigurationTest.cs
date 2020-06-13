using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Ycode.TestRailClient.Test.ApiMock.Models;

namespace Ycode.TestRailClient.V2.Test
{
    [TestFixture]
    public class ApiClientConfigurationTest : TestBase
    {
        static readonly IEnumerable<StatusV2> data_statuses_1
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

        const string apiUrl_1 = "https://test:1234/";
        const string apiUrl_2 = "https://test:1234";

        static readonly TestRailApiClientConfiguration data_config_1
            = new TestRailApiClientConfiguration
            {
                Url = apiUrl_1,
                UserName = "test.user",
                AppKey = "qwerasdfzxcv",
            };
        static readonly TestRailApiClientConfiguration data_config_2
            = new TestRailApiClientConfiguration
            {
                Url = null,
                UserName = "test.user",
                AppKey = "qwerasdfzxcv",
            };
        static readonly TestRailApiClientConfiguration data_config_3
            = new TestRailApiClientConfiguration
            {
                Url = "",
                UserName = "test.user",
                AppKey = "qwerasdfzxcv",
            };
        static readonly TestRailApiClientConfiguration data_config_4
            = new TestRailApiClientConfiguration
            {
                Url = apiUrl_1,
                UserName = null,
                AppKey = "qwerasdfzxcv",
            };
        static readonly TestRailApiClientConfiguration data_config_5
            = new TestRailApiClientConfiguration
            {
                Url = apiUrl_1,
                UserName = "",
                AppKey = "qwerasdfzxcv",
            };
        static readonly TestRailApiClientConfiguration data_config_6
            = new TestRailApiClientConfiguration
            {
                Url = apiUrl_1,
                UserName = "test.user",
                AppKey = null,
            };
        static readonly TestRailApiClientConfiguration data_config_7
            = new TestRailApiClientConfiguration
            {
                Url = apiUrl_1,
                UserName = "test.user",
                AppKey = "",
            };

        [TestCase(apiUrl_1)]
        [TestCase(apiUrl_2)]
        public async Task TestValidUrlInConfig(string apiUrl)
        {
            var (webClient, webClientFactory) = MockWebClientFactory(
                new Dictionary<string, object>
                {
                    { "https://test:1234/index.php?/api/v2/get_statuses", data_statuses_1 }
                });

            var client = new TestRailApiClient(
                new TestRailApiClientConfiguration
                {
                    Url = apiUrl,
                    UserName = "test.user",
                    AppKey = "qwerasdfzxcv",
                },
                webClientFactory.Object);

            var statuses = await client.GetStatusesAsync();

            Assert.That(statuses, Has.Length.GreaterThan(0));
        }

        static object[][] dataSrc_TestUserNameAndAppKeyInConfig =
        {
            new[] { data_config_2 },
            new[] { data_config_3 },
            new[] { data_config_4 },
            new[] { data_config_5 },
            new[] { data_config_6 },
            new[] { data_config_7 },
        };

        [TestCaseSource("dataSrc_TestUserNameAndAppKeyInConfig")]
        public async Task TestInvalidConfig(TestRailApiClientConfiguration config)
        {

            Assert.CatchAsync<TestRailClientException>(async () =>
            {
                var (webClient, webClientFactory) = MockWebClientFactory(
                    new Dictionary<string, object>
                    {
                        { "https://test:1234/index.php?/api/v2/get_statuses", data_statuses_1 }
                    });

                var client = new TestRailApiClient(
                    config,
                    webClientFactory.Object);
            });
        }
    }
}

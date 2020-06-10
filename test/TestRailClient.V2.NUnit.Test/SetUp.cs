using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using TestRailApiMock = Ycode.TestRailClient.Test.ApiMock.Program;

namespace Ycode.TestRailClient.V2.NUnit.Test
{
    [SetUpFixture]
    public class SetUp
    {
        public const int ApiServiceStartUpTimeoutSeconds = 7;
        Task _testRailMockTask;
        CancellationTokenSource _testRailMockCancellation;

        public static TestRailApiMock TestRailApiMock { get; private set; }

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_URLS", "https://localhost:62182;http://localhost:62181");
            TestRailApiMock = new TestRailApiMock();
            _testRailMockCancellation = new CancellationTokenSource();
            _testRailMockTask = TestRailApiMock.Start(_testRailMockCancellation.Token);

            DateTime start = DateTime.UtcNow;
            DateTime timeout = DateTime.UtcNow + TimeSpan.FromSeconds(ApiServiceStartUpTimeoutSeconds);
            Exception exception = null;
            while (true)
            {
                try
                {
                    await Task.Delay(500);
                    var client = new WebClient();
                    using (var stream = await client.OpenReadTaskAsync(new Uri("https://localhost:62182"))) { }
                    var wakeup = DateTime.UtcNow - start;
                    break;
                }
                catch (Exception e)
                {
                    exception = e;
                    if (e is WebException && ((WebException)e).Status != WebExceptionStatus.UnknownError)
                    {
                        break;
                    }

                    if (DateTime.UtcNow > timeout)
                    {
                        var error = $"Test target API service did not start up within {ApiServiceStartUpTimeoutSeconds} seconds";
                        throw new Exception(error, exception);
                    }
                }
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _testRailMockCancellation.Cancel();
            Task.WaitAll(_testRailMockTask);
        }
    }
}

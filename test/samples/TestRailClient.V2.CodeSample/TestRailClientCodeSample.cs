using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Ycode.TestRailClient.V2.CodeSample
{
    [TestFixture]
    public class TestRailClientCodeSample
    {
        [Test]
        public async Task Simple()
        {
            const int projectId = 10000;
            const int suiteId = 100;

            var caseIds = new[] { 789, 790, 791 };
            var version = "v2.3.1";

            var client = new TestRailClient(
                new TestRailClientConfiguration
                {
                    Url = "https://localhost:62182",
                    UserName = "taro.yamada",
                    AppKey = "aiueo-kakikukeko-sasisuseso",
                });

            await client.StartTestRunAsync(
                new TestRailRunInfo
                {
                    ProjectId = projectId,
                    SuiteId = suiteId,
                    Name = "Test Run Sample",
                });

            try
            {
                // Execute a test
            }
            catch (Exception e)
            {
                await client.RecordResultAsync(
                    caseIds,
                    new TestResult
                    {
                        Status = "Failed",
                        Version = version,
                        Comment = e.Message,
                    });
                throw;
            }

            await client.RecordResultAsync(
                caseIds,
                new TestResult
                {
                    Status = "Passed",
                    Version = version,
                });

            await client.EndTestRunAsync();
        }

        [Test]
        public async Task CaseFilter()
        {
            var client = new TestRailClient(
                new TestRailClientConfiguration
                {
                    Url = "https://localhost:62182",
                    UserName = "taro.yamada",
                    AppKey = "aiueo-kakikukeko-sasisuseso",
                    CaseFilters =
                    {
                        new TestRailPriorityCaseFilter("P2"),
                    },
                });

            await client.StartTestRunAsync(
                new TestRailRunInfo
                {
                    ProjectId = 10000,
                    SuiteId = 100,
                    Name = "Test Run Sample",
                    Description = "This is just a sample. Take it easy~.",
                    IncludeAll = true,
                });

            var targetCaseIds = new[] { 789, 790, 791 };

            string testStatus = null;
            if (client.IsTestRequiredFor(targetCaseIds))
            {
                // Execute a test
                testStatus = "Failed";
            }

            foreach (var (isFilteredCases, caseIds) in client.SplitCaseIdsByFilter(targetCaseIds))
            {
                await client.RecordResultAsync(
                    caseIds,
                    new TestResult
                    {
                        Status = isFilteredCases ? testStatus : "Blocked",
                        Version = "v2.3.1",
                        Comment = "This test failed. The actual result was ...",
                        Defects = { "ASD-8437", "ADS-9852" },
                        Elapsed = TimeSpan.FromMilliseconds(2938),
                    });
            }


            await client.EndTestRunAsync();
        }
    }
}
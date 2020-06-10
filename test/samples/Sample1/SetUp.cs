using System.Threading.Tasks;
using NUnit.Framework;
using Ycode.TestRailClient.V2;
using Ycode.TestRailClient.V2.NUnit;

namespace Sample1
{
    [SetUpFixture]
    public class SetUp
    {
        public static NUnitTestRailClient TestRailClient { get; private set; }

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            TestRailClient = new NUnitTestRailClient(
                new NUnitTestRailClientConfiguration
                {
                    Url = "https://localhost:62182",
                    UserName = "taro.yamada",
                    AppKey = "aiueo-kakikukeko-sasisuseso",
                    CaseFilters =
                    {
                        new TestRailPriorityCaseFilter("P2"),
                    },
                    StatusMapping =
                    {
                        Warning = "Caution",
                        WithDefect = "Known Issue",
                        FilteringResidue = "Excluded",
                    }
                });
            await TestRailClient.StartTestRunAsync(
                new TestRailRunInfo
                {
                    ProjectId = 10000,
                    SuiteId = 100,
                    Name = "Test Run Sample",
                    Description = "This is just a sample. Take it easy~.",
                    IncludeAll = true,
                });
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await TestRailClient.EndTestRunAsync();
        }
    }
}

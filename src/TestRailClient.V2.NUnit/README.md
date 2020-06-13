# TestRail Client for NUnit Test Code

Ycode.TestRailClient.V2.NUnit provides a TestRail API v2 client dedicated to the usage in NUnit test code.

This library implements functionalities typically required to integrate your NUnit test code with TestRail.

# Quick Start

With the code sample below, you start a test run and then close it after recording some test results.

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
                });
            await TestRailClient.StartTestRunAsync(
                new TestRailRunInfo
                {
                    ProjectId = 10000,
                    SuiteId = 100,
                    Name = "Test Run Sample",
                });
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await TestRailClient.EndTestRunAsync();
        }
    }

    public class TestBase
    {
        NUnitTestRailClient _testRailClient => SetUp.TestRailClient;

        [SetUp]
        public void Setup()
        {
            _testRailClient.SetUpSingleTest();
        }

        [TearDown]
        public async Task TearDown()
        {
            await _testRailClient.TearDownSingleTest();
        }
    }

    [TestFixture]
    public class NUnitTestRailClientCodeSample : TestBase
    {
        [Test]
        [TestRailCase(10101)]
        public void Test1()
        {
            Assert.Pass();
        }
    }

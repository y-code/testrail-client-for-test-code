﻿using System.Threading.Tasks;
using NUnit.Framework;
using Ycode.TestRailClient.V2;
using Ycode.TestRailClient.V2.NUnit;

namespace Ycode.TestRailClient.V2.CodeSample
{
    [SetUpFixture]
    public class SetUpWithCaseFilter
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

    [TestFixture]
    public class NUnitTestRailClientCodeSampleWithCaseFilter
    {
        NUnitTestRailClient TestRailClient => SetUp.TestRailClient;

        [SetUp]
        public void Setup()
        {
            TestRailClient.SetUpSingleTest();
        }

        [TearDown]
        public async Task TearDown()
        {
            await TestRailClient.TearDownSingleTest();
        }

        [Test]
        [TestRailCase(10101)]
        [TestRailCase(10103)]
        public void Test1()
        {
            Assert.Pass();
        }

        [TRTestCase("param1", Case = 10109, Defect = "ASD-123")]
        [TRTestCase("param2", Cases = new[] { 10110, 10111 }, Defects = new[] { "ASD-456" })]
        public void Test2(string param)
        {
            Assert.Fail();
        }
    }
}

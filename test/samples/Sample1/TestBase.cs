using System.Threading.Tasks;
using NUnit.Framework;

namespace Sample1
{
    public class TestBase
    {
        [SetUp]
        public void Setup()
        {
            SetUp.TestRailClient.SetUpSingleTest();
        }

        [TearDown]
        public async Task TearDown()
        {
            await SetUp.TestRailClient.TearDownSingleTest();
        }
    }
}

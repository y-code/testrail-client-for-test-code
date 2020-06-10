using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Ycode.TestRailClient.V2.NUnit.Test
{
    [TestFixture]
    public class Tests : TestBase
    {
        [Test]
        public async Task TestSample1()
        {
            var projFile = GetSampleProjectFile("Sample1");

            var result = await RunNUnitTestAsync(projFile.FullName);

            Assert.That(result.Succeeded, Is.False, result.ToString());
            Assert.That(result.Passed, Is.EqualTo(3), result.ToString());
            Assert.That(result.Failed, Is.EqualTo(3), result.ToString());
            Assert.That(result.Skipped, Is.EqualTo(1), result.ToString());

            var resultFor10101 = TestRailApiMock.AccessLog.AddResultForCase
                .SingleOrDefault(a => a.CaseId == 10101);
            var resultFor10102 = TestRailApiMock.AccessLog.AddResultForCase
                .SingleOrDefault(a => a.CaseId == 10102);
            var resultFor10103 = TestRailApiMock.AccessLog.AddResultForCase
                .SingleOrDefault(a => a.CaseId == 10103);
            var resultFor10104 = TestRailApiMock.AccessLog.AddResultForCase
                .SingleOrDefault(a => a.CaseId == 10104);
            var resultFor10105 = TestRailApiMock.AccessLog.AddResultForCase
                .SingleOrDefault(a => a.CaseId == 10105);
            var resultFor10106 = TestRailApiMock.AccessLog.AddResultForCase
                .SingleOrDefault(a => a.CaseId == 10106);
            var resultFor10107 = TestRailApiMock.AccessLog.AddResultForCase
                .SingleOrDefault(a => a.CaseId == 10107);
            var resultFor10108 = TestRailApiMock.AccessLog.AddResultForCase
                .SingleOrDefault(a => a.CaseId == 10108);
            var resultFor10109 = TestRailApiMock.AccessLog.AddResultForCase
                .SingleOrDefault(a => a.CaseId == 10109);
            var resultFor10110 = TestRailApiMock.AccessLog.AddResultForCase
                .SingleOrDefault(a => a.CaseId == 10110);
            var resultFor10111 = TestRailApiMock.AccessLog.AddResultForCase
                .SingleOrDefault(a => a.CaseId == 10111);

            Assert.That(resultFor10101?.Data.Status, Is.EqualTo(103), result.ToString());
            Assert.That(resultFor10102?.Data.Status, Is.EqualTo(5), result.ToString());
            Assert.That(resultFor10103?.Data.Status, Is.EqualTo(1), result.ToString());
            Assert.That(resultFor10104?.Data.Status, Is.EqualTo(5), result.ToString());
            Assert.That(resultFor10105?.Data.Status, Is.EqualTo(103), result.ToString());
            Assert.That(resultFor10106?.Data.Status, Is.EqualTo(103), result.ToString());
            Assert.That(resultFor10107?.Data.Status, Is.EqualTo(1), result.ToString());
            Assert.That(resultFor10108?.Data.Status, Is.EqualTo(101), result.ToString());
            Assert.That(resultFor10109?.Data.Status, Is.EqualTo(101), result.ToString());
            Assert.That(resultFor10110?.Data.Status, Is.EqualTo(101), result.ToString());
            Assert.That(resultFor10111?.Data.Status, Is.EqualTo(101), result.ToString());
        }
    }
}

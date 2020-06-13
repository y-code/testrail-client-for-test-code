using NUnit.Framework;

namespace Ycode.TestRailClient.V2.NUnit
{
    public static class TestContextExtensions
    {
        public static TestCaseData AddTestRailCase(this TestCaseData data, int caseId)
            => data.SetProperty(TestRailCaseAttribute.Name, caseId);
    }
}

namespace Ycode.TestRailClient.V2.NUnit
{
    public class NUnitTestRailClientConfiguration : TestRailClientConfiguration
    {
        public NUnitTestRailStatusMapping StatusMapping { get; set; }
            = new NUnitTestRailStatusMapping();
    }
}

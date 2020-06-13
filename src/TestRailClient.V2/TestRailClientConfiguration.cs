using System.Collections.Generic;

namespace Ycode.TestRailClient.V2
{
    public class TestRailApiClientConfiguration
    {
        public string Url { get; set; }
        public string UserName { get; set; }
        public string AppKey { get; set; }
    }

    public class TestRailClientConfiguration : TestRailApiClientConfiguration
    {
    	public bool Disabled { get; set; } = false;
    	public List<ITestRailCaseFilter> CaseFilters { get; set; } = new List<ITestRailCaseFilter>();
    }
}

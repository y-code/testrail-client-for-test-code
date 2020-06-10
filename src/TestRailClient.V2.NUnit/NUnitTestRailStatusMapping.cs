namespace Ycode.TestRailClient.V2.NUnit
{
	public class NUnitTestRailStatusMapping
    {
    	public string Passed { get; set; } = "Passed";
    	public string Failed { get; set; } = "Failed";
    	public string Skipped { get; set; } = "Blocked";
    	public string Inconclusive { get; set; } = "Retest";
    	public string Warning { get; set; } = "Retest";
    	public string WithDefect { get; set; } = "Blocked";
    	public string FilteringResidue { get; set; } = "Retest";
    }
}

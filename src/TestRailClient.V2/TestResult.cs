using System;
using System.Collections.Generic;

namespace Ycode.TestRailClient.V2
{
	public class TestResult
    {
    	public string Status { get; set; }
    	public string Comment { get; set; }
    	public string Version { get; set; }
    	public IList<string> Defects { get; set; } = new List<string>();
    	public TimeSpan Elapsed { get; set; }
    	public int AssignedToId { get; set; }
    }
}

using System.Collections.Generic;

namespace Ycode.TestRailClient.V2
{
	public interface ITestRailApiCache
    {
    	IReadOnlyDictionary<string, TestRailStatus> Statuses { get; }
    	IReadOnlyDictionary<string, TestRailPriority> Priorities { get; }
    	IReadOnlyDictionary<int, TestRailCase> Cases { get; }
    }
}

using System.Collections.Generic;

namespace Ycode.TestRailClient.V2
{
	public interface ITestRailApiCache
    {
    	IReadOnlyDictionary<int, TestRailStatus> StatusesById { get; }
    	IReadOnlyDictionary<string, TestRailStatus> Statuses { get; }
        IReadOnlyDictionary<int, TestRailPriority> PrioritiesById { get; }
        IReadOnlyDictionary<string, TestRailPriority> Priorities { get; }
        IReadOnlyDictionary<int, TestRailCase> Cases { get; }
    }
}

using System;
using System.Linq;

namespace Ycode.TestRailClient.V2
{
	public class TestRailPriorityCaseFilter : ITestRailCaseFilter
    {
    	public ResidueCaseStrategy Strategy { get; set; }

    	public string Priority { get; set; }

    	public TestRailPriorityCaseFilter(string priority, ResidueCaseStrategy? residueCaseStrategy = null)
        {
        	Priority = priority;
        	Strategy = residueCaseStrategy ?? ResidueCaseStrategy.IncludeToRun;
        }

    	public void Validate(ITestRailApiCache cache)
        {
        	if (!string.IsNullOrEmpty(Priority)
                && !cache.Priorities.Keys.Contains(Priority))
            {
            	throw new TestRailClientException($"Invalid priority, \"{Priority}\", was specified in the case filter for TestRailClient.");
            }
        }

    	public bool Filter(TestRailCase @case, ITestRailApiCache cache)
        {
        	var priorityFilter = cache.Priorities[Priority];
        	return (@case.Priority?.Priority ?? int.MaxValue) >= (priorityFilter?.Priority ?? 0);
        }
    }
}

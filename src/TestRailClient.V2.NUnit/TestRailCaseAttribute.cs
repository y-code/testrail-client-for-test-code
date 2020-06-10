using System;
using NUnit.Framework;

namespace Ycode.TestRailClient.V2.NUnit
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public class TestRailCaseAttribute : PropertyAttribute
    {
    	public static readonly string Name = "Case ID";

    	public int CaseId { get; }

    	public TestRailCaseAttribute(int caseId) : base(Name, caseId)
        {
        	CaseId = caseId;
        }
    }
}

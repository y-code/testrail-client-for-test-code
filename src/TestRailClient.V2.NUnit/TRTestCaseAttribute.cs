using System;
using NUnit.Framework;

namespace Ycode.TestRailClient.V2.NUnit
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public class TRTestCaseAttribute : TestCaseAttribute
    {
    	public static readonly string Name = "Test ID";

    	public bool IsCasePopulated { get; private set; }
    	private int _case;
    	public int Case
        {
        	get => _case;
        	set
            {
            	IsCasePopulated = true;
            	_case = value;
            }
        }
    	public int[] Cases { get; set; }
    	public string Defect { get; set; }
    	public string[] Defects { get; set; }

    	public TRTestCaseAttribute(params object[] arguments) : base(arguments)
        {
        }
    }
}

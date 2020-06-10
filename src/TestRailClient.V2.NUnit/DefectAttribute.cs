using System;
using NUnit.Framework;

namespace Ycode.TestRailClient.V2.NUnit
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public class DefectAttribute : PropertyAttribute
    {
    	public static readonly string Name = "Defect";

    	public string DefectId { get; }

    	public DefectAttribute(string defectId) : base(Name, defectId)
        {
        	DefectId = defectId;
        }
    }
}

namespace Ycode.TestRailClient.V2
{
    /// <summary>
    /// Filter to narrow TestRail cases down to a group of a certain condition
    /// </summary>
    public interface ITestRailCaseFilter
    {
        /// <summary>
        /// It identifies how the residue cases should be handled.
        /// </summary>
        /// <remarks>
        /// If it is ExcludeFromRun, the residue cases are not included in the test run.
        /// If IncludeToRun, the residue cases are included in the test run together with filtered cases.
        /// In this case, the residue cases are returned by <see cref="TestRailClient.SplitCaseIdsByFilter"/> with isFilteredCases = false.
        /// </remarks>
    	ResidueCaseStrategy Strategy { get; }

        /// <summary>
        /// Validate this filter.
        /// </summary>
        /// <param name="cache">Cached TestRail data</param>
    	void Validate(ITestRailApiCache cache);

        /// <summary>
        /// Filter a TestRail case by a certain condition.
        /// </summary>
        /// <param name="case">TestRail case to be filtered</param>
        /// <param name="cache">Cached TestRail data</param>
        /// <returns>
        /// If it is true, the case will be included in filtered cases.
        /// If false, the case will be excluded and regarded as a residue.
        /// </returns>
    	bool Filter(TestRailCase @case, ITestRailApiCache cache);
    }

    public enum ResidueCaseStrategy
    {
    	IncludeToRun,
    	ExcludeFromRun,
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ycode.TestRailClient.V2
{
    public interface ITestRailClient
    {
        TestRailRun CurrentRun { get; }
        IReadOnlyDictionary<string, TestRailStatus> Statuses { get; }
        IReadOnlyDictionary<string, TestRailPriority> Priorities { get; }
        IReadOnlyDictionary<int, TestRailCase> Cases { get; }
        IReadOnlyList<TestRailCase> FilteredCases { get; }
        IReadOnlyList<TestRailCase> IncludedCases { get; }

        Task StartTestRunAsync(TestRailRunInfo runInfo, Action validate = null);
        Task EndTestRunAsync();
        Task CleanTestRunsAsync();
        IEnumerable<(bool isFilteredCases, int[] caseIds)> SplitCaseIdsByFilter(int[] caseIds);
        bool IsTestRequiredFor(params int[] caseIds);
        Task<List<TestRailResult>> RecordResultAsync(int[] caseIds, TestResult result);
    }

    public class TestRailClient : ITestRailClient
    {
        private TestRailClientConfiguration _config;
    	private ITestRailApiClient _apiClient;

    	public TestRailRun CurrentRun { get; private set; }
    	public IReadOnlyDictionary<string, TestRailStatus> Statuses
            => _apiClient.Statuses;
    	public IReadOnlyDictionary<string, TestRailPriority> Priorities
            => _apiClient.Priorities;
    	public IReadOnlyDictionary<int, TestRailCase> Cases
            => _apiClient.Cases;
        public IReadOnlyList<TestRailCase> FilteredCases { get; private set; }
            = new List<TestRailCase>().AsReadOnly();
    	public IReadOnlyList<TestRailCase> IncludedCases { get; private set; }
            = new List<TestRailCase>().AsReadOnly();

        public TestRailClient(TestRailClientConfiguration config, ITestRailApiClient apiClient)
        {
        	_config = config;
            _apiClient = apiClient;

        	if (!_config.Url.EndsWith("/"))
            {
            	_config.Url += "/";
            }
        }

        public TestRailClient(TestRailClientConfiguration config)
            : this(config, new TestRailApiClient(config)) { }

    	public async Task StartTestRunAsync(TestRailRunInfo runInfo, Action validate = null)
        {
        	if (_config.Disabled)
            {
            	return;
            }

        	if (CurrentRun != null)
            {
            	throw new TestRailClientException($"There's already a test run in progress. "
                    + "Please end it before starting another. Or, initialize and use another client instance.");
            }

        	await _apiClient.GetStatusesAsync();
        	await _apiClient.GetPrioritiesAsync();
        	var cases = await _apiClient.GetCasesAsync(runInfo.ProjectId, runInfo.SuiteId);

        	if (!_apiClient.Cases.Any())
            {
            	throw new TestRailClientException(
                    $"There's no test case in suite {runInfo.SuiteId} of project {runInfo.ProjectId}.");
            }

        	_config.CaseFilters.ForEach(f => f.Validate(_apiClient));
        	validate?.Invoke();

        	if (runInfo.CaseIds != null && runInfo.CaseIds.Length > 0)
            {
            	throw new TestRailClientException($"Provided TestRailRunInfo filters out all the test cases. "
                    + "Make sure to have one or more case IDs in it."
                    + "If you want to include all the cases, set null to CaseIds property.");
            }

        	var filtering = cases.Select(c => new FilteringResult
            {
            	Case = c,
            	isIncluded = runInfo.CaseIds?.Any(i => i == c.Id) ?? true,
            	isFiltered = true,
            });

        	foreach (var filter in _config.CaseFilters)
            {
            	filtering = filtering.Select(f =>
                {
                	if (f.isFiltered && !filter.Filter(f.Case, _apiClient))
                    {
                    	f.isFiltered = false;
                    	f.isIncluded = filter.Strategy switch
                        {
                        	ResidueCaseStrategy.ExcludeFromRun => false,
                        	ResidueCaseStrategy.IncludeToRun => f.isIncluded,
                        	_ => throw new NotImplementedException(),
                        };
                    }
                	return f;
                });
            }

        	IncludedCases = filtering.Where(f => f.isIncluded)
                .Select(f => f.Case).ToList().AsReadOnly();
        	FilteredCases = filtering.Where(f => f.isFiltered)
                .Select(f => f.Case).ToList().AsReadOnly();

        	runInfo.CaseIds = IncludedCases.Select(c => c.Id).ToArray();

        	CurrentRun = await _apiClient.AddRunAsync(runInfo);
        }

    	private class FilteringResult
        {
        	public TestRailCase Case { get; set; }
        	public bool isFiltered { get; set; }
        	public bool isIncluded { get; set; }
        }

    	public async Task EndTestRunAsync()
        {
        	if (_config.Disabled)
            {
            	return;
            }

        	if (CurrentRun == null)
            {
            	throw new TestRailClientException("Attempted to end a test run before TestRailClient starts one.");
            }

        	await _apiClient.CloseTestRunAsync(CurrentRun.Id);
        	CurrentRun = null;
        }

    	public async Task CleanTestRunsAsync()
        {
        	if (_config.Disabled)
            {
            	return;
            }

        	if (CurrentRun == null)
            {
            	throw new TestRailClientException("Please use CleanTestRunsAsync method after TestRailClient started a test run.");
            }

        	var testRuns = await _apiClient.GetRunsAsync(
            	CurrentRun.Id,
            	CurrentRun.SuiteId,
            	DateTime.UtcNow - TimeSpan.FromDays(30),
            	DateTime.UtcNow - TimeSpan.FromHours(1));

        	var tasks = new List<Task>();
        	foreach (var testRun in testRuns)
            {
            	if (!testRun.IsCompleted)
                {
                	tasks.Add(_apiClient.CloseTestRunAsync(testRun.Id));
                }
            }
        	Task.WaitAll(tasks.ToArray());
        }

    	public IEnumerable<(bool isFilteredCases, int[] caseIds)> SplitCaseIdsByFilter(int[] caseIds)
        {
        	if (CurrentRun == null)
            {
            	throw new TestRailClientException("Please use SplitCaseIdsByFilter method after TestRailClient started a test run.");
            }

        	if (!_config.Disabled)
            {
            	foreach (var group in caseIds
                    .Where(id => IncludedCases.Any(c => c.Id == id))
                    .GroupBy(id => FilteredCases.Any(f => f.Id == id)))
                {
                	var isFilteredCases = group.Key;
                	var ids = group.ToArray();

                	yield return (isFilteredCases, ids);
                }
            }
        }

    	public bool IsTestRequiredFor(params int[] caseIds)
        {
        	if (CurrentRun == null)
            {
            	throw new TestRailClientException("Please use ContainsFiltered method after TestRailClient started a test run.");
            }

        	return caseIds.Any(id => FilteredCases.Any(f => f.Id == id));
        }

    	public async Task<List<TestRailResult>> RecordResultAsync(int[] caseIds, TestResult result)
        {
        	var results = new List<TestRailResult>();

        	if (_config.Disabled || caseIds == null)
            {
            	return results;
            }

        	foreach (var caseId in caseIds)
            {
            	if (!IncludedCases.Any(c => c.Id == caseId))
                {
                	continue;
                }

            	results.Add(await _apiClient.AddResultForCaseAsync(
                	CurrentRun.Id,
                	caseId,
                	new TestRailResultInfo
                    {
                    	Status = _apiClient.Statuses[result.Status],
                        Version = result.Version,
                        Comment = result.Comment,
                        Defects = result.Defects.DefaultIfEmpty().Aggregate((a, b) => $"{a},{b}"),
                        Elapsed = result.Elapsed,
                    }));
            }
        	return results;
        }
    }
}

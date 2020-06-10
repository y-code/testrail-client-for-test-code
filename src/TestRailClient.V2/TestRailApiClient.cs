using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ycode.TestRailClient.V2
{
    public interface ITestRailApiClient : ITestRailApiCache
    {
        Task<TestRailCase[]> GetCasesAsync(int projectId, int suiteId);
        Task<TestRailStatus[]> GetStatusesAsync();
        Task<TestRailPriority[]> GetPrioritiesAsync();
        Task<TestRailRun> AddRunAsync(TestRailRunInfo data);
        Task<TestRailRun[]> GetRunsAsync(int projectId, int suiteId, DateTime after, DateTime before);
        Task<TestRailRun> CloseTestRunAsync(int runId, JsonSerializerOptions jsonOptions = null);
        Task<TestRailResult> AddResultForCaseAsync(int runId, int caseId, TestRailResultInfo data);
    }

    public class TestRailApiClient : ITestRailApiClient
    {
        private TestRailApiClientConfiguration _config;

    	public IReadOnlyDictionary<string, TestRailStatus> Statuses { get; private set; }
            = new TestRailStatusDictionary();
    	public IReadOnlyDictionary<string, TestRailPriority> Priorities { get; private set; }
            = new TestRailPriorityDictionary();
    	public IReadOnlyDictionary<int, TestRailCase> Cases { get; private set; }
            = new ReadOnlyDictionary<int, TestRailCase>(new Dictionary<int, TestRailCase>());

    	private string Credential
            => "Basic " + Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes($"{_config.UserName}:{ _config.AppKey}"));

    	public TestRailApiClient(TestRailApiClientConfiguration config)
        {
        	_config = config;
        }

    	protected async Task<T> Get<T>(string route, JsonSerializerOptions jsonOptions = null)
        {
        	using (var webClient = new WebClient()
            {
            	Headers = {
                    { HttpRequestHeader.Authorization, Credential },
                    { HttpRequestHeader.ContentType, "application/json" },
                },
            })
        	using (var stream = await webClient.OpenReadTaskAsync(_config.Url + "index.php?/api/v2/" + route))
            {
            	return await JsonSerializer.DeserializeAsync<T>(stream, jsonOptions);
            }
        }

    	protected async Task<T> Post<T, U>(string route, U data, JsonSerializerOptions jsonOptions = null)
        {
        	using (var webClient = new WebClient()
            {
            	Headers = {
                    { HttpRequestHeader.Authorization, Credential },
                    { HttpRequestHeader.ContentType, "application/json" },
                },
            })
            {
            	var response = await webClient.UploadStringTaskAsync(_config.Url + "index.php?/api/v2/" + route,
                	JsonSerializer.Serialize(data, jsonOptions));
            	return JsonSerializer.Deserialize<T>(response, jsonOptions);
            }
        }

    	public async Task<TestRailCase[]> GetCasesAsync(int projectId, int suiteId)
        {
        	var cases = await Get<TestRailCase[]>(
                $"get_cases/{projectId}&suite_id={suiteId}",
            	new JsonSerializerOptions
                {
                    Converters =
                    {
                        new TestRailPriorityConverter(
                            ((TestRailPriorityDictionary)Priorities).ToDictionaryById())
                    }
                });
        	Cases = new ReadOnlyDictionary<int, TestRailCase>(
            	cases.ToDictionary(c => c.Id));
        	return cases;
        }

    	public async Task<TestRailStatus[]> GetStatusesAsync()
        {
        	var statuses = await Get<TestRailStatus[]>("get_statuses");
        	Statuses = new TestRailStatusDictionary(statuses);
        	return statuses;
        }

    	public async Task<TestRailPriority[]> GetPrioritiesAsync()
        {
        	var priorities = await Get<TestRailPriority[]>("get_priorities");
        	Priorities = new TestRailPriorityDictionary(priorities);
        	return priorities;
        }

    	public Task<TestRailRun> AddRunAsync(TestRailRunInfo data)
            => Post<TestRailRun, TestRailRunInfo>("add_run/" + data.ProjectId, data);

        /// <summary>
        /// This method requests a list of test runs.
        /// </summary>
        /// <param name="after">Time in UTC. This method requests only test runs created after this time.</param>
        /// <param name="before">Time in UTC. This method requests only test runs created before this time</param>
        /// <returns></returns>
    	public Task<TestRailRun[]> GetRunsAsync(int projectId, int suiteId, DateTime after, DateTime before)
            => Get<TestRailRun[]>(
                $"get_runs/{projectId}"
                + $"&suite_id={suiteId}"
                + $"&created_after={(int)(after - new DateTime(1970, 1, 1)).TotalSeconds}"
                + $"&created_before={(int)(before - new DateTime(1970, 1, 1)).TotalSeconds}");

    	public Task<TestRailRun> CloseTestRunAsync(int runId, JsonSerializerOptions jsonOptions = null)
            => Post<TestRailRun, object>("close_run/" + runId, null, jsonOptions);

    	public Task<TestRailResult> AddResultForCaseAsync(int runId, int caseId, TestRailResultInfo data)
            => Post<TestRailResult, TestRailResultInfo>(
                $"add_result_for_case/{runId}/{caseId}",
            	data,
            	new JsonSerializerOptions
                {
                    Converters =
                    {
                        new TestRailStatusConverter(
                            ((TestRailStatusDictionary)Statuses).ToDictionaryById())
                    }
                });
    }
}

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
        private IWebClientFactory _webClientFactory;

        private Lazy<TestRailStatusDictionary> _statuses;
        public IReadOnlyDictionary<int, TestRailStatus> StatusesById
            => _statuses.Value.ById;
        public IReadOnlyDictionary<string, TestRailStatus> Statuses
            => _statuses.Value.ByName;

        private Lazy<TestRailPriorityDictionary> _priorities;
        public IReadOnlyDictionary<int, TestRailPriority> PrioritiesById
            => _priorities.Value.ById;
        public IReadOnlyDictionary<string, TestRailPriority> Priorities
            => _priorities.Value.ByName;

        public IReadOnlyDictionary<int, TestRailCase> Cases { get; private set; }
            = new ReadOnlyDictionary<int, TestRailCase>(new Dictionary<int, TestRailCase>());

    	private string Credential
            => "Basic " + Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes($"{_config.UserName}:{ _config.AppKey}"));

        public TestRailApiClient(TestRailApiClientConfiguration config) : this(config, null) { }

        internal TestRailApiClient(TestRailApiClientConfiguration config, IWebClientFactory webClientFactory = null)
        {
        	_config = config;
            _webClientFactory = webClientFactory ?? new WebClientFactory();

            if (string.IsNullOrEmpty(config.Url))
            {
                throw new TestRailClientException("Url is not provided in TestRail client configuration.");
            }

            if (string.IsNullOrEmpty(config.UserName))
            {
                throw new TestRailClientException("UserName is not provided in TestRail client configuration.");
            }

            if (string.IsNullOrEmpty(config.AppKey))
            {
                throw new TestRailClientException("AppKey is not provided in TestRail client configuration.");
            }

            if (config.Url.Last() != '/')
            {
                config.Url += "/";
            }

            _statuses = new Lazy<TestRailStatusDictionary>(
                () => InnerGetStatusesAsync().Result);
            _priorities = new Lazy<TestRailPriorityDictionary>(
                () => InnerGetPrioritiesAsync().Result);
        }

    	protected async Task<T> Get<T>(string route, JsonSerializerOptions jsonOptions = null)
        {
            using (var webClient = _webClientFactory.Create(
                headers: new WebHeaderCollection {
                    { HttpRequestHeader.Authorization, Credential },
                    { HttpRequestHeader.ContentType, "application/json" },
                }))
            using (var stream = await webClient.OpenReadTaskAsync(_config.Url + "index.php?/api/v2/" + route))
            {
                return await JsonSerializer.DeserializeAsync<T>(stream, jsonOptions);
            }
        }

    	protected async Task<T> Post<T, U>(string route, U data, JsonSerializerOptions jsonOptions = null)
        {
            using (var webClient = _webClientFactory.Create(
                headers: new WebHeaderCollection {
                    { HttpRequestHeader.Authorization, Credential },
                    { HttpRequestHeader.ContentType, "application/json" },
                }))
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
                    Converters = { new TestRailPriorityConverter(PrioritiesById) }
                });
        	Cases = new ReadOnlyDictionary<int, TestRailCase>(
            	cases.ToDictionary(c => c.Id));
        	return cases;
        }

    	public async Task<TestRailStatus[]> GetStatusesAsync()
        {
            var statuses = await InnerGetStatusesAsync();
        	return statuses.Values;
        }

        private async Task<TestRailStatusDictionary> InnerGetStatusesAsync()
        {
            var statuses = await Get<TestRailStatus[]>("get_statuses");
            return new TestRailStatusDictionary(statuses);
        }

        public async Task<TestRailPriority[]> GetPrioritiesAsync()
        {
            var priorities = await InnerGetPrioritiesAsync();
            return priorities.Values;
        }

        private async Task<TestRailPriorityDictionary> InnerGetPrioritiesAsync()
        {
            var priorities = await Get<TestRailPriority[]>("get_priorities");
            return new TestRailPriorityDictionary(priorities);
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

    	public async Task<TestRailResult> AddResultForCaseAsync(int runId, int caseId, TestRailResultInfo data)
        {
            return await Post<TestRailResult, TestRailResultInfo>(
                $"add_result_for_case/{runId}/{caseId}",
                data,
                new JsonSerializerOptions
                {
                    Converters = { new TestRailStatusConverter(StatusesById) }
                });
        }
    }
}

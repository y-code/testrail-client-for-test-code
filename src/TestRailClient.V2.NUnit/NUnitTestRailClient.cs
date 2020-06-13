using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Ycode.TestRailClient.V2.NUnit
{
	public class NUnitTestRailClient
    {
        private NUnitTestRailClientConfiguration _config;
        private ITestRailClient _client;

    	private ConcurrentDictionary<string, Stopwatch> _stopwatches = new ConcurrentDictionary<string, Stopwatch>();

    	public TestRailRun CurrentRun => _client.CurrentRun;

    	public NUnitTestRailClient(NUnitTestRailClientConfiguration config, ITestRailClient client)
        {
            _config = config;
        	_client = client;

        	_config.StatusMapping ??= new NUnitTestRailStatusMapping();
        }

        public NUnitTestRailClient(NUnitTestRailClientConfiguration config)
            : this(config, new TestRailClient(config)) { }

        protected (int[] caseIds, string[] defects) ReadAttributesOnTest(TestContext context)
        {
        	var caseIds = context.Test.Properties[TestRailCaseAttribute.Name]
                .Cast<int>().ToArray();
        	var defects = context.Test.Properties[DefectAttribute.Name]
                .Cast<string>().ToArray();

        	var testClassType = AppDomain.CurrentDomain.GetAssemblies()
                .Select(a => a.GetType(context.Test.ClassName))
                .Where(t => t != null).FirstOrDefault();
        	var testCase = testClassType
                .GetMethod(context.Test.MethodName)
                .GetCustomAttributes(typeof(TRTestCaseAttribute), true)
                .Cast<TRTestCaseAttribute>()
                .Where(c => {
                	var args1 = c.Arguments;
                	var args2 = context.Test.Arguments;
                	if (args1.Count() == args2.Count())
                    {
                    	for (var i = 0; i < c.Arguments.Count(); i++)
                        {
                        	if (!args1[i].Equals(args2[i]))
                            {
                            	return false;
                            }
                        }
                    }
                	else
                    {
                    	return false;
                    }
                	return true;
                })
                .FirstOrDefault();

        	if (testCase != null)
            {
            	var caseIdList = new List<int>();
            	if (testCase.IsCasePopulated)
                {
                	caseIdList.Add(testCase.Case);
                }
            	if (testCase.Cases != null)
                {
                	caseIdList.AddRange(testCase.Cases);
                }
            	caseIds = caseIdList.ToArray();

            	var defectList = new List<string>();
            	if (testCase.Defect != null)
                {
                	defectList.Add(testCase.Defect);
                }
            	if (testCase.Defects != null)
                {
                	defectList.AddRange(testCase.Defects);
                }
            	defects = defectList.ToArray();
            }

        	return (caseIds, defects);
        }

    	public Task StartTestRunAsync(TestRailRunInfo runInfo)
            => _client.StartTestRunAsync(runInfo, () => {
            	var invalidStatuses = new[] {
                	_config.StatusMapping.Passed,
                	_config.StatusMapping.Failed,
                	_config.StatusMapping.Skipped,
                	_config.StatusMapping.Inconclusive,
                	_config.StatusMapping.Warning,
                	_config.StatusMapping.WithDefect,
                	_config.StatusMapping.FilteringResidue,
                }.Where(s => !_client.Statuses.Keys.Contains(s));
            	if (invalidStatuses.Count() > 0)
                {
                	throw new Exception("Invalid TestRail statuses were specified in NUnitTestRailClient's status mapping: "
                        + invalidStatuses.Aggregate((a, b) => $"{a}, {b}"));
                }
            });

    	public async Task EndTestRunAsync()
        {
        	await _client.CleanTestRunsAsync();
        	await _client.EndTestRunAsync();
        }

    	public void SetUpSingleTest()
        {
        	var (caseIds, defects) = ReadAttributesOnTest(TestContext.CurrentContext);

        	var hasFilteredCase = _client.IsTestRequiredFor(caseIds);

        	var stopwatch = _stopwatches.GetOrAdd(TestContext.CurrentContext.Test.ID, k => new Stopwatch());
        	stopwatch.Start();

        	if (!hasFilteredCase)
            {
            	Assert.Ignore();
            }
        }

    	public async Task TearDownSingleTest(string version = null, string comment = null)
        {
        	if (_stopwatches.TryGetValue(TestContext.CurrentContext.Test.ID, out var stopwatch))
            {
            	stopwatch.Stop();
            }

        	await RecordResultAsync(
            	TestContext.CurrentContext,
            	version: version,
            	comment: comment,
            	elapsed: stopwatch?.Elapsed);
        }

    	protected async Task<List<TestRailResult>> RecordResultAsync(TestContext context, string version = null, string comment = null, TimeSpan? elapsed = null)
        {
        	var (caseIds, defects) = ReadAttributesOnTest(context);

        	string defectsFixedComment = null;
        	if (defects.Length > 0 && context.Result.Outcome.Status == TestStatus.Passed)
            {
            	defectsFixedComment = "**This test passed**, although there's known defects related to this test. The defects could be already fixed.";
            }

        	string notPassedComment = context.Result.Outcome.Status switch
            {
            	TestStatus.Passed => null,
            	TestStatus.Skipped => null,
            	_ => context.Result.Assertions.Select(a => $"{a.Message}\n\n{a.StackTrace}").LastOrDefault()
            };

        	string status = defects.Count() > 0
                ? _config.StatusMapping.WithDefect
                : context.Result.Outcome.Status switch
                {
                	TestStatus.Passed => _config.StatusMapping.Passed,
                	TestStatus.Failed => _config.StatusMapping.Failed,
                	TestStatus.Skipped => _config.StatusMapping.Skipped,
                	TestStatus.Inconclusive => _config.StatusMapping.Inconclusive,
                	TestStatus.Warning => _config.StatusMapping.Warning,
                	_ => _config.StatusMapping.Inconclusive,
                };

        	var results = new List<TestRailResult>();

        	foreach (var (isFilteredCases, ids) in _client.SplitCaseIdsByFilter(caseIds))
            {
            	var partial = await _client.RecordResultAsync(
                	ids,
                	new TestResult
                    {
                    	Version = version,
                    	Status = isFilteredCases ? status : _config.StatusMapping.FilteringResidue,
                    	Defects = defects,
                    	Comment = new[] { defectsFixedComment, comment, notPassedComment }
                            .Where(c => !string.IsNullOrEmpty(c))
                            .DefaultIfEmpty().Aggregate((a, b) => $"{a}\n\n{b}"),
                    	Elapsed = elapsed ?? TimeSpan.Zero,
                    }
                );
            	results.AddRange(partial);
            }

        	return results;
        }
    }
}

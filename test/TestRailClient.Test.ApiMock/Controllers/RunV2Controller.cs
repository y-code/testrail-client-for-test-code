using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;
using Ycode.TestRailClient.Test.ApiMock.Models;

namespace Ycode.TestRailClient.Test.ApiMock.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}")]
    [OpenApiTag("API: Statuses")]
    public partial class RunV2Controller : ControllerBase
    {
        ILogger _logger;

        private RunV2[] _data = new[]
        {
            new RunV2 // represents an old test run
            {
                Id = 10101,
                ProjectId = 10000,
                SuiteId = 100,
                PlanId = 1,
                MilestoneId = 1,
                Name = "[10100] Sample Test Run",
                Description = "This is a test run sample.",
                Refs = null,
                BlockedCount = 1,
                CustomStatusCount = 1,
                FailedCount = 1,
                PassedCount = 1,
                RetestCount = 1,
                UntestedCount = 1,
            },
            new RunV2 // represents an old test run
            {
                Id = 10102,
                ProjectId = 10000,
                SuiteId = 100,
                PlanId = 1,
                MilestoneId = 1,
                Name = "[10100] Sample Test Run",
                Description = "This is a test run sample.",
                Refs = null,
                BlockedCount = 1,
                CustomStatusCount = 1,
                FailedCount = 1,
                PassedCount = 1,
                RetestCount = 1,
                UntestedCount = 1,
            },
            new RunV2
            {
                Id = 10103,
                ProjectId = 10000,
                SuiteId = 100,
                PlanId = 1,
                MilestoneId = 1,
                Name = "[10100] Sample Test Run",
                Description = "This is a test run sample.",
                Refs = null,
                BlockedCount = 1,
                CustomStatusCount = 1,
                FailedCount = 1,
                PassedCount = 1,
                RetestCount = 1,
                UntestedCount = 1,
            },
        };

        public RunV2Controller(ILogger<RunV2Controller> logger)
        {
            _logger = logger;
        }

        [HttpGet("get_runs/{projectId}")]
        public IEnumerable<RunV2> GetRun(
            [FromRoute] int projectId,
            [FromQuery(Name = "suite_id")] int suiteId,
            [FromQuery(Name = "created_after")] int createdAfter,
            [FromQuery(Name = "created_before")] int createdBefore)
        {
            return _data.Where(d => d.Id == 10101 || d.Id == 10102);
        }

        [HttpPost("add_run/{projectId}")]
        public RunV2 AddRun(
            [FromRoute] int projectId,
            [FromBody] RunInfoV2 runInfo)
        {
            return _data.Single(d =>
                d.ProjectId == projectId
                && d.SuiteId == runInfo.SuiteId
                && d.Id == 10103);
        }

        [HttpPost("close_run/{run_id}")]
        public RunV2 CloseRun([FromRoute] int run_id)
        {
            return _data.Single(d => d.Id == run_id);
        }
    }
}

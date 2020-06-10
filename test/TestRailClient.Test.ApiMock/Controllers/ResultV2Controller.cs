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
    public partial class ResultV2Controller : ControllerBase
    {
        ILogger _logger;
        AccessLogV2 _accessLog;

        private int counter = 100000;

        public ResultV2Controller(ILogger<ResultV2Controller> logger, AccessLogV2 accessLog)
        {
            _logger = logger;
            _accessLog = accessLog;
        }

        [HttpPost("add_result_for_case/{runId}/{caseId}")]
        public ResultV2 AddResultForCase(
            [FromRoute] int runId,
            [FromRoute] int caseId,
            [FromBody] ResultInfoV2 data)
        {
            _accessLog.Add(new AddResultForCaseV2AccessLog(runId, caseId, data));

            return new ResultV2
            {
                Id = counter++,
                TestId = caseId,
                Comment = data.Comment,
                Status = data.Status,
                Version = data.Version,
                Defects = data.Defects,
                Elapsed = data.Elapsed,
                AssignedToId = data.AssignedToId,
            };
        }
    }
}

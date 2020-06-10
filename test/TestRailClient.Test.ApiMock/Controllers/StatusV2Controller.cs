using System.Collections.Generic;
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
    public partial class StatusV2Controller : ControllerBase
    {
        ILogger _logger;

        public StatusV2Controller(ILogger<StatusV2Controller> logger)
        {
            _logger = logger;
        }

        [HttpGet("get_statuses")]
        public IEnumerable<StatusV2> GetStatuses()
        {
            return new[]
            {
                new StatusV2
                {
                    Id = 1,
                    Name = "passed",
                    Label = "Passed",
                    IsFinal = true,
                    IsSystem = true,
                    IsUntested = false,
                },
                new StatusV2
                {
                    Id = 5,
                    Name = "failed",
                    Label = "Failed",
                    IsFinal = true,
                    IsSystem = true,
                    IsUntested = false,
                },
                new StatusV2
                {
                    Id = 2,
                    Name = "blocked",
                    Label = "Blocked",
                    IsFinal = true,
                    IsSystem = true,
                    IsUntested = false,
                },
                new StatusV2
                {
                    Id = 4,
                    Name = "retest",
                    Label = "Retest",
                    IsFinal = true,
                    IsSystem = true,
                    IsUntested = false,
                },
                new StatusV2
                {
                    Id = 101,
                    Name = "custom_status_1",
                    Label = "Known Issue",
                    IsFinal = true,
                    IsSystem = true,
                    IsUntested = false,
                },
                new StatusV2
                {
                    Id = 102,
                    Name = "custom_status_2",
                    Label = "Caution",
                    IsFinal = true,
                    IsSystem = true,
                    IsUntested = false,
                },
                new StatusV2
                {
                    Id = 103,
                    Name = "custom_status_3",
                    Label = "Excluded",
                    IsFinal = true,
                    IsSystem = true,
                    IsUntested = true,
                },
            };
        }
    }
}

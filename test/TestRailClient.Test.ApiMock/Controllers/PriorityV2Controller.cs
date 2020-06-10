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
    public partial class PriorityV2Controller : ControllerBase
    {
        ILogger _logger;

        public PriorityV2Controller(ILogger<PriorityV2Controller> logger)
        {
            _logger = logger;
        }

        [HttpGet("get_priorities")]
        public IEnumerable<PriorityV2> GetPriorities()
        {
            return new[]
            {
                new PriorityV2
                {
                    Id = 1,
                    Name = "P1 - Don't Test",
                    ShortName = "P1",
                    Priority = 1,
                    IsDefault = false,
                },
                new PriorityV2
                {
                    Id = 2,
                    Name = "P2 - Test If Time",
                    ShortName = "P2",
                    Priority = 2,
                    IsDefault = false,
                },
                new PriorityV2
                {
                    Id = 3,
                    Name = "P3 - Test If Time",
                    ShortName = "P3",
                    Priority = 3,
                    IsDefault = false,
                },
                new PriorityV2
                {
                    Id = 4,
                    Name = "P4 - Must Test",
                    ShortName = "P4",
                    Priority = 4,
                    IsDefault = false,
                },
                new PriorityV2
                {
                    Id = 5,
                    Name = "P5 - Must Test",
                    ShortName = "P5",
                    Priority = 5,
                    IsDefault = false,
                },
            };
        }
    }
}

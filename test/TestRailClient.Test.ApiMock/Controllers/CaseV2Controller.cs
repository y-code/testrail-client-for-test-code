using System;
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
    public partial class CaseV2Controller : ControllerBase
    {
        ILogger _logger;

        public CaseV2Controller(ILogger<CaseV2Controller> logger)
        {
            _logger = logger;
        }

        [HttpGet("get_cases/{projectId}")]
        public IEnumerable<CaseV2> GetCases(
            [FromRoute] int projectId,
            [FromQuery(Name = "suite_id")] int suiteId)
        {
            return projectId switch
            {
                10000 => suiteId switch
                {
                    // This test suite 100 is used by Sample1 test project.
                    // Change in these cases below may cause breaking existing tests.
                    100 => new[]
                    {
                        new CaseV2
                        {
                            Id = 10101,
                            SuiteId = 100,
                            SectionId = 1,
                            TemplateId = 1,
                            Title = "Test 1",
                            PriorityId = 1,
                            TypeId = 1,
                            Refs = "ASD-123,ASD-456", // <- this should never be recognized as defects!
                        },
                        new CaseV2
                        {
                            Id = 10102,
                            SuiteId = 100,
                            SectionId = 1,
                            TemplateId = 2,
                            Title = "Test 2",
                            PriorityId = 2,
                            TypeId = 2,
                            Refs = null,
                        },
                        new CaseV2
                        {
                            Id = 10103,
                            SuiteId = 100,
                            SectionId = 2,
                            TemplateId = 3,
                            Title = "Test 3",
                            PriorityId = 3,
                            TypeId = 3,
                            Refs = null,
                        },
                        new CaseV2
                        {
                            Id = 10104,
                            SuiteId = 100,
                            SectionId = 2,
                            TemplateId = 4,
                            Title = "Test 4",
                            PriorityId = 4,
                            TypeId = 1,
                            Refs = null,
                        },
                        new CaseV2
                        {
                            Id = 10105,
                            SuiteId = 100,
                            SectionId = 3,
                            TemplateId = 1,
                            Title = "Test 5",
                            PriorityId = 1,
                            TypeId = 2,
                            Refs = null,
                        },
                        new CaseV2
                        {
                            Id = 10106,
                            SuiteId = 100,
                            SectionId = 3,
                            TemplateId = 2,
                            Title = "Test 6",
                            PriorityId = 1,
                            TypeId = 3,
                            Refs = null,
                        },
                        new CaseV2
                        {
                            Id = 10107,
                            SuiteId = 100,
                            SectionId = 4,
                            TemplateId = 3,
                            Title = "Test 7",
                            PriorityId = 5,
                            TypeId = 1,
                            Refs = null,
                        },
                        new CaseV2
                        {
                            Id = 10108,
                            SuiteId = 100,
                            SectionId = 4,
                            TemplateId = 1,
                            Title = "Test 8",
                            PriorityId = 5,
                            TypeId = 2,
                            Refs = null,
                        },
                        new CaseV2
                        {
                            Id = 10109,
                            SuiteId = 100,
                            SectionId = 4,
                            TemplateId = 2,
                            Title = "Test 9",
                            PriorityId = 5,
                            TypeId = 1,
                            Refs = null,
                        },
                        new CaseV2
                        {
                            Id = 10110,
                            SuiteId = 100,
                            SectionId = 4,
                            TemplateId = 3,
                            Title = "Test 10",
                            PriorityId = 5,
                            TypeId = 2,
                            Refs = null,
                        },
                        new CaseV2
                        {
                            Id = 10111,
                            SuiteId = 100,
                            SectionId = 4,
                            TemplateId = 1,
                            Title = "Test 11",
                            PriorityId = 5,
                            TypeId = 1,
                            Refs = null,
                        },
                        new CaseV2
                        {
                            Id = 10112,
                            SuiteId = 100,
                            SectionId = 5,
                            TemplateId = 1,
                            Title = "Test 12",
                            PriorityId = 5,
                            TypeId = 1,
                            Refs = null,
                        },
                        new CaseV2
                        {
                            Id = 10113,
                            SuiteId = 100,
                            SectionId = 5,
                            TemplateId = 1,
                            Title = "Test 13",
                            PriorityId = 5,
                            TypeId = 1,
                            Refs = null,
                        },
                        new CaseV2
                        {
                            Id = 10114,
                            SuiteId = 100,
                            SectionId = 5,
                            TemplateId = 1,
                            Title = "Test 14",
                            PriorityId = 5,
                            TypeId = 1,
                            Refs = null,
                        },
                    },
                    200 => new[]
                    {
                        new CaseV2
                        {
                            Id = 10201,
                            SuiteId = 200,
                            SectionId = 1,
                            TemplateId = 1,
                            Title = "Test 1",
                            PriorityId = 1,
                            TypeId = 1,
                            Refs = null,
                        },
                        new CaseV2
                        {
                            Id = 10202,
                            SuiteId = 200,
                            SectionId = 1,
                            TemplateId = 2,
                            Title = "Test 2",
                            PriorityId = 2,
                            TypeId = 2,
                            Refs = null,
                        },
                        new CaseV2
                        {
                            Id = 10203,
                            SuiteId = 200,
                            SectionId = 2,
                            TemplateId = 3,
                            Title = "Test 3",
                            PriorityId = 3,
                            TypeId = 3,
                            Refs = null,
                        },
                        new CaseV2
                        {
                            Id = 10204,
                            SuiteId = 200,
                            SectionId = 2,
                            TemplateId = 1,
                            Title = "Test 4",
                            PriorityId = 4,
                            TypeId = 4,
                            Refs = null,
                        },
                    },
                    _ => Array.Empty<CaseV2>(),
                    },
                _ => Array.Empty<CaseV2>(),
            };
        }
    }
}

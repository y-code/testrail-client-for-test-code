using System;
namespace Ycode.TestRailClient.Test.ApiMock.Models
{
    public class AddResultForCaseV2AccessLog
    {
        public int RunId { get; }
        public int CaseId { get; }
        public ResultInfoV2 Data { get; }

        public AddResultForCaseV2AccessLog(int runId, int caseId, ResultInfoV2 data)
        {
            RunId = runId;
            CaseId = caseId;
            Data = data;
        }
    }
}

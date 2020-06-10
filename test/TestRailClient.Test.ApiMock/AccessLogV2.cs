using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Ycode.TestRailClient.Test.ApiMock.Models;

namespace Ycode.TestRailClient.Test.ApiMock
{
    public interface IAccessLogV2
    {
        IReadOnlyList<AddResultForCaseV2AccessLog> AddResultForCase { get; }
    }

    public class AccessLogV2 : IAccessLogV2
    {
        ConcurrentQueue<AddResultForCaseV2AccessLog> _addResultForCase = new ConcurrentQueue<AddResultForCaseV2AccessLog>();
        public IReadOnlyList<AddResultForCaseV2AccessLog> AddResultForCase => _addResultForCase.ToList().AsReadOnly();

        public AccessLogV2()
        {
        }

        public void Add(AddResultForCaseV2AccessLog result)
        {
            _addResultForCase.Enqueue(result);
        }
    }
}

using System;
namespace Ycode.TestRailClient.V2
{
    public class TestRailClientException : Exception
    {
        public TestRailClientException(string message) : base(message) { }
    }
}

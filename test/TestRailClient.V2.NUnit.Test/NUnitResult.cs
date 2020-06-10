using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Ycode.TestRailClient.V2.NUnit.Test
{
    public class NUnitResult
    {
        public string StdOut { get; }
        public string ErrOut { get; }
        public bool Succeeded { get; } = false;
        public int TotalTests { get; }
        public int Passed { get; }
        public int Failed { get; }
        public int Skipped { get; }
        public int TotalMilliseconds { get; }

        public NUnitResult(string stdOut, string errOut)
        {
            StdOut = stdOut;
            ErrOut = errOut;

            var lines = StdOut.Split('\n', '\r').Where(s => !string.IsNullOrEmpty(s));

            Succeeded = lines.Any(s => s == "Test Run Successful.");

            var summary = new Regex(@"Total tests: (?<total>[0-9]+)([\r\n]{1,2}\s*Passed: (?<passed>[0-9]+))?([\r\n]{1,2}\s*Failed: (?<failed>[0-9]+))?([\r\n]{1,2}\s*Skipped: (?<skipped>[0-9]+))?[\r\n]{1,2}\s*Total time: (?<time>[0-9\.]+)\s+Seconds")
                .Match(StdOut);

            int totalTests = int.TryParse(summary.Groups["total"].Value, out totalTests) ? totalTests : 0;
            int passed = int.TryParse(summary.Groups["passed"].Value, out passed) ? passed : 0;
            int failed = int.TryParse(summary.Groups["failed"].Value, out failed) ? failed : 0;
            int skipped = int.TryParse(summary.Groups["skipped"].Value, out skipped) ? skipped : 0;
            double totalTime = double.TryParse(summary.Groups["time"].Value, out totalTime) ? totalTime : 0;

            TotalTests = totalTests;
            Passed = passed;
            Failed = failed;
            Skipped = skipped;
            TotalMilliseconds = Convert.ToInt32(totalTime * 1000d);
        }

        public override string ToString()
            => $"{StdOut}\n\n{ErrOut}";
    }
}

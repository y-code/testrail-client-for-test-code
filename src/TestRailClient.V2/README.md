# TestRail Client for Test Code

Ycode.TestRailClient.V2 provides a TestRail API v2 client dedicated to the usage in test code.

This library implements functionalities typically required to integrate your test code with TestRail. For this sake, it never tries to cover all the TestRail API v2.

# Quick Start

With the code sample below, you start a test run and then close it after recording a test result.

    const int projectId = 10000;
    const int suiteId = 100;

    var caseIds = new[] { 789, 790, 791 };
    var version = "v2.3.1";

    var client = new TestRailClient(
        new TestRailClientConfiguration
        {
            Url = "https://localhost:62182",
            UserName = "taro.yamada",
            AppKey = "aiueo-kakikukeko-sasisuseso",
        });

    await client.StartTestRunAsync(
        new TestRailRunInfo
        {
            ProjectId = projectId,
            SuiteId = suiteId,
            Name = "Test Run Sample",
        });

    try
    {
        // Execute a test
    }
    catch (Exception e)
    {
        await client.RecordResultAsync(
            caseIds,
            new TestResult
            {
                Status = "Failed",
                Version = version,
                Comment = e.Message,
            });
        throw;
    }

    await client.RecordResultAsync(
        caseIds,
        new TestResult
        {
            Status = "Passed",
            Version = version,
        });

    await client.EndTestRunAsync();

using System.Collections.Generic;
using NUnit.Framework;
using Ycode.TestRailClient.V2;
using Ycode.TestRailClient.V2.NUnit;

namespace Sample1
{
    [TestFixture]
    public class SampleTests : TestBase
    {
        /// <summary>
        /// Both case 10102 and 10104 are reported as failed.
        /// </summary>
        [Test]
        [TRTestCase(Cases = new[] { 10102, 10104 })]
        public void Test1()
        {
            Assert.Fail();
        }

        /// <summary>
        /// Both case 10103 is reported as passed, while case 10101 is reported as filtering residue.
        /// </summary>
        [Test]
        [TestRailCase(10101)]
        [TestRailCase(10103)]
        public void Test2()
        {
            Assert.Pass();
        }

        /// <summary>
        /// Test is skipped because all the cases are filtered out.
        /// </summary>
        [TRTestCase(Cases = new[] { 10105, 10106 })]
        public void Test3()
        {
            Assert.Pass();
        }

        /// <summary>
        /// Case 10107 is reported as passd, while case 10108 is reported with defects.
        /// </summary>
        [TRTestCase("param1-1", 1, Case = 10107)]
        [TRTestCase("param1-2", 2, Case = 10108, Defect = "ASD-123")]
        public void Test4(string param1, int param2)
        {
            Assert.Pass();
        }

        /// <summary>
        /// Case 10109, 10110 and 10111 are all reported with defects.
        /// </summary>
        [TRTestCase(1, "param2-1", Cases = new[] { 10109, 10110 }, Defect = "ASD-123")]
        [TRTestCase(2, "param2-2", Case = 10111, Defects = new[] { "ASD-123" })]
        public void Test5(int param1, string param2)
        {
            Assert.Fail();
        }

        [TestCaseSource(typeof(TestCaseSource), "TestCases")]
        public int Test6(int n, int d)
        {
            return n / d;
        }

        public class TestCaseSource
        {
            public static IEnumerable<TestCaseData> TestCases
            {
                get
                {
                    yield return new TestCaseData(12, 3).Returns(4) // to pass
                        .AddTestRailCase(10112);
                    yield return new TestCaseData(12, 2).Returns(5) // to fail
                        .AddTestRailCase(10113)
                        .AddTestRailCase(10114);
                }
            }
        }
    }
}
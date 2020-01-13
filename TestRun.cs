using System;
using System.Collections.Generic;
using System.Linq;

namespace DiskSpeedTest
{
    public class TestRun
    {
        public TestRun()
        {
            TestTargets = new List<TestTarget>();
            TestParameters = new List<TestParameter>();
        }

        public void AddTestTargets(List<string> fileNames)
        {
            if (fileNames == null)
                throw new ArgumentNullException(nameof(fileNames));

            TestTargets.AddRange(fileNames.Select(fileName => new TestTarget { FileName = fileName }));
        }

        public void AddTestBlockRange(int blockBegin, int blockEnd)
        {
            for (int blockSize = blockBegin; blockSize <= blockEnd; blockSize *= 2)
            {
                // 50% mix read and write, 100% read, 100% write
                TestParameters.Add(new TestParameter { BlockSize = blockSize, WriteRatio = 0 });
                TestParameters.Add(new TestParameter { BlockSize = blockSize, WriteRatio = 50 });
                TestParameters.Add(new TestParameter { BlockSize = blockSize, WriteRatio = 100 });
            }
        }

        public static bool RunTest(TestTarget testTarget, TestParameter testParameter, out TestResult testResult)
        {
            if (testTarget == null)
                throw new ArgumentNullException(nameof(testTarget));
            if (testParameter == null)
                throw new ArgumentNullException(nameof(testParameter));

            testResult = new TestResult();
            if (DiskSpeed.RunSpeedTest(testTarget, testParameter, out string xml) != 0)
                return false;

            // Parse results
            testResult.FromXml(xml);

            return true;
        }

        public List<TestTarget> TestTargets { get; }
        public List<TestParameter> TestParameters { get; }
    }
}

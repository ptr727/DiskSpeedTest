using InsaneGenius.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiskSpeedTest
{
    internal static class Program
    {
        private static int Main()
        {
            // Result file
            ResultsFile resultsFile = new ResultsFile(@"D:\SpeedTestResults.csv");
            ConsoleEx.WriteLine($"Writing results to : \"{resultsFile.FileName}\"");
            resultsFile.WriteHeader();

            // Test targets
            // Default 64GB size
            TestRun testRun = new TestRun();
            testRun.AddTestTargets( new List<string> {
                /*@"\\Server-1\CacheSpeedTest\SpeedTestDataFile.dat",
                @"\\Server-1\DiskSpeedTest\SpeedTestDataFile.dat",*/
                @"\\Server-2\CacheSpeedTest\SpeedTestDataFile.dat"/*,
                @"\\Server-2\DiskSpeedTest\SpeedTestDataFile.dat",
                @"\\WIN-EKJ8HU9E5QC\TestW2K19\SpeedTestDataFile.dat"*/ });

            // Block size from 4K to 2MB, 100% Read, 50% Read 50% Write, 100% Write
            testRun.AddTestBlockRange(4 * Format.KiB, 2 * Format.MiB);

            // Estimated time to complete
            int totalIterations = testRun.TestTargets.Count * testRun.TestParameters.Count;
            int remainingSeconds = testRun.TestParameters.Sum(parameter => parameter.WarmupTime + parameter.TestTime) * testRun.TestTargets.Count;
            ConsoleEx.WriteLine($"Running {totalIterations} iterations, {remainingSeconds} seconds, estimated to complete by {DateTime.Now + TimeSpan.FromSeconds(remainingSeconds)}");
            ConsoleEx.WriteLine("");

            // Run all tests
            int result = 0;
            int iteration = 0;
            foreach (TestTarget testTarget in testRun.TestTargets)
            {
                // Reuse the existing file, or create a new file file
                if (testTarget.DoesTargetExist())
                    ConsoleEx.WriteLine($"Using existing test file : {testTarget.FileName}");
                else 
                {
                    ConsoleEx.WriteLine($"Creating new test file : {testTarget.FileName}");
                    if (!testTarget.CreateTarget())
                    {
                        ConsoleEx.WriteLineError($"Failed to create test file : {testTarget.FileName}");
                        ConsoleEx.WriteLine("");

                        // Try the next target
                        result = -1;
                        continue;
                    }
                }
                ConsoleEx.WriteLine("");

                // Run all tests against target
                foreach (TestParameter testParameter in testRun.TestParameters)
                {
                    // Run speed test
                    iteration ++;
                    remainingSeconds -= testParameter.WarmupTime + testParameter.TestTime;
                    ConsoleEx.WriteLine($"Running test {iteration} of {totalIterations}, " +
                        $"iteration to complete by {DateTime.Now + TimeSpan.FromSeconds(testParameter.WarmupTime + testParameter.TestTime)}, " +
                        $"remaining tests to complete by {DateTime.Now + TimeSpan.FromSeconds(remainingSeconds + testParameter.WarmupTime + testParameter.TestTime)}");
                    if (!TestRun.RunTest(testTarget, testParameter, out TestResult testResult))
                    {
                        resultsFile.AddFailedResult(testTarget, testParameter);
                        ConsoleEx.WriteLineError("Failed to run test");
                        ConsoleEx.WriteLine("");

                        // Try the next test
                        result = -1;
                        continue;
                    }

                    // Report test results
                    resultsFile.AddResult(testTarget, testParameter, testResult);
                    ConsoleEx.WriteLine($"{testTarget.FileName} : {testResult.BytesPerSec / Format.MiB:n} MiB/s, {testResult.IosPerSec:n} IO/s");
                    ConsoleEx.WriteLine("");
                }
            }

            return result;
        }
    }
}

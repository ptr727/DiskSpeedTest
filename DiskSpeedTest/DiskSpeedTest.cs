using InsaneGenius.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiskSpeedTest
{
    internal class DiskSpeedTest
    {
        public DiskSpeedTest(DiskSpeedConfig config)
        {
            Config = config;
        }

        public int Run()
        {
            // Result file
            DiskSpeedResultFile resultFile = new DiskSpeedResultFile(Config.ResultFile);
            ConsoleEx.WriteLine($"Writing results to : \"{resultFile.FileName}\"");
            resultFile.WriteHeader();

            // Set the test config
            DiskSpeedRun testRun = new DiskSpeedRun();
            testRun.AddTestTargets(Config.Targets, Config.TargetSize);
            testRun.AddTestBlockRange(Config.BlockSizeBegin, Config.BlockSizeEnd, Config.WarmupTime, Config.TestTime);

            // Estimated time to complete
            int totalIterations = testRun.TestTargets.Count * testRun.TestParameters.Count;
            int remainingSeconds = testRun.TestParameters.Sum(parameter => parameter.WarmupTime + parameter.TestTime) * testRun.TestTargets.Count + (totalIterations - 1) * Config.RestTime;
            ConsoleEx.WriteLine($"Running {totalIterations} iterations, {remainingSeconds} seconds, estimated to complete by {DateTime.Now + TimeSpan.FromSeconds(remainingSeconds)}");
            ConsoleEx.WriteLine("");

            // Run all tests
            int result = 0;
            int iteration = 0;
            foreach (DiskSpeedTarget testTarget in testRun.TestTargets)
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
                foreach (DiskSpeedParameter testParameter in testRun.TestParameters)
                {
                    // Calculate test times
                    iteration++;
                    int thisTestTime = testParameter.WarmupTime + testParameter.TestTime + (iteration > 1 ? Config.RestTime : 0);
                    remainingSeconds -= thisTestTime;
                    ConsoleEx.WriteLine($"Running test {iteration} of {totalIterations}, " +
                        $"iteration to complete by {DateTime.Now + TimeSpan.FromSeconds(thisTestTime)}, " +
                        $"remaining tests to complete by {DateTime.Now + TimeSpan.FromSeconds(remainingSeconds + thisTestTime)}");

                    // Sleep between tests
                    // This may be required if the target file is in use after a test
                    if (Config.RestTime > 0 && iteration > 1)
                    {
                        // TODO : Add Ctrl-C handler so that we can break the test during wait
                        ConsoleEx.WriteLine($"Resting for {Config.RestTime} seconds...");
                        Task.Delay(Config.RestTime * 1000).Wait();
                    }

                    // Run test
                    if (!DiskSpeedRun.RunTest(testTarget, testParameter, out DiskSpeedResult testResult))
                    {
                        resultFile.AddFailedResult(testTarget, testParameter);
                        ConsoleEx.WriteLineError("Failed to run test");
                        ConsoleEx.WriteLine("");

                        // Try the next test
                        result = -1;
                        continue;
                    }

                    // Report test results
                    resultFile.AddResult(testTarget, testParameter, testResult);
                    ConsoleEx.WriteLine($"{testTarget.FileName} : {testResult.BytesPerSec / Format.MiB:n} MiB/s, {testResult.IosPerSec:n} IO/s");
                    ConsoleEx.WriteLine("");
                }
            }

            return result;
        }

        private readonly DiskSpeedConfig Config;
    }
}

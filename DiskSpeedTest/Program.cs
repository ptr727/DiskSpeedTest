using InsaneGenius.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DiskSpeedTest
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            // Load config from file
            if (args.Length != 1)
            {
                ConsoleEx.WriteLineError("Usage : DiskSpeedTest.exe [JSON config file]");
                return -1;
            }
            string configFile = args[0];
            ConsoleEx.WriteLine($"Loading config from : \"{configFile}\"");
            if (!File.Exists(configFile))
            {
                ConsoleEx.WriteLineError($"Config file not found : \"{configFile}\"");
                return -1;
            }
            Config config = Config.FromFile(configFile);
            if (config == null)
            {
                ConsoleEx.WriteLineError($"Unable to parse config file : \"{configFile}\"");
                return -1;
            }

            // Result file
            ResultsFile resultsFile = new ResultsFile(config.ResultsFile);
            ConsoleEx.WriteLine($"Writing results to : \"{resultsFile.FileName}\"");
            resultsFile.WriteHeader();

            // Set the test config
            TestRun testRun = new TestRun();
            testRun.AddTestTargets(config.TestTargets, config.TestTargetSize);
            testRun.AddTestBlockRange(config.BlockSizeBegin, config.BlockSizeEnd, config.WarmupTime, config.TestTime);

            // Estimated time to complete
            int totalIterations = testRun.TestTargets.Count * testRun.TestParameters.Count;
            int remainingSeconds = testRun.TestParameters.Sum(parameter => parameter.WarmupTime + parameter.TestTime) * testRun.TestTargets.Count + (totalIterations - 1) * config.RestTime;
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
                    // Calculate test times
                    iteration ++;
                    int thisTestTime = testParameter.WarmupTime + testParameter.TestTime + (iteration > 1 ? config.RestTime : 0);
                    remainingSeconds -= thisTestTime;
                    ConsoleEx.WriteLine($"Running test {iteration} of {totalIterations}, " +
                        $"iteration to complete by {DateTime.Now + TimeSpan.FromSeconds(thisTestTime)}, " +
                        $"remaining tests to complete by {DateTime.Now + TimeSpan.FromSeconds(remainingSeconds + thisTestTime)}");

                    // Sleep between tests
                    // This may be required if the target file is in use after a test
                    if (config.RestTime > 0 && iteration > 1)
                    {
                        // TODO : Add Ctrl-C handler so that we can break the test during wait
                        ConsoleEx.WriteLine($"Resting for {config.RestTime} seconds...");
                        Task.Delay(config.RestTime * 1000).Wait();
                    }

                    // Run test
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

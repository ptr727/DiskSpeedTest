using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DiskSpeedTest
{
    static class Program
    {
        static void Main(string[] _)
        {
            // Output file
            const string resultsFile = @"D:\SpeedTestResults.csv";

            // CSV header
            StringBuilder resultText = new StringBuilder();
            resultText.AppendLine("FileName, FileSize, BlockSize, WriteRatio, ThreadCount, OutstandingOperations, WarmupTime, TestTime, Bytes, IOS");

            // Test targets
            List<TestTarget> testTargets = new List<TestTarget>();
            // testTargets.Add(new TestTarget {FileName = @"D:\SpeedTestDataFile.dat"});
            // testTargets.Add(new TestTarget {FileName = @"\\Server-2\CacheSpeedTest\SpeedTestDataFile.dat"});
            testTargets.Add(new TestTarget { FileName = @"\\Server-2\DiskSpeedTest\SpeedTestDataFile.dat" });

            // Test parameters
            // Block size from 4K to 2MB
            List<TestParameter> testParameters = new List<TestParameter>();
            for (int blockSize = 4 * Format.KiB; blockSize <= 2 * Format.MiB; blockSize *= 2)
            {
                // 50% mix read and write, 100% read, 100% write
                testParameters.Add(new TestParameter { BlockSize = blockSize, WriteRatio = 0 });
                testParameters.Add(new TestParameter { BlockSize = blockSize, WriteRatio = 50 });
                testParameters.Add(new TestParameter { BlockSize = blockSize, WriteRatio = 100 });
            }

            // Estimated time to complete
            int totalIterations = testTargets.Count * testParameters.Count;
            int totalSeconds = testParameters.Sum(parameter => parameter.WarmupTime + parameter.TestTime) * testTargets.Count;
            Console.WriteLine($"Running {totalIterations} iterations, estimated time to complete {TimeSpan.FromSeconds(totalSeconds)} ({totalSeconds} seconds)");
            Console.WriteLine("");

            // Prepare all test targets
            int iteration = 0;
            foreach (TestTarget testTarget in testTargets)
            {
                // If the target exists and is the right size skip creating it again
                if (File.Exists(testTarget.FileName) &&
                    new FileInfo(testTarget.FileName).Length == testTarget.FileSize)
                {
                    Console.WriteLine($"Using existing test file : {testTarget.FileName}");
                }
                else 
                {
                    Console.WriteLine($"Creating new test file : {testTarget.FileName}");
                    DiskSpeed.CreateTestTarget(testTarget);
                }
                Console.WriteLine("");

                // Run all tests against target
                foreach (TestParameter testParameter in testParameters)
                {
                    // Run speed test
                    iteration ++;
                    Console.WriteLine($"Running test {iteration} of {totalIterations}");
                    string xml;
                    DiskSpeed.RunSpeedTest(testTarget, testParameter, out xml);

                    // Parse results
                    TestResult.Results results = TestResult.FromXml(xml);

                    // Calculated IO's and Bytes per second
                    long bytes = 0, ios = 0;
                    foreach (TestResult.Thread thread in results.TimeSpan.Thread)
                    {
                        bytes += Convert.ToInt64(thread.Target.BytesCount);
                        ios += Convert.ToInt64(thread.Target.IOCount);
                    }
                    double seconds = Convert.ToDouble(results.TimeSpan.TestTimeSeconds);
                    double bytesPerSec = bytes / seconds;
                    double iosPerSec = ios / seconds;

                    // Results
                    resultText.AppendLine($"\"{testTarget.FileName}\", {testTarget.FileSize}, {testParameter.BlockSize}, {testParameter.WriteRatio}, {testParameter.ThreadCount}, {testParameter.OutstandingOperations}, {testParameter.WarmupTime}, {seconds}, {bytes}, {ios}");
                    Console.WriteLine($"{testTarget.FileName} : {bytesPerSec / Format.MiB:n} MiB/s, {iosPerSec:n} IO/s");
                    Console.WriteLine("");
                }
            }

            // Write results to file
            File.WriteAllText(resultsFile, resultText.ToString());
            Console.WriteLine($"Writing test results to : {resultsFile}");
        }
    }
}

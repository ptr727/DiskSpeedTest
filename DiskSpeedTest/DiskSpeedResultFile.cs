using System;
using System.IO;

namespace DiskSpeedTest
{
    public class DiskSpeedResultFile
    {
        public DiskSpeedResultFile(string fileName)
        {
            FileName = fileName;
        }

        public void WriteHeader()
        {
            // Create or replace the contents of the file
            File.WriteAllText(FileName, Header + Environment.NewLine);
        }

        public void AddResult(DiskSpeedTarget testTarget, DiskSpeedParameter testParameter, DiskSpeedResult testResult)
        {
            if (testTarget == null)
                throw new ArgumentNullException(nameof(testTarget));
            if (testParameter == null)
                throw new ArgumentNullException(nameof(testParameter));
            if (testResult == null)
                throw new ArgumentNullException(nameof(testResult));

            // Add a result line
            string result = $"{DateTime.UtcNow:s}, \"{testTarget.FileName}\", {testTarget.FileSize}, {testParameter.BlockSize}" +
                $", {testParameter.WriteRatio}, {testParameter.ThreadCount}, {testParameter.OutstandingOperations}" + 
                $", {testParameter.WarmupTime}, {testResult.Seconds}, {testResult.Bytes}, {testResult.Ios}";
            File.AppendAllText(FileName, result + Environment.NewLine);
        }

        public void AddFailedResult(DiskSpeedTarget testTarget, DiskSpeedParameter testParameter)
        {
            if (testTarget == null)
                throw new ArgumentNullException(nameof(testTarget));
            if (testParameter == null)
                throw new ArgumentNullException(nameof(testParameter));

            // Add a result line
            string result = $"{DateTime.UtcNow:s}, \"{testTarget.FileName}\", {testTarget.FileSize}, {testParameter.BlockSize}" +
                $", {testParameter.WriteRatio}, {testParameter.ThreadCount}, {testParameter.OutstandingOperations}" +
                $", {testParameter.WarmupTime}, 0, 0, 0";
            File.AppendAllText(FileName, result + Environment.NewLine);
        }

        public string FileName { get; }
        private const string Header = "UTC, Target, FileSize, BlockSize, WriteRatio, ThreadCount, OutstandingOperations, WarmupTime, TestTime, Bytes, IOS";
    }
}

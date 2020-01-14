using System;
using System.IO;

namespace DiskSpeedTest
{
    public static class Format
    {
        public const int KiB = 1024;
        public const int MiB = KiB * 1024;
        public const int GiB = MiB * 1024;

        public static string TimeStampFileName(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);
            string fileName = $"{DateTime.Now:yyyyMMddTHHmmss}_{Path.GetFileName(filePath)}";
            return Path.Combine(directory, fileName);
        }
    }
}

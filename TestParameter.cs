namespace DiskSpeedTest
{
    public static class Format
    {
        public const int KiB = 1024;
        public const int MiB = KiB * 1024;
        public const int GiB = MiB * 1024;
    }

    public class TestTarget
    {
        public string FileName;
        public long FileSize = 64L * Format.GiB;
    }

    public class TestParameter
    {
        public int BlockSize = 4 * Format.KiB;
        public int WriteRatio = 50;
        public int ThreadCount = 2;
        public int OutstandingOperations = 8;
        public int WarmupTime = 60;
        public int TestTime = 120;
    }
}

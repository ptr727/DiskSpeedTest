using System.IO;

namespace DiskSpeedTest
{
    public class TestTarget
    {
        public bool DoesTargetExist()
        {
            // Does the target exists and is it the right size
            return File.Exists(FileName) &&
                   new FileInfo(FileName).Length == FileSize;
        }

        public bool CreateTarget()
        {
            return DiskSpeed.CreateTestTarget(this) == 0;
        }

        public string FileName { get; set; }
        public long FileSize { get; } = 64L * Format.GiB;
    }
}

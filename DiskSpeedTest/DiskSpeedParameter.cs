using InsaneGenius.Utilities;

namespace DiskSpeedTest
{
    public class DiskSpeedParameter
    {
        public int BlockSize { get; set; } = 4 * Format.KiB;
        public string BlockSizeString => Format.BytesToKibi(BlockSize);
        public int WriteRatio { get; set; } = 50;
        public string WriteRatioString 
        { 
            get
            {
                return WriteRatio switch
                {
                    0 => "Read",
                    50 => "ReadWrite",
                    100 => "Write",
                    _ => $"{WriteRatio}"
                };
            }
        }
        public int ThreadCount { get; set; } = 2;
        public int OutstandingOperations { get; set; } = 8;
        public int WarmupTime { get; set; } = 30;
        public int TestTime { get; set; } = 120;
    }
}

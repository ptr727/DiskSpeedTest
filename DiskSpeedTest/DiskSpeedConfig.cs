using System.Collections.Generic;
using InsaneGenius.Utilities;
using Newtonsoft.Json;

namespace DiskSpeedTest
{
    public class DiskSpeedConfig
    {
        public DiskSpeedConfig()
        {
            Targets = new List<string>();
        }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; } = true;

        [JsonProperty("resultfile")]
        public string ResultFile { get; set; } = @"SpeedTestResult.csv";

        [JsonProperty("targets")]
        public List<string> Targets { get; }

        [JsonProperty("targetsize")]
        public long TargetSize { get; set; } = 64L * Format.GiB;

        [JsonProperty("blocksizebegin")]
        public int BlockSizeBegin { get; set; } = 4 * Format.KiB;

        [JsonProperty("blocksizeend")]
        public int BlockSizeEnd { get; set; } = 2 * Format.MiB;

        [JsonProperty("warmuptime")]
        public int WarmupTime { get; set; } = 30;

        [JsonProperty("testtime")]
        public int TestTime { get; set; } = 120;

        [JsonProperty("resttime")]
        public int RestTime { get; set; }
    }
}
